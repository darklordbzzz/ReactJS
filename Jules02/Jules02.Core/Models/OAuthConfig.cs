namespace Jules02.Core.Models
{
    public class OAuthConfig
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? TokenUrl { get; set; }
        public string? Scope { get; set; }
        public string GrantType { get; set; } = "client_credentials";
    }
}