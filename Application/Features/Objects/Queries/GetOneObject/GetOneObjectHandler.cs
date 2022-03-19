﻿using Application.Features.Handlers;
using AutoMapper;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Objects.Queries.GetOneObject
{
    public abstract class GetOneObjectHandler<T> : HandlerOfOneObject<T> where T : class
    {
        public GetOneObjectHandler(IBaseRepository<T> _baseRepository, IMapper _mapper) : 
            base(_baseRepository, _mapper)
        {

        }
    }
}