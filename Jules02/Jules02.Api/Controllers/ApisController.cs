using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Jules02.Core.Models;

namespace Jules02.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApisController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ApiConfiguration> Get()
        {
            // In the future, this will be loaded from a configuration file or database.
            return new List<ApiConfiguration>
            {
                new ApiConfiguration
                {
                    Name = "Spotify",
                    BaseUrl = "https://api.spotify.com/v1",
                    OAuthConfig = new OAuthConfig
                    {
                        TokenUrl = "https://accounts.spotify.com/api/token"
                    }
                }
            };
        }
    }
}