using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Article
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

        public ArticleCategory Category { get; set; }
    }
}
