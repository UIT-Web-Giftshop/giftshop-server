using Domain.ViewModels.Profile;

namespace Domain.ViewModels.Auth
{
    public class SignInResponseViewModel
    {
        public string AccessToken { get; set; }
        public MyProfileViewModel Profile { get; set; }
    }
}