using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Auths.VerifyToken.ConfirmEmail
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerifyTokenRepository _verifyTokenRepository;

        public ConfirmEmailQueryHandler(IUserRepository userRepository, IVerifyTokenRepository verifyTokenRepository)
        {
            _userRepository = userRepository;
            _verifyTokenRepository = verifyTokenRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}