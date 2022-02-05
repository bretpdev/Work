using System.Text;

namespace ESPQUEUES
{
    static class ScriptHelper
    {
        public static string GenerateTs01Comment(Loan firstUpdateLoan, string separationReason)
        {
            //Add an activity record.
            StringBuilder commentBuilder = new StringBuilder("changed ");
            if (firstUpdateLoan.NewSeparationDate != Loan.NoDate && firstUpdateLoan.NewSeparationDate != firstUpdateLoan.SeparationDate)
                commentBuilder.AppendFormat("sep dt from {0:MM/dd/yyyy} to {1:MM/dd/yyyy}", firstUpdateLoan.SeparationDate, firstUpdateLoan.NewSeparationDate);

            string abbreviatedSeparationReason = firstUpdateLoan.GetAbbreviatedSeparationReason(separationReason);
            if (abbreviatedSeparationReason != firstUpdateLoan.EnrollmentStatus) { commentBuilder.AppendFormat("sep rea from {0} to {1};", abbreviatedSeparationReason, firstUpdateLoan.NewSeparationReason); }
            if (firstUpdateLoan.LenderNotifiedDate != firstUpdateLoan.CompassNotifiedDate) { commentBuilder.AppendFormat("not dt from {0:MM/dd/yy} to {1:MM/dd/yy};", firstUpdateLoan.CompassNotifiedDate, firstUpdateLoan.LenderNotifiedDate); }
            if (firstUpdateLoan.OneLinkSchoolCode != firstUpdateLoan.CompassSchoolCode) { commentBuilder.AppendFormat("sch cd from {0} to {1};", firstUpdateLoan.CompassSchoolCode, firstUpdateLoan.OneLinkSchoolCode); }
            if (firstUpdateLoan.CertificationDate != firstUpdateLoan.CompassCertifiedDate) { commentBuilder.AppendFormat("cert dt from {0:MM/dd/yy} to {1:MM/dd/yy};", firstUpdateLoan.CompassCertifiedDate, firstUpdateLoan.CertificationDate); }
            commentBuilder.AppendFormat("enr sta dt to {0:MM/dd/yyyy}", firstUpdateLoan.EnrollmentStatusEffectiveDate);

            return commentBuilder.ToString();
        }
    }
}
