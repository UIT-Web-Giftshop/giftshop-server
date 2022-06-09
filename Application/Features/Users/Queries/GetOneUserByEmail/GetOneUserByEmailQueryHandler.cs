using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Domain.Entities.User;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserByEmail
{
    public class GetOneUserByEmailQueryHandler : IRequestHandler<GetOneUserByEmailQuery, ResponseApi<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetOneUserByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<User>> Handle(GetOneUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindOneAsync(x => x.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return ResponseApi<User>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }
            
            return ResponseApi<User>.ResponseOk(user);
        }
    }
}