using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Queries.GetPagingObjects;
using Application.Features.Users.Vms;
using AutoMapper;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetPagingUsers
{
    public class GetPagingUsersHandler : GetPagingObjectsHandler<User, UserVm>,
        IRequestHandler<GetPagingUsersQuery, ResponseApi<PagingModel<UserVm>>>
    {
        public GetPagingUsersHandler(IUserRepository _userRepository, IMapper _mapper,
            ISaveFlagRepository _saveFlagRepository) : base(_userRepository, _mapper, _saveFlagRepository)
        {

        }

        public override Expression<Func<User, bool>> GetExpression(GetPagingObjectsQuery request)
        {
            return string.IsNullOrWhiteSpace(request.Search) ? null : p =>
                p.GetFullName().Contains(request.Search);
        }

        public override Expression<Func<User, object>> GetSortExpression(GetPagingObjectsQuery
            request)
        {
            return q => q.DateOfBirth;
        }

        public async Task<ResponseApi<PagingModel<UserVm>>> Handle(GetPagingUsersQuery request,
           CancellationToken cancellationToken)
        {
            return await base.Handle(request, cancellationToken);
        }
    }
}