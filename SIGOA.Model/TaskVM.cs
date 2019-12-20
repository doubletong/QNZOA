using SIGOA.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGOA.Model
{
    #region Tasks
    public class TaskVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<int> BadgeIds { get; set; }
        public List<BadgeVM> Badges { get; set; }
        public Guid? Performer { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTme { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal WorkHours { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ActualWorkingHours {
            get {
                if (StartTime != null && FinishTme != null)
                {
                    TimeSpan interval = FinishTme.Value - StartTime.Value;
                    return  string.Format("{0:N2}小时", interval.TotalHours);
                }
                return "无";
            }
        }
        public string ExpectedTime
        {
            get
            {
                if (WorkHours>0)
                {                    
                    return $"{WorkHours}小时";
                }
                return "无";
            }
        }
        

    }

    public class TaskDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<int> BadgeIds { get; set; }
        public List<BadgeVM> Badges { get; set; }
        public Guid? Performer { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

    }
 
    public class TaskPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public int ProjectId { get; set; }
        public TaskStatus Status { get; set; }
        public string Performer { get; set; }
        public IList<TaskVM> ItemList { get; set; }

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

    public class TaskIM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Performer { get; set; }
        public TaskStatus Status { get; set; }
        public decimal WorkHours { get; set; }
        public int ProjectId { get; set; }
        public List<SelectVM> Badges { get; set; }

    }

    public class SetTaskStatusIM
    {
        public int Id { get; set; }
     
        public TaskStatus Status { get; set; }    

    }

    #endregion

    #region Badges
    public class BadgeVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Importance { get; set; }
    }

    public class BadgePagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IList<BadgeVM> ItemList { get; set; }

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
    public class BadgeIM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Importance { get; set; }
    }
    #endregion
}
