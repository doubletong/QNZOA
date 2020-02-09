using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QNZOA.Model
{

    public class CustomerForSelectVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte CustomerType { get; set; }
        public string DisplayName
        {
            get
            {
                if (CustomerType == 1)
                    return $"个人-{Name}";
                return $"公司-{Name}";
            }
        }

    }
    public class CustomerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte CustomerType { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Homepage { get; set; }
        public string Logo { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProjectCount { get; set; }
    }
    public class CustomerPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IList<CustomerVM> Customers { get; set; }

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

    public class CustomerIM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte CustomerType { get; set; }
        public string Description { get; set; }

        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Url]
        public string Homepage { get; set; }
        public string Logo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
