using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Domain.Entities.Account;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserById
{
    public class GetOneUserByIdQueryHandler : IRequestHandler<GetOneUserByIdQuery, ResponseApi<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetOneUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<User>> Handle(GetOneUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetOneAsync(request.Id, cancellationToken);
            if (user is null)
                return ResponseApi<User>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            
            return ResponseApi<User>.ResponseOk(user);
        }
    }
}