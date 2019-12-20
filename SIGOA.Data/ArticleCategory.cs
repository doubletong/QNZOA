using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class ArticleCategory
    {
        public ArticleCategory()
        {
            Articles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Importance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public string UpdatedBy { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
