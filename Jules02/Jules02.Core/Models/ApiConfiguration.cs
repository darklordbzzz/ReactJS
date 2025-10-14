namespace Jules02.Core.Models
{
    public class ApiConfiguration
    {
        public string? Name { get; set; }
        public string? BaseUrl { get; set; }
        public OAuthConfig? OAuthConfig { get; set; }
    }
}