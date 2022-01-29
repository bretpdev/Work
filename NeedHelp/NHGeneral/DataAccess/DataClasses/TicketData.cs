using System;
using System.Collections.Generic;
using SubSystemShared;
using Uheaa.Common.ProcessLogger;

namespace NHGeneral
{
    public class TicketData
    {
        public string Area { get; set; }
        public bool AssignProgrammer { get; set; }
        public SqlUser AssignedTo { get; set; }
        public List<string> AttachedFiles { get; set; }
        public string CatOption { get; set; }
        public string CCCIssue { get; set; }
        public string Comments { get; set; }
        public SqlUser Court { get; set; }
        public DateTime CourtDate { get; set; }
        public string History { get; set; }
        public string NewHistoryComment { get; set; }
        public string Issue { get; set; }
        public string IssueUpdate { get; set; }
        public DateTime LastUpdated { get; set; }
		public SqlUser PreviousCourt { get; set; }
        public string PreviousStatus { get; set; }
        public short Priority { get; set; }
        public DateTime Requested { get; set; }
        public SqlUser Requester { get; set; }
        public string RequestProjectNum { get; set; }
        public DateTime Required { get; set; }
        public string RelatedProcedures { get; set; }
        public string RelatedQCIssues { get; set; }
        public string ResolutionCause { get; set; }
        public string ResolutionFix { get; set; }
        public string ResolutionPrevention { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string Subject { get; set; }
        public List<string> Systems { get; set; }
        public string TicketCode { get; set; }
        public long TicketID { get; set; }
        public BusinessUnit Unit { get; set; }
        public string UrgencyOption { get; set; }
        public List<SqlUser> UserSelectedEmailRecipients { get; set; }
		public List<SqlUser> NotifyEmailList { get; set; }

        public TicketData()
        {
            UserSelectedEmailRecipients = new List<SqlUser>();
			NotifyEmailList = new List<SqlUser>();
            Systems = new List<string>();
            AttachedFiles = new List<string>();
            Court = new SqlUser();
            AssignedTo = new SqlUser();
            Requester = new SqlUser();
        }
    }
}