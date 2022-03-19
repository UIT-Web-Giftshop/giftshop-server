using Application.Commons;
using Application.Features.Objects.Queries.GetPagingObjects;
using Application.Features.Users.Vms;
using Domain.Paging;
using MediatR;

namespace Application.Features.Users.Queries.GetPagingUsers
{
    public class GetPagingUsersQuery : GetPagingObjectsQuery, IRequest<ResponseApi<PagingModel<UserVm>>>
    {

    }
}