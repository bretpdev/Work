using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ProjectRequest.Models
{
    public class ProductPrioritization
    {
        public int ProjectId { get; set; }
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        public string BusinessUnit { get; set; }
        public string Details { get; set; }
        public int FinanceScore { get; set; }
        public int RequestorScore { get; set; }
        public int UrgencyScore { get; set; }
        public int ResourcesScore { get; set; }
        public int RiskScore { get; set; }
        public int TotalScore { get; set; }
        public string Status { get; set; }
        public static IEnumerable<ProductPrioritization> ApplySort(IEnumerable<ProductPrioritization> records, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name":
                    records = records.OrderBy(r => r.ProjectName);
                    break;
                case "name_desc":
                    records = records.OrderByDescending(r => r.ProjectName);
                    break;
                case "business_unit":
                    records = records.OrderBy(r => r.BusinessUnit);
                    break;
                case "business_unit_desc":
                    records = records.OrderByDescending(r => r.BusinessUnit);
                    break;
                case "details":
                    records = records.OrderBy(r => r.Details);
                    break;
                case "details_desc":
                    records = records.OrderByDescending(r => r.Details);
                    break;
                case "finance_score":
                    records = records.OrderBy(r => r.FinanceScore);
                    break;
                case "finance_score_desc":
                    records = records.OrderByDescending(r => r.FinanceScore);
                    break;
                case "requestor_score":
                    records = records.OrderBy(r => r.RequestorScore);
                    break;
                case "requestor_score_desc":
                    records = records.OrderByDescending(r => r.RequestorScore);
                    break;
                case "urgency_score":
                    records = records.OrderBy(r => r.UrgencyScore);
                    break;
                case "urgency_score_desc":
                    records = records.OrderByDescending(r => r.UrgencyScore);
                    break;
                case "resource_score":
                    records = records.OrderBy(r => r.ResourcesScore);
                    break;
                case "resource_score_desc":
                    records = records.OrderByDescending(r => r.ResourcesScore);
                    break;
                case "risk_score":
                    records = records.OrderBy(r => r.RiskScore);
                    break;
                case "risk_score_desc":
                    records = records.OrderByDescending(r => r.RiskScore);
                    break;
                case "total_score":
                    records = records.OrderBy(r => r.TotalScore);
                    break;
                case "status":
                    records = records.OrderBy(r => r.Status);
                    break;
                case "status_desc":
                    records = records.OrderByDescending(r => r.Status);
                    break;
                default:
                    records = records.OrderByDescending(r => r.TotalScore);
                    break;
            }
            return records;
        }
    }
}