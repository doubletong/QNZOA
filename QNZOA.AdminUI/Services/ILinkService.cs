using QNZOA.Data;
using QNZOA.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface ILinkService
    {
        Task CreateCategoryAsync(LinkCategoryIM item);
        Task CreateLinkAsync(LinkIM item);
        Task DeleteCategory(int id);
        Task DeleteLink(int id);
        Task<LinkCategoryPagedVM> GetCategoriesAsync(int page, int pageSize, string keywords, string orderBy, string orderMode);
        Task<LinkCategory> GetCategoryById(int id);
        Task<IEnumerable<LinkCategoryForSelectVM>> GetForLinkCategorySelectAsync();
        Task<Link> GetLinkById(int id);
        Task<LinkPagedVM> GetLinksAsync(int page, int pageSize, string keywords, string orderBy, string orderMode, int categoryId = 0);
        Task UpdateCategoryAsync(int id, LinkCategoryIM item);
        Task UpdateLinkAsync(int id, LinkIM item);
    }
}