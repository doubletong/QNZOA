using QNZOA.Model;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface ICustomerService
    {
        Task<CustomerPagedVM> GetCustomersAsync(int page, int pageSize, string keywords);
    }
}