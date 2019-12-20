using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Paymentlog
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ProjectId { get; set; }

        public Project Project { get; set; }
        public ProjectBusiness ProjectNavigation { get; set; }
    }
}
