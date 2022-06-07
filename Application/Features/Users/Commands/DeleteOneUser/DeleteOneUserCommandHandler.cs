using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Users.Commands.DeleteOneUser
{
    public class DeleteOneUserCommandHandler : IRequestHandler<DeleteOneUserCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;

        public DeleteOneUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(DeleteOneUserCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _userRepository.DeleteOneAsync(request.Id, cancellationToken);
            
            if (deleted.AnyDocumentDeleted()) 
                return ResponseApi<Unit>.ResponseOk(Unit.Value);
            
            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}