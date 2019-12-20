using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class ProjectBusiness
    {
        public ProjectBusiness()
        {
            Paymentlogs = new HashSet<Paymentlog>();
        }

        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public string Contract { get; set; }

        public Project Project { get; set; }
        public ICollection<Paymentlog> Paymentlogs { get; set; }
    }
}
