using Application.Commons;
using Domain.ViewModels.Profile;
using MediatR;

namespace Application.Features.Profile.Queries.GetMyProfile
{
    public class GetMyProfileQuery : IRequest<ResponseApi<MyProfileViewModel>>
    {
        
    }
}