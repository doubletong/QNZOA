using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Model
{
    #region Link
    public class LinkVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WebLink { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string LogoUrl { get; set; }
    }


    public class LinkPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public int CategoryId { get; set; }
        public IList<LinkVM> ItemList { get; set; }

        public int LastPageIndex
        {
            get
            {
                if (PageSize > 0)
                {
                    var d = RowCount / PageSize;
                    return (int)Math.Floor((double)d);
                }
                return 0;
            }
        }
    }

    public class LinkIM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WebLink { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CategoryId { get; set; }
        public string LogoUrl { get; set; }

    }

    #endregion
    #region Category
    public class LinkCategoryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Importance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
    }


    public class LinkCategoryPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IList<LinkCategoryVM> ItemList { get; set; }

        public int LastPageIndex
        {
            get
            {
                if (PageSize > 0)
                {
                    var d = RowCount / PageSize;
                    return (int)Math.Floor((double)d);
                }
                return 0;
            }
        }
    }

    public class LinkCategoryIM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int Importance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public string UpdatedBy { get; set; }

    }

    #endregion
}
