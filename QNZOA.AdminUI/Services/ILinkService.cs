using QNZOA.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface ILinkService
    {
        Task DeleteCategory(int id);
        Task DeleteLink(int id);
        Task<LinkCategoryPagedVM> GetCategoriesAsync(int page, int pageSize, string keywords, string orderBy, string orderMode);
        Task<IEnumerable<LinkCategoryForSelectVM>> GetForLinkCategorySelectAsync();
        Task<LinkPagedVM> GetLinksAsync(int page, int pageSize, string keywords, string orderBy, string orderMode, int categoryId = 0);
    }
}