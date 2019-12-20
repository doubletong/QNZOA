using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class LinkCategory
    {
        public LinkCategory()
        {
            Links = new HashSet<Link>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentCategoryId { get; set; }
        public int Importance { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public ICollection<Link> Links { get; set; }
    }
}
