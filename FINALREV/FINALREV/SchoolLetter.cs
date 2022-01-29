using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace FINALREV
{
    public class SchoolLetter
    {
        private ReflectionInterface RI { get; set; }
        private DataAccess DA { get; set; }
        private BorrowerRecord Borrower { get; set; }

        public SchoolLetter(ReflectionInterface ri, DataAccess da, BorrowerRecord bor)
        {
            RI = ri;
            DA = da;
            Borrower = bor;
        }

        public bool SchoolLetterCreated(List<string> schools)
        {
            schools.AddRange(DA.GetSchools(Borrower.BorrowerRecordId));

            bool schoolLetterCreated = false;
            RI.FastPath($"LG29I{Borrower.Demos.Ssn}");
            if (RI.CheckForText(1, 49, "STUDENT ENROLLMENT STATUS SELECT")) //review school on selection screen
            {
                int row = 9;
                while (RI.AltMessageCode != "46004")
                {
                    InsertSchoolRecord(schools, RI.GetText(row, 14, 8));
                    row++;
                    if (RI.CheckForText(row, 6, " "))
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 9;
                    }
                }
            }
            else if (RI.CheckForText(1, 48, "STUDENT ENROLLMENT STATUS DISPLAY")) //review school on detail screen
                InsertSchoolRecord(schools, RI.GetText(10, 71, 8));
            else
            {
                schoolLetterCreated = true;
                ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    AccountNumber = Borrower.Demos.AccountNumber,
                    Arc = "S4SCL",
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    Comment = "",
                    ScriptId = FinalReview.ScriptId
                };
                arc.AddArc();
            }

            return schoolLetterCreated;
        }

        /// <summary>
        /// Insert a school code for borrower if one doesn't already exist
        /// </summary>
        private void InsertSchoolRecord(List<string> schools, string school)
        {
            int schoolId = 0;
            if (!schools.Any(s => s == school))
            {
                try
                {
                    schoolId = DA.InsertSchool(school);
                    DA.InsertBorrowerSchool(Borrower.BorrowerRecordId, schoolId);
                    schools.Add(school);
                }
                catch (Exception ex)
                {
                    string message = $"There was an error insert the School ID {schoolId} for Borrower: {Borrower.Demos.AccountNumber}, Borrower Record ID: {Borrower.BorrowerRecordId}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                }
            }
        }
    }
}