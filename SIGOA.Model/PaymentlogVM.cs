using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Model
{
    public class PaymentlogVM
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }

    public class PaymentlogPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int ProjectId { get; set; }
        public IEnumerable<PaymentlogVM> Paymentlogs { get; set; }

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

    public class PaymentlogIM
    {
        public int Id { get; set; }

        /// <summary>
        /// //1:收入 2：支出
        /// </summary>
        public int PayType { get; set; } 
        public decimal Money { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ProjectId { get; set; }
    }
}
