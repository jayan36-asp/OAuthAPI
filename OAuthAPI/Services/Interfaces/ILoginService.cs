using OAuthAPI.Models;
using OAuthAPI.Services.Service;

namespace OAuthAPI.Services.Interfaces
{
    public interface ILoginService
    {
        public loginResponse GenerateJwtToken(LoginRequest request);
    }
}
