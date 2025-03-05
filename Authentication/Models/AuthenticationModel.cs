namespace Authentication.Models
{
    public class AuthenticationModel
    {
        public Dictionary<string , List<string>> Errors { get; set; }

        public string Message { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsAuthenticated { get; set; } = false;

        public string Token { get; set; } = string.Empty;

        public List<string> Roles { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}
