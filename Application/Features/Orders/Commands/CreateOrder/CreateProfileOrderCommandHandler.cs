﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateProfileOrderCommandHandler : IRequestHandler<CreateProfileOrderCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly IUserRepository _userRepository;

        public CreateProfileOrderCommandHandler(IProductRepository productRepository, IAccessorService accessorService,
            IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _accessorService = accessorService;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(CreateProfileOrderCommand request,
            CancellationToken cancellationToken)
        {
            var productsQuantityCursor = await _productRepository.FindAsync(
                x => request.OrderItems.Keys.Contains(x.Sku), // TODO: maybe need refactor
                x => new MinimalProductForOrder { Sku = x.Sku, Price = x.Price, Stock = x.Stock, Name = x.Name },
                cancellationToken: cancellationToken);

            var productList = await productsQuantityCursor.ToListAsync(cancellationToken);

            // pre check
            foreach (var product in productList)
            {
                var diff = product.Stock - request.OrderItems[product.Sku];
                if (diff < 0)
                {
                    return ResponseApi<Unit>.ResponseFail($"Mã sản phẩm {product.Sku} không đủ số lượng đặt hàng");
                }
            }

            // update stock
            var tasks = new List<Task>();
            foreach (var item in productList)
            {
                if (item is null) continue;
                tasks.Add(_productRepository.UpdateOneAsync(
                    x => x.Sku == item.Sku,
                    x => x.Set(p => p.Stock, item.Stock - request.OrderItems[item.Sku]),
                    cancellationToken: cancellationToken));
            }

            tasks.ForEach(x => x.Start());

            // create order
            var order = new Order
            {
                UserEmail = _accessorService.Email(),
                IsPaid = false,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            foreach (var item in productList)
            {
                order.AddItem(new OrderItem
                    { Price = item.Price, Quantity = request.OrderItems[item.Sku], ProductSku = item.Sku });
            }

            // push to task
            var addOrderTask = _orderRepository.InsertAsync(order, cancellationToken);
            tasks.Add(addOrderTask);

            // wait
            await Task.WhenAll(tasks);

            return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đặt hàng thành công");
        }
    }
}