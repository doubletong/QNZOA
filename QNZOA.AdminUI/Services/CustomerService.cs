using Microsoft.EntityFrameworkCore;
using QNZOA.Data;
using QNZOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blazored.SessionStorage;

namespace QNZOA.AdminUI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        private ISessionStorageService _sessionStorageService;
        public CustomerService(SIGOAContext db, IMapper mapper, ISessionStorageService sessionStorageService)
        {
            _db = db;
            _mapper = mapper;
            _sessionStorageService = sessionStorageService;
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

        public IQueryable<Customer> GetCustomers()
        {
            return _db.Customers.OrderByDescending(d=>d.Id).AsQueryable();
        }
        public async System.Threading.Tasks.Task CreateCustomerAsync(CustomerIM item)
        {
            var customer = _mapper.Map<Customer>(item);
            customer.CreatedDate = DateTime.Now;
            customer.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            await _db.AddAsync(customer);
            await _db.SaveChangesAsync();
        }
    }
}
