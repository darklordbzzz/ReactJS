using System.Threading.Tasks;

namespace Jules02.Core.Services
{
    public interface IApiClient
    {
        Task<string> GetAccessTokenAsync();
        Task<string> GetAsync(string endpoint);
    }
}