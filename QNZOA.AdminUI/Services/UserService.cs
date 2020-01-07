using Microsoft.EntityFrameworkCore;
using QNZOA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public class UserService : IUserService
    {
        private readonly SIGOAContext _db;
        public UserService(SIGOAContext db)
        {
            _db = db;
        }

        public User GetUserByUsername(string username)
        {
            return _db.Users.FirstOrDefault(d => d.Username == username);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(d => d.Username == username);
        }
    }
}
