using QNZOA.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QNZOA.Model
{
    public class ProjectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
       
        public string CreatedBy { get; set; }
        public string CustomerName { get; set; }
        public string ManagerName { get; set; }
        public int UserCount { get; set; }
        public int TaskCount { get; set; } 
        public DateTime? StartDate { get; set; }    
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ProjectDetailVM: ProjectVM
    {
        public string Description { get; set; }
    }
    public class ProjectPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public int CustomerId { get; set; }
        public string OrderBy { get; set; }
        public string OrderMode { get; set; }
        public IList<ProjectVM> Projects { get; set; }

        public int LastPageIndex
        {
            get
            {
                if (PageSize > 0)
                {
                    var isInt = RowCount % PageSize;
                    var d = RowCount / PageSize;
                    if (isInt == 0)
                    {
                        return d - 1;
                    }
                    else
                    {
                        return (int)Math.Floor((double)d);
                    }

                }
                return 0;
            }
        }
    }


    public class ProjectIM
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Name { get; set; }
        public int CustomerId { get; set; }      
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid Manager { get; set; }




    }
}
