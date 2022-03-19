using Application.Features.Objects.Commands.Add;
using Application.Features.Users.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Users.Commands.Add
{
    public class AddOneUserHandler : AddOneObjectHandler<User, UserVm>
    {
        public AddOneUserHandler(IUserRepository _userRepository, IMapper _mapper) :
            base(_userRepository, _mapper)
        {

        }
    }
}