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
    public class PaymentlogService : IPaymentlogService
    {
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        private ISessionStorageService _sessionStorageService;
        public PaymentlogService(SIGOAContext db, IMapper mapper, ISessionStorageService sessionStorageService)
        {
            _db = db;
            _mapper = mapper;
            _sessionStorageService = sessionStorageService;
        }
        #region ProjectBusiness
        public async Task<PaymentlogPagedVM> GetListAsync(int page, int pageSize, int? projectId, string orderBy, string orderMode)
        {
            var vm = new PaymentlogPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                ProjectId = projectId ?? 0,
                OrderBy = orderBy,
                OrderMode = orderMode,
            };

            var query = _db.Paymentlogs.Include(d => d.Project).AsQueryable();
     
            if (projectId > 0)
            {
                query = query.Where(d => d.ProjectId == projectId);             
            }

            vm.RowCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "money":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.Money);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.Money);
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

          
            vm.Paymentlogs = await query.Skip(vm.PageIndex * vm.PageSize)
                .Take(vm.PageSize)
                .ProjectTo<PaymentlogVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;

        }

        public async Task<Paymentlog> Get(int id)
        {
            return await _db.Paymentlogs.Include(d => d.Project).FirstOrDefaultAsync(d => d.ProjectId == id);
        }

        public async Task<PaymentlogIM> Edit(int id)
        {
            var log = await _db.Paymentlogs.Include(d=>d.Project).FirstOrDefaultAsync(d => d.Id == id);
            return _mapper.Map<PaymentlogIM>(log);
        }

        public async Task CreateAsync(PaymentlogIM item)
        {
            var pl = _mapper.Map<Paymentlog>(item);
            pl.CreatedDate = DateTime.Now;
            pl.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            await _db.AddAsync(pl);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PaymentlogIM item)
        {
            var origin = await _db.Paymentlogs.FindAsync(id);
            var pl = _mapper.Map(item, origin);

            _db.Update(pl);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {

            var c = await _db.Paymentlogs.FindAsync(id);
            _db.Paymentlogs.Remove(c);
            await _db.SaveChangesAsync();


        }
        #endregion
    }
}
