namespace RazorEmailLibs.Views.Emails
{
    public class ResetPasswordViewModel
    {
        public ResetPasswordViewModel(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }
}