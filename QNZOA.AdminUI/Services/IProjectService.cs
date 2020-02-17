using QNZOA.Data;
using QNZOA.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public interface IProjectService
    {
        Task AddProjectUsers(UserProject item);
        Task Archive(int id);
        Task CreateAsync(ProjectIM item);
        Task Delete(int id);

        Task<Project> Get(int id);
        Task<ProjectDetailVM> GetDetail(int id);
        Task<ProjectPagedVM> GetProjectsAsync(int page, int pageSize, string keywords, string orderBy, string orderMode, bool archive, int customerId = 0);
        Task<IEnumerable<UserForSelectVM>> GetProjectUsers(int id);
        Task RemoveProjectUsers(UserProject item);
        Task UnArchive(int id);
        Task UpdateAsync(int id, ProjectIM item);
    }
}