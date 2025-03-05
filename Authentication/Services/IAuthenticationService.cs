using Authentication.Entities;
using Authentication.Models;

namespace Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> Registering(RegisterModel model);
        Task<AuthenticationModel> Login(LoginModel model);
        Task<string> CreateToken(AppUser user);
    }
}
