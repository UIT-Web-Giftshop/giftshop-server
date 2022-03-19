using Application.Features.Objects.Commands.Update.UpdateOneObject;
using Application.Features.Users.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Users.Commands.Update.UpdateOneUserInfo
{
    public class UpdateOneUserInfoHandler : UpdateOneObjectInfoHandler<User, UserVm>
    {
        public UpdateOneUserInfoHandler(IUserRepository _userRepository, IMapper _mapper) : 
            base(_userRepository, _mapper)
        {

        }
    }
}