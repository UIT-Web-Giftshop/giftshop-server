using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Users.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserById
{
    public class GetOneUserByIdHandler : GetOneObjectHandler<User>, IRequestHandler<GetOneUserByIdQuery, ResponseApi<UserVm>>
    {
        public GetOneUserByIdHandler(IUserRepository _userRepository, IMapper _mapper) :
            base(_userRepository, _mapper)
        {

        }

        public async Task<ResponseApi<UserVm>> Handle(GetOneUserByIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> expression = p => p.Id == request.Id;
            var user = await this._baseRepository.GetOneAsync(expression, cancellationToken);
            var data = this._mapper.Map<UserVm>(user);
            return ResponseApi<UserVm>.ResponseOk(data);
        }
    }
}