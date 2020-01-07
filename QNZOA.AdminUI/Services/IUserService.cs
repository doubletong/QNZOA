using QNZOA.Data;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface IUserService
    {
        User GetUserByUsername(string username);
        Task<User> GetUserByUsernameAsync(string username);
    }
}