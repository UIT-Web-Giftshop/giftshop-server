using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using RazorEmailLibs.Constants;
using RazorEmailLibs.Views.Emails;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateProfileOrderCommandHandler : IRequestHandler<CreateProfileOrderCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IDiscountService _discountService;
        private readonly IMailService _mailService;

        public CreateProfileOrderCommandHandler(IProductRepository productRepository, IAccessorService accessorService,
            IUserRepository userRepository, IOrderRepository orderRepository, ICartRepository cartRepository,
            IDiscountService discountService, IMailService mailService)
        {
            _productRepository = productRepository;
            _accessorService = accessorService;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _discountService = discountService;
            _mailService = mailService;
        }

        public async Task<ResponseApi<Unit>> Handle(CreateProfileOrderCommand request,
            CancellationToken cancellationToken)
        {
            var cartId = _accessorService.GetHeader("cartId");

            if (string.IsNullOrEmpty(cartId))
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status403Forbidden);

            // lay product tu cart
            var userCart = await _cartRepository.GetOneAsync(cartId);
            var userCartItems = userCart.Items;

            if (userCartItems.Count <= 0)
                return ResponseApi<Unit>.ResponseFail("Giỏ hàng trống");

            var orderItems = new Dictionary<string, int>();
            foreach (var item in userCartItems)
            {
                orderItems.Add(item.Sku, item.Quantity);
                userCartItems.Remove(item);
            }

            var productsQuantityCursor = await _productRepository.FindAsync(
                x => orderItems.Keys.Contains(x.Sku), // TODO: maybe need refactor
                x => new MinimalProductForOrder
                    { Sku = x.Sku, Price = x.Price, Stock = x.Stock, Name = x.Name, ImageUrl = x.ImageUrl },
                cancellationToken: cancellationToken);

            var productList = await productsQuantityCursor.ToListAsync(cancellationToken);

            // pre check
            foreach (var product in productList)
            {
                var diff = product.Stock - orderItems[product.Sku];
                if (diff < 0)
                {
                    return ResponseApi<Unit>.ResponseFail($"Mã sản phẩm {product.Sku} không đủ số lượng đặt hàng");
                }
            }

            // update stock
            var tasks = new Task[productList.Count + 2];
            var index = 0;
            foreach (var item in productList)
            {
                if (item is null) continue;
                tasks[index++] = (_productRepository.UpdateOneAsync(
                    x => x.Sku == item.Sku,
                    x => x.Set(p => p.Stock, item.Stock - orderItems[item.Sku]),
                    cancellationToken: cancellationToken));
            }

            // create order
            var order = new Order
            {
                UserEmail = _accessorService.Email(),
                IsPaid = false,
                Status = nameof(OrderStatus.Pending),
                CreatedAt = DateTime.UtcNow,
                Address = request.Address
            };
            foreach (var item in productList)
            {
                order.AddItem(new OrderItem
                {
                    Price = item.Price, Quantity = orderItems[item.Sku], Sku = item.Sku,
                    Name = item.Name, ImageUrl = item.ImageUrl
                });
            }

            order.TotalPaid = order.TotalPrice;
            _discountService.ApplyDiscount(order, request.CouponCode);

            // push insert order to task
            tasks[index++] = _orderRepository.InsertAsync(order, cancellationToken);
            tasks[index] = _cartRepository.UpdateOneAsync(
                cartId,
                x => x.Set(o => o.Items, userCartItems));

            // wait
            Task.WaitAll(tasks);

            // send mail
            await _mailService.SendWithTemplate(order.UserEmail, "Hóa đơn đặt hàng", new List<IFormFile>(),
                MailTemplatesName.RECEIPT_EMAIL, new ReceiptEmailViewModel(order));

            return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đặt hàng thành công");
        }
    }
}