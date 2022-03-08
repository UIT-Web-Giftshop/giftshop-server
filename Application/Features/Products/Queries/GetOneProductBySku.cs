﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Queries
{
    public class GetOneProductBySku
    {
        public class Query : IRequest<ResponseApi<ProductVm>>
        {
            public string Sku { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, ResponseApi<ProductVm>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ResponseApi<ProductVm>> Handle(Query request, CancellationToken cancellationToken)
            {
                Expression<Func<Product, bool>> expression = p => p.Sku == request.Sku;
                var product = await _productRepository.GetOneAsync(expression, cancellationToken);

                if (product == null)
                {
                    return ResponseApi<ProductVm>.ResponseFail(StatusCodes.Status400BadRequest, ResponseConstants.ERROR_NOT_FOUND_ITEM);
                }
                var data = _mapper.Map<ProductVm>(product);
                return ResponseApi<ProductVm>.ResponseOk(data);
            }
        }
    }
}