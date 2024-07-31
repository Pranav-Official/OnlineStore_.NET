using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IAuthenticationService
    {
        void Register(Users user);
        bool Login(string userName, string password, out string role);

        string GenerateJwt(string userName, string password, string role);
    }
}
