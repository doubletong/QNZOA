using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SIGOA.Model
{
    #region  project

    public class ProjectForSelectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
      
    }
    public class ProjectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? CustomerId { get; set; }
        public Guid? Manager { get; set; }
        public string CustomerName { get; set; }
        public string ManagerName { get; set; }
        public int TaskCount { get; set; }       
        public int UserCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ProjectPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IEnumerable<ProjectVM> Projects { get; set; }

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

    public class ProjectIM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int CustomerId { get; set; }
        public string Manager { get; set; }
   
        public DateTime StartDate { get; set; }
     
        public DateTime EndDate { get; set; }

    }
    public class ProjectUserIM
    {
        public int Id { get; set; }
        public string UserId { get; set; }

    }
    #endregion

    #region ProjectBusinesses
    public class ProjectBusinessVM
    {

        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
       
        public string Contract { get; set; }
        public string ProjectName { get; set; }
        public List<PaymentlogVM> Paymentlogs { get; set; }

        public decimal Paymented { get; set; }

    }

    public class ProjectBusinessPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IEnumerable<ProjectBusinessVM> ProjectBusinesses { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaymented { get; set; }

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

    public partial class ProjectBusinessIM
    {    
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public string Contract { get; set; }
      
    }


    #endregion
}
