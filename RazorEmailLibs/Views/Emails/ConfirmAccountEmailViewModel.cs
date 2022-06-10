namespace RazorEmailLibs.Views.Emails
{
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel(string confirmUrl, string resendUrl)
        {
            ConfirmUrl = confirmUrl;
            ResendUrl = resendUrl;
        }

        public string ConfirmUrl { get; set; }
        public string ResendUrl { get; set; }
    }
}