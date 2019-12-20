using QNZOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface IAccountService
    {
        public Task<ReturnVM> LoginAsync(LoginIM loginIM);
    }
}
