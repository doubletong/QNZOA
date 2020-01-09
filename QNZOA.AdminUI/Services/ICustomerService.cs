using QNZOA.Data;
using QNZOA.Model;
using System.Linq;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface ICustomerService
    {
        System.Threading.Tasks.Task CreateCustomerAsync(CustomerIM item);
        IQueryable<Customer> GetCustomers();
        Task<CustomerPagedVM> GetCustomersAsync(int page, int pageSize, string keywords);
    }
}