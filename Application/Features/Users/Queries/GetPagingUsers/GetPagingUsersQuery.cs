﻿using System.ComponentModel;
using Application.Commons;
using Domain.Entities;
using Domain.Entities.Account;
using Domain.Paging;
using MediatR;

namespace Application.Features.Users.Queries.GetPagingUsers
{
    public class GetPagingUsersQuery : IRequest<ResponseApi<PagingModel<User>>>
    {
        [DefaultValue(1)] public int PageIndex { get; set; } = 1;

        [DefaultValue(20)] public int PageSize { get; set; } = 20;
        
        public string SearchBy { get; set; }
        
        public string Search { get; set; }
        
        [DefaultValue("email")] public string SortBy { get; set; }
        
        [DefaultValue(true)] public bool IsDesc { get; set; }
    }
}