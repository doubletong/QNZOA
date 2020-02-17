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

        public async Task<CustomerPagedVM> GetCustomersAsync(int page, int pageSize,  string keywords,string orderBy,string orderMode,int customerType= 0)
        {
            var vm = new CustomerPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                OrderBy = orderBy,
                OrderMode = orderMode,
                CustomerType = customerType
            };

            var query = _db.Customers.AsNoTracking().Include(d => d.Projects).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Name.Contains(keywords) || d.Description.Contains(keywords) || d.Phone.Contains(keywords));
            }
            if (customerType > 0)
            {
                query = query.Where(d => d.CustomerType == customerType);
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
            vm.Customers = await query.Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<CustomerVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }

        public async Task<Customer> Get(int id)
        {
            return await _db.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<CustomerForSelectVM>> GetCustomersForSelectAsync()
        {
            return await _db.Customers.OrderByDescending(d=>d.Id)
                .ProjectTo<CustomerForSelectVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task CreateCustomerAsync(CustomerIM item)
        {
            var customer = _mapper.Map<Customer>(item);
            customer.CreatedDate = DateTime.Now;
            customer.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            await _db.AddAsync(customer);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(int id, CustomerIM item)
        {
            var origin = await _db.Customers.FindAsync(id);
            var customer = _mapper.Map(item,origin);

            //customer.CreatedDate = DateTime.Now;
            //customer.CreatedBy = await _sessionStorageService.GetItemAsync<string>("username");

            _db.Update(customer);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if(await _db.Projects.AnyAsync(d=>d.CustomerId == id))
            {
                throw new Exception("此客户还关联着项目，不能直接删除！");         
            }
            else
            {
                var c = await _db.Customers.FindAsync(id);
                _db.Customers.Remove(c);
                await _db.SaveChangesAsync();
            }

           
        }
    }
}
