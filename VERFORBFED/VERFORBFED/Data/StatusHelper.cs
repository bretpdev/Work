using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERFORBFED
{
    class StatusHelper
    {
        public List<LoanStatusInfo> CollectionSuspensionDeniedStatuses { get; private set; }
        public StatusHelper()
        {
            CollectionSuspensionDeniedStatuses = new List<LoanStatusInfo>()
            {
                new LoanStatusInfo("21", null, "Bankruptcy Verified"),
                new LoanStatusInfo(null, "LITIGATION", "Litigation"),
                new LoanStatusInfo("17", null, "Verified Death"),
                new LoanStatusInfo(null, "DECONVERTED", "Deconverted"),
                new LoanStatusInfo("16", null, "Alleged Death"),
                new LoanStatusInfo("02", null, "In School"),
                new LoanStatusInfo("22", null, "Paid In Full"),
                new LoanStatusInfo(null, "IDENTITY THEFT", "Identity Theft"),
                new LoanStatusInfo(null, "CNSLD-STOP PURSUIT", "CNSLD - Stop Pursuit"),
                new LoanStatusInfo(null, "DISABILITY TRANSFER", "Disability Transfer"),
                new LoanStatusInfo("18", "ALLEGED INDEF SUSP", "Alleged Indef Susp"),
                new LoanStatusInfo("18", "ALLEGED 120 TPD SUSP", "Alleged 120 TPD Susp"),
                new LoanStatusInfo("19", null, "Verified Disability"),
                new LoanStatusInfo(null, "DELINQUENCY TRANSFER", "Delinquency Transfer"),
                new LoanStatusInfo("01", null, "In Grace"),
                new LoanStatusInfo(null, "PENDING DELINQUENCY TRANSFER", "Pending Delinquency Transfer"),
                new LoanStatusInfo(null, "PEND DELQ TRANSFER", "Pending Delinquency Transfer"),
                new LoanStatusInfo(null, "SUBMIT DELINQUENCY TRANSFER", "Submit Delinquency Transfer"),
                new LoanStatusInfo(null, "SUBMIT DELQ TRANSFER", "Submit Delinquency Transfer"),
                new LoanStatusInfo("20", null, "Alleged Bankruptcy"),
                new LoanStatusInfo("23", null, "Not Fully Originated"),
                new LoanStatusInfo(null, "PENDING ID THEFT", "Pending ID Theft")
            };
        }

        public bool AllStatusesMatchCollectionSuspensionDenied(List<LoanStatusInfo> existingStatuses)
        {
            if (!existingStatuses.Any())
                return false;
            foreach (var status in existingStatuses)
            {
                bool match = false;
                foreach (var denial in CollectionSuspensionDeniedStatuses)
                {
                    if (denial.Matches(status))
                    {
                        match = true;
                        if (string.IsNullOrEmpty(status.WX_OVR_DW_LON_STA))
                            status.WX_OVR_DW_LON_STA = denial.SimpleName;
                        break;
                    }
                }
                if (!match)
                    return false;
            }
            return true;
        }
    }
}
