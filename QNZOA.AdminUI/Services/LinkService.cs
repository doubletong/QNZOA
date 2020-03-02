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
    public class LinkService : ILinkService
    {
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        private ISessionStorageService _sessionStorageService;
        public LinkService(SIGOAContext db, IMapper mapper, ISessionStorageService sessionStorageService)
        {
            _db = db;
            _mapper = mapper;
            _sessionStorageService = sessionStorageService;
        }

        #region link categories
        public async Task<IEnumerable<LinkCategoryForSelectVM>> GetForLinkCategorySelectAsync()
        {

            return await _db.LinkCategories.AsNoTracking()
                .OrderByDescending(d => d.Id)
                .Select(d => new LinkCategoryForSelectVM { Id = d.Id, Title = d.Title })
                .ToListAsync();

        }

        public async Task<LinkCategoryPagedVM> GetCategoriesAsync(int page,
                                                           int pageSize,
                                                           string keywords,
                                                           string orderBy,
                                                           string orderMode)
        {
            var vm = new LinkCategoryPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                OrderBy = orderBy,
                OrderMode = orderMode

            };

            var query = _db.LinkCategories.AsNoTracking().Include(d => d.Links).AsQueryable();

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords));
            }


            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "title":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.Title);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.Title);
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
            vm.ItemList = await query.Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<LinkCategoryVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }


        public async Task DeleteCategory(int id)
        {
            if (await _db.Links.AnyAsync(d => d.CategoryId == id))
            {
                throw new Exception("此分类下还有链接，不能直接删除！");
            }
            else
            {
                var c = await _db.LinkCategories.FindAsync(id);
                _db.LinkCategories.Remove(c);
                await _db.SaveChangesAsync();
            }


        }

        #endregion

        #region links

       
        public async Task<LinkPagedVM> GetLinksAsync(int page,
                                                           int pageSize,
                                                           string keywords,
                                                           string orderBy,
                                                           string orderMode,                                                     
                                                           int categoryId = 0)
        {
            var vm = new LinkPagedVM
            {
                PageIndex = page - 1,
                PageSize = pageSize,
                Keywords = keywords,
                OrderBy = orderBy,
                OrderMode = orderMode,
                CategoryId = categoryId
            };

            var query = _db.Links.AsNoTracking().Include(d => d.Category).AsQueryable();

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d => d.Title.Contains(keywords) || d.Description.Contains(keywords));
            }

            if (categoryId > 0)
            {
                query = query.Where(d => d.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "title":
                        if (orderMode == "desc")
                        {
                            query = query.OrderByDescending(d => d.Title);
                        }
                        else
                        {
                            query = query.OrderBy(d => d.Title);
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
            vm.ItemList = await query.Skip(vm.PageIndex * vm.PageSize).Take(vm.PageSize).ProjectTo<LinkVM>(_mapper.ConfigurationProvider).ToListAsync();

            return vm;
        }



        public async Task DeleteLink(int id)
        {          
            var c = await _db.Links.FindAsync(id);
            _db.Links.Remove(c);
            await _db.SaveChangesAsync();            
        }


        #endregion
    }
}
