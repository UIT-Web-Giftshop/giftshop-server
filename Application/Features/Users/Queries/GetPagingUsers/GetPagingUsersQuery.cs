using System.ComponentModel;
using Application.Commons;
using Domain.Entities;
using Domain.Paging;
using MediatR;

namespace Application.Features.Users.Queries.GetPagingUsers
{
    // public class GetPagingUsersQuery : GetPagingObjectsQuery, IRequest<ResponseApi<PagingModel<UserVm>>>
    // {
    //
    // }

    public class GetPagingUsersQuery : IRequest<ResponseApi<PagingModel<User>>>
    {
        [DefaultValue(1)] public int PageIndex { get; set; } = 1;

        [DefaultValue(20)] public int PageSize { get; set; } = 20;
    }
}