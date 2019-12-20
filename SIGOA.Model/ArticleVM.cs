using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Model
{

    #region Article
    public class ArticleVM
    {
        public int Id { get; set; }
        public string Title { get; set; }    
        public int Viewcount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }       
        public bool? Recommend { get; set; }
        public bool? Active { get; set; }
        public string Thumbnail { get; set; }
    }


    public class ArticlePagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public int CategoryId { get; set; }
        public IList<ArticleVM> ItemList { get; set; }

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

    public class ArticleIM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Keywords { get; set; }
        public int Viewcount { get; set; }
        public int CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool Recommend { get; set; }
        public bool Active { get; set; }
        public string Thumbnail { get; set; }

    }

    public class ArticleDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Viewcount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? Recommend { get; set; }
        public bool? Active { get; set; }
        public string Thumbnail { get; set; }
    }

    #endregion


    #region Category
    public class ArticleCategoryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Importance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
    }


    public class ArticleCategoryPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IList<ArticleCategoryVM> ItemList{ get; set; }

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

    public class ArticleCategoryIM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Importance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public string UpdatedBy { get; set; }

    }

    #endregion
}
