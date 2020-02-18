using QNZOA.Data;
using QNZOA.Model;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface IPaymentlogService
    {
        Task CreateAsync(PaymentlogIM item);
        Task Delete(int id);
        Task<PaymentlogIM> Edit(int id);
        Task<Paymentlog> Get(int id);
        Task<PaymentlogPagedVM> GetListAsync(int page, int pageSize, int? projectId, string orderBy, string orderMode);
        Task UpdateAsync(int id, PaymentlogIM item);
    }
}