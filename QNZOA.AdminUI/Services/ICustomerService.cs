using QNZOA.Data;
using QNZOA.Model;
using System.Linq;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface ICustomerService
    {
        System.Threading.Tasks.Task CreateCustomerAsync(CustomerIM item);
        Task<Customer> Get(int id);
        IQueryable<Customer> GetCustomers();
        Task<CustomerPagedVM> GetCustomersAsync(int page, int pageSize, string keywords);
        System.Threading.Tasks.Task UpdateCustomerAsync(int id, CustomerIM item);
    }
}