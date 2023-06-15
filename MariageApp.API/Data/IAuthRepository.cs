using MariageApp.API.Models;
using System.Threading.Tasks;

namespace MariageApp.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user,string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}