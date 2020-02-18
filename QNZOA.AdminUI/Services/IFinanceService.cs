using QNZOA.Data;
using QNZOA.Model;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface IFinanceService
    {
        Task CreateAsync(ProjectBusinessIM item);
        Task Delete(int id);
        Task<ProjectBusiness> Get(int id);
        Task<ProjectBusinessPagedVM> GetListAsync(int page, int pageSize, string keywords, string orderBy, string orderMode);
        Task UpdateAsync(int id, ProjectBusinessIM item);
    }
}