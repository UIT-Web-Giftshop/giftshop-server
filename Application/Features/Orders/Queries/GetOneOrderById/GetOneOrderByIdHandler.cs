﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Orders.Vms;
using AutoMapper;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Orders.Queries.GetOneOrderById
{
    public class GetOneOrderByIdHandler : GetOneObjectHandler<Order>, 
        IRequestHandler<GetOneOrderByIdQuery, ResponseApi<OrderVm>>
    {
        public GetOneOrderByIdHandler(IOrderRepository _orderRepository, IMapper _mapper) :
            base(_orderRepository, _mapper)
        {

        }

        public async Task<ResponseApi<OrderVm>> Handle(GetOneOrderByIdQuery request, 
            CancellationToken cancellationToken)
        {
            Expression<Func<Order, bool>> expression = p => p.Id == request.Id;
            var order = await this._baseRepository.GetOneAsync(expression, cancellationToken);

            if (order == null)
            {
                return ResponseApi<OrderVm>.ResponseFail(StatusCodes.Status400BadRequest,
                    ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var data = this._mapper.Map<OrderVm>(order);
            return ResponseApi<OrderVm>.ResponseOk(data);
        }
    }
}