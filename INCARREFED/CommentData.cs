using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INCARREFED
{
    public class CommentData
    {
        public bool IsBorrower { get; set; }
        public string BorrowerAccountNumber { get; set; }
        public string StudentAccountNumber { get; set; }
        public string BorrowerName { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityZip { get; set; }
        public string FacilityState { get; set; }
        public string FacilityPhone { get; set; }
        public string InmateNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime FollowUpDate { get; set; }
        public string Source { get; set; }
        public string OtherInfo { get; set; }
    }
}
