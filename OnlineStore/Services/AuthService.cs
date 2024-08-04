using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IAuthenticationService
    {
        Task Register(Users user);
        Task<Users> LoginAsync(string userName, string password);

    }
}
