using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.ViewModels.Profile;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Profile.Queries.GetMyProfile
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, ResponseApi<MyProfileViewModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccessorService _accessorService;
        private readonly IMapper _mapper;

        public GetMyProfileQueryHandler(IUserRepository userRepository, IAccessorService accessorService, IMapper mapper)
        {
            _userRepository = userRepository;
            _accessorService = accessorService;
            _mapper = mapper;
        }
        
        public async Task<ResponseApi<MyProfileViewModel>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var profile = await _userRepository.FindOneAsync(x => x.Email == email, cancellationToken);
            return ResponseApi<MyProfileViewModel>.ResponseOk(_mapper.Map<MyProfileViewModel>(profile));
        }
    }
}