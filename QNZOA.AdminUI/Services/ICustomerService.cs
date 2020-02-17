using QNZOA.Data;
using QNZOA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface ICustomerService
    {
        Task CreateCustomerAsync(CustomerIM item);
        Task Delete(int id);
        Task<Customer> Get(int id);   
        Task<CustomerPagedVM> GetCustomersAsync(int page, int pageSize, string keywords, string orderBy, string orderMode, int customerType = 0);
        Task<IEnumerable<CustomerForSelectVM>> GetCustomersForSelectAsync();
        Task UpdateCustomerAsync(int id, CustomerIM item);
    }
}