using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Link
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

        public LinkCategory Category { get; set; }
    }
}
