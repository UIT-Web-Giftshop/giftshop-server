using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Profile.Commands.UpdateProfileInfo
{
    public class UpdateProfileInfoCommandHandler : IRequestHandler<UpdateProfileInfoCommand, ResponseApi<Unit>>
    {
        private readonly IAccessorService _accessorService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateProfileInfoCommandHandler(IAccessorService accessorService, IUserRepository userRepository, IMapper mapper)
        {
            _accessorService = accessorService;
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<ResponseApi<Unit>> Handle(UpdateProfileInfoCommand request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var user = await _userRepository.FindOneAsync(x => x.Email == email, cancellationToken);

            var newUser = _mapper.Map(request, user);

            var replaced = await _userRepository.ReplaceOneAsync(
                user.Id,
                newUser,
                false,
                cancellationToken);

            if (replaced.AnyDocumentReplaced())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhập thành công");
            }
            
            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}