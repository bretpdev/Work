using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectRequest.Models
{
    public class ProjectRequest
    {
        public enum ProjectStatus
        {
            PendingReview,
            PendingPrioritization,
            PendingResources,
            InProgress,
            Complete,
            Other
        }

        public long? ProjectRequestId { get; set; } = null;

        [DisplayName("Project Name")]
        [Required]
        public string ProjectName { get; set; }
        [DisplayName("Submitted By")]
        [Required]
        public string SubmittedBy { get; set; }
        [Required]
        public string Department { get; set; } = "Applciation Development";
        [Required]
        public string Status { get; set; } = "PendingReview";
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [DisplayName("Project Summary")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string ProjectSummary { get; set; }
        [DisplayName("Business Need")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string BusinessNeed { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        public string Benefits { get; set; }
        [DisplayName("Implementation Approach")]
        [DataType(DataType.MultilineText)]
        public string ImplementationApproach { get; set; }
        [DisplayName("Requestor Score")]
        [Range(0, 5)]
        public int? RequestorScore { get; set; } = null;
        [DisplayName("Urgency Score")]
        [Range(0, 5)]
        public int? UrgencyScore { get; set; } = null;
        [DisplayName("Risk Score")]
        [Range(0, 5)]
        public int? RiskScore { get; set; } = null;

        public static List<string> GetStates()
        {
            return new List<string>
            {
                "Pending Review",
                "Pending Prioritization",
                "Pending Resources",
                "Complete",
                "In Progress"
            };
        }

        public static List<string> GetDepartments()
        {
            return DataAccess.GetDepartments();
        }

        public static string GetStateWithSpace(string state)
        {
            if(state == null)
            {
                return null;
            }
            if(state == "PendingReview")
            {
                return "Pending Review";
            }
            if(state == "PendingPrioritization")
            {
                return "Pending Prioritization";
            }
            if(state == "Pending Resources")
            {
                return "Pending Resources";
            }
            if(state == "InProgress")
            {
                return "In Progress";
            }
            else
            {
                return state;
            }
        }
    }
}