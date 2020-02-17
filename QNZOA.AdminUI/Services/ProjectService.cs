using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;
using QNZOA.Data;
using QNZOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        private ISessionStorageService _sessionStorageService;
        public ProjectService(SIGOAContext db, IMapper mapper, ISessionStorageService sessionStorageService)
        {
            _db = db;
            _mapper = mapper;
            _sessionStorageService = sessionStorageService;
        }


        public async Task<ProjectPagedVM> GetProjectsAsync(int page,
                                                           int pageSize,
                                                           string keywords,
                                                           string orderBy,
                                                           string orderMode,
                                                           bool archive,
                                                           int customerId = 0)
        {
            var vm = new ProjectPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                OrderBy = orderBy,
                OrderMode = orderMode,
                CustomerId = customerId
            };

            var query = _db.Projects.AsNoTracking().Include(d => d.Customer)
                .Include(d => d.UserProjects).Include(d => d.ManagerNavigation)
                .Include(d => d.TaskLists).Where(d=>d.Archive == archive).AsQueryable();

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Name.Contains(keywords) || d.Description.Contains(keywords));
            }

            if (customerId > 0)
            {
                query = query.Where(d => d.CustomerId == customerId);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "name":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.Name);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.Name);
                        }
                        break;
                    case "createdDate":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.CreatedDate);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.CreatedDate);
                        }
                        break;
                    default:
                        query = query.OrderByDescending(d => d.CreatedDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(d => d.CreatedDate);
            }



            vm.RowCount = await query.CountAsync();
            vm.Projects = await query.Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<ProjectVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }


        public async Task<ProjectDetailVM> GetDetail(int id)
        {
            var p = await _db.Projects.Include(d => d.ManagerNavigation).Include(d => d.Customer).FirstOrDefaultAsync(d => d.Id == id);
            if (p == null)
                return null;

            var vm = _mapper.Map<ProjectDetailVM>(p);
            return vm;
        }

        public async Task<Project> Get(int id)
        {
            return await _db.Projects.FindAsync(id);           
        }
        public async Task<IEnumerable<UserForSelectVM>> GetProjectUsers(int id)
        {
            var users = await _db.Users.Where(d => d.UserProjects.Any(u => u.ProjectId == id))
                .ProjectTo<UserForSelectVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return users;
        }

        public async Task AddProjectUsers(UserProject item)
        {
            if(!_db.UserProjects.Any(d=>d.ProjectId == item.ProjectId && d.UserId == item.UserId))
            {
                _db.UserProjects.Add(item);
                await _db.SaveChangesAsync();
            }
                   
        }
        public async Task RemoveProjectUsers(UserProject item)
        {
           
                _db.UserProjects.Remove(item);
                await _db.SaveChangesAsync();
            
        }
        public async Task UnArchive(int id)
        {      
          
            var c = await _db.Projects.FindAsync(id);
            c.Archive = false;
            c.ArchiveDate = null;

            _db.Projects.Update(c);
            await _db.SaveChangesAsync();    

        }

        public async Task Archive(int id)
        {

            var c = await _db.Projects.FindAsync(id);
            c.Archive = true;
            c.ArchiveDate = DateTime.Now;

            _db.Projects.Update(c);
            await _db.SaveChangesAsync();

        }

        public async Task CreateAsync(ProjectIM item)
        {
            var project = _mapper.Map<Project>(item);
            project.CreatedDate = DateTime.Now;
            project.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            await _db.AddAsync(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProjectIM item)
        {
            var origin = await _db.Projects.FindAsync(id);
            var project = _mapper.Map(item, origin);

            //customer.CreatedDate = DateTime.Now;
            //customer.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            _db.Update(project);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (await _db.TaskLists.AnyAsync(d => d.ProjectId == id))
            {
                throw new Exception("此客户还关联着任务，不能直接删除！");
            }
            else
            {
                var c = await _db.Projects.FindAsync(id);
                _db.Projects.Remove(c);
                await _db.SaveChangesAsync();
            }


        }

    }
}
