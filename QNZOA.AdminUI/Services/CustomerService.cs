using Microsoft.EntityFrameworkCore;
using QNZOA.Data;
using QNZOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace QNZOA.AdminUI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        public CustomerService(SIGOAContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CustomerPagedVM> GetCustomersAsync(int page, int pageSize, string keywords)
        {
            var vm = new CustomerPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords
            };

            var query = _db.Customers.AsNoTracking().Include(d => d.Projects).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Name.Contains(keywords) || d.Description.Contains(keywords) || d.Phone.Contains(keywords));
            }

            vm.RowCount = await query.CountAsync();
            vm.Customers = await query.OrderByDescending(d => d.CreatedDate)
                .Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<CustomerVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }
    }
}
