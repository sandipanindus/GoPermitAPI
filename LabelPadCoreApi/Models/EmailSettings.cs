

namespace LabelPadCoreApi.Models
{
    public class EmailSettings
    {
        public string AdminMail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SMTP_Host { get; set; }
        public int SMTP_Port { get; set; }
        public bool SSL { get; set; }
        public bool UseAuthentication { get; set; }
        public string AuthenticationName { get; set; }
        public string AuthenticationPassword { get; set; }
        public OAuthSettings OAuth { get; set; }
    }
    public class OAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string AuthUrl { get; set; }
        public string TokenUrl { get; set; }
    }
}
