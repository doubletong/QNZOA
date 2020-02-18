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
    public class FinanceService : IFinanceService
    {
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        private ISessionStorageService _sessionStorageService;
        public FinanceService(SIGOAContext db, IMapper mapper, ISessionStorageService sessionStorageService)
        {
            _db = db;
            _mapper = mapper;
            _sessionStorageService = sessionStorageService;
        }
        #region ProjectBusiness
        public async Task<ProjectBusinessPagedVM> GetListAsync(int page, int pageSize, string keywords, string orderBy, string orderMode)
        {
            var vm = new ProjectBusinessPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                OrderBy = orderBy,
                OrderMode = orderMode,
            };

            var query = _db.ProjectBusinesses.Include(d => d.Project).Include(d => d.Paymentlogs).AsQueryable();
            var queryPL = _db.Paymentlogs.Where(d => d.Money > 0).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Project.Name.Contains(keywords));
                queryPL = queryPL.Where(d => d.Project.Name.Contains(keywords));
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "amount":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.Amount);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.Amount);
                        }
                        break;
                    case "createdDate":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.Project.CreatedDate);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.Project.CreatedDate);
                        }
                        break;
                    default:
                        query = query.OrderByDescending(d => d.Project.CreatedDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(d => d.Project.CreatedDate);
            }

            vm.RowCount = await query.CountAsync();
            vm.TotalAmount = await query.SumAsync(d => d.Amount);
            vm.TotalPaymented = await queryPL.SumAsync(d => d.Money);
            vm.ProjectBusinesses = await query.Skip(vm.PageIndex * vm.PageSize)
                .Take(vm.PageSize)
                .ProjectTo<ProjectBusinessVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }

        public async Task<ProjectBusiness> Get(int id)
        {
            return await _db.ProjectBusinesses.Include(d=>d.Project).FirstOrDefaultAsync(d=>d.ProjectId==id);
        }

        public async Task CreateAsync(ProjectBusinessIM item)
        {
            var project = _mapper.Map<ProjectBusiness>(item);
            //project.CreatedDate = DateTime.Now;
            //project.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            await _db.AddAsync(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProjectBusinessIM item)
        {
            var origin = await _db.ProjectBusinesses.FindAsync(id);
            var project = _mapper.Map(item, origin);      

            _db.Update(project);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (await _db.Paymentlogs.AnyAsync(d => d.ProjectId == id))
            {
                throw new Exception("此项目还关联着流水帐，不能直接删除！");
            }
            else
            {
                var c = await _db.ProjectBusinesses.FindAsync(id);
                _db.ProjectBusinesses.Remove(c);
                await _db.SaveChangesAsync();
            }


        }
        #endregion
    }
}
