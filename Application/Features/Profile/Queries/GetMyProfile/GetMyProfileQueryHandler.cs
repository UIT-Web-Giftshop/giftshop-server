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
        private readonly IAccessor _accessor;
        private readonly IMapper _mapper;

        public GetMyProfileQueryHandler(IUserRepository userRepository, IAccessor accessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _accessor = accessor;
            _mapper = mapper;
        }
        
        public async Task<ResponseApi<MyProfileViewModel>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var email = _accessor.Email();
            var profile = await _userRepository.FindOneAsync(x => x.Email == email, cancellationToken);
            return ResponseApi<MyProfileViewModel>.ResponseOk(_mapper.Map<MyProfileViewModel>(profile));
        }
    }
}