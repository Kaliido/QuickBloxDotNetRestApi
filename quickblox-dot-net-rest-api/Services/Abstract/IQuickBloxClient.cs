using System.Threading.Tasks;
using Kaliido.QuickBlox.Models;
using Kaliido.QuickBlox.Parameters;
using Kaliido.QuickBlox.Responses;

namespace Kaliido.QuickBlox
{
    public interface IQuickBloxClient
    {
        Task<string> GetQBToken();
        Task<QuickbloxUser> RegisterAsync(UserParameters userParameters);
        Task<string> GetApplicationToken();
        Task<QuickbloxUser> RegisterByExternal(string provider, string scope, string token);
        Task<QuickbloxUser> UpdateUserAsync(long userId, UserParameters userParameters);
        Task<QuickbloxUser> RemoveExternalLogin(long userId, UserParameters userParameters);
        Task<dynamic> GetUsersClosest(double lat, double longitude);
        Task<UsersResponse> GetAllUsers();
        Task<QuickbloxUser> GetUser(string userLogin);
    }
}