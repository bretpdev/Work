using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTracking.Models
{
    public class ProjectData
    {
        public bool IsLoaded { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime From { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime To { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool ShowUnstopped { get; set; }
        public List<UserTime> Times { get; set; }
        public UserTime CurrentTime { get; set; }
        public List<SystemTypes> Systems { get; set; }
        public List<int> RequestNumbers { get; set; }
        public string GenericMeeting { get; set; }
        public List<CostCenters> CostCenter { get; set; }
        public bool BatchProcessing { get; set; }
        public List<User> Users { get; set; }
        public User SelectedUser { get; set; }
        public string SortOrder { get; set; }
        public Func<UserTime, object> Predicate { get; set; }
        public bool IsDescending { get; set; }

        public ProjectData()
        {
            Times = new List<UserTime>();
            Systems = new List<SystemTypes>();
            RequestNumbers = new List<int>();
            CostCenter = new List<CostCenters>();
            Users = new List<User>();
        }
    }
}