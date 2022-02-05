using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DPLTRS.NotificationOfGarnishmentSuspension
{
    public class LoanManagementLetters_NotificationOfGarnishmentSuspension : ScriptBase
    {
        LetterHelper lh;
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }

        public LoanManagementLetters_NotificationOfGarnishmentSuspension(ReflectionInterface ri)
            : base(ri, "DPLTRS")
        {
            lh = new LetterHelper(ri, ScriptId);
            logRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false);
            DA = new DataAccess(logRun);
        }

        public override void Main()
        {
            SystemBorrowerDemographics demos = null;
            NOTSUSPInfo info = null;

            //Validate that the user is a valid manager
            if (!ValidateUserHasManagementAccess())
            {
                return;
            }

            //Prompt the user for an Ssn and a letter reason
            using (var form = new NOTSUSP())
            {
                bool prompt = true;
                while (prompt)
                {
                    prompt = false;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        info = form.GetInfo();
                        try
                        {
                            demos = lh.ResolveBorrower(info.Ssn);
                        }
                        catch (Exception e)
                        {
                            Dialog.Error.Ok("Unable to find borrower from provided Ssn in LP22");
                        }
                        if (demos == null)
                            prompt = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            //Select LC67 AWG Record
            if(!CheckLC67(info.Ssn))
            {
                return;
            }

            //Update LC67 AWG Record
            LC67UpdateResults lc67Results = UpdateLC67();
            if(lc67Results == null)
            {
                return;
            }

            //Get employer information
            EmployerData employerData = GetEmployerData(lc67Results.EmployerId);
            if(employerData == null)
            {
                return;
            }

            //Add LC20 EI Override Indicator
            if(!AddLC20EIOverrideIndicator(demos.Ssn))
            {
                return;
            }

            //LP50
            string comment = $"MAILED NOTSUSP TO {employerData.Employer} ({lc67Results.EmployerId})";
            AddCommentInLP50(demos.Ssn, "LT", "81", "LPAWG", comment, ScriptId);

            //Create Letter
            bool result = CreateLetter("NOTSUSP", demos, employerData);
            if(result)
            {
                Dialog.Info.Ok("The letter has been generated.");
            }
            else
            {
                Dialog.Warning.Ok("The letter has already been generated today, No new letter created.");
            }
        }

        public bool AddLC20EIOverrideIndicator(string ssn)
        {
            RI.FastPath($"LC20C{ssn}");
            if (RI.CheckForText(22, 3, "48012"))
            {
                Dialog.Error.Ok("No LC20 Record Found");
                return false;
            }
            RI.PutText(10, 78, "Y", ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(22, 3, "49000"))
            {
                Dialog.Error.Ok("Unable to update EI OVRRIDE on LC20");
                return false;
            }
            return true;
        }

        public bool ValidateUserHasManagementAccess()
        {
            //Begin Processing Borrower            
            string user = lh.GetUserFromLP40();
            bool? hasAccess = DA.CheckUserHasAccess(user, "LMCollectorLetters");
            if (!hasAccess.HasValue || hasAccess.Value == false)
            {
                Dialog.Error.Ok("You are not authorized to generate a management/other Notice of Satisfaction.");
                return false;
            }
            return true;
        }

        public bool CreateLetter(string letterId, SystemBorrowerDemographics demos, EmployerData employerData)
        {
            string letterData = CreateLetterData(demos, employerData);
            int? id = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber.Replace(" ", ""), "MA2329");
            return id.HasValue;
        }

        public string CreateLetterData(SystemBorrowerDemographics demos, EmployerData employerData)
        {
            //File Header: "AN,SSN,KeyLine,Name,EmployerName,EmpAddress1,EmpAddress2,EmpCity,EmpState,EmpZIP"
            string employerZip = employerData.EmployerZip;
            if (employerZip.Length > 5)
            {
                employerZip = employerZip.SafeSubString(0, 5) + "-" + employerZip.SafeSubString(5, 4);
            }

            string accountNumber = demos.AccountNumber.Replace(" ", "");
            string maskedSsn = "XXX-XX-" + demos.Ssn.SafeSubString(5, 4);
            string keyLine = DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string name = demos.FirstName + " " + demos.LastName;
            string employerState = employerData.EmployerState.Replace("FC", "");
            //string balance = string.Format("{0:n}", lc10Data.Balance);
            return $"{accountNumber},{maskedSsn},{keyLine},{name},\"{employerData.Employer}\",\"{employerData.EmployerAddress1}\",\"{employerData.EmployerAddress2}\",\"{employerData.EmployerCity}\",\"{employerState}\",\"{employerZip}\"";
        }

        public LC67UpdateResults UpdateLC67()
        {
            LC67UpdateResults results = new LC67UpdateResults();

            if (RI.CheckForText(15, 71, "MMDDCCYY") && RI.CheckForText(7, 35, "MMDDCCYY"))
            {
                string dateString = DateTime.Now.ToString("MMddyyyy");
                RI.PutText(7, 35, dateString);

                //update the withdrawn reason to 99 if management is selected
                RI.PutText(8, 19, "99");

                //Withdrawn LTR field
                RI.PutText(6, 19, "N");
                //blank the hold and inactive dates
                if (!RI.CheckForText(15, 71, "MM"))
                {
                    RI.PutText(15, 71, "**");
                }
                if (!RI.CheckForText(17, 71, "MM"))
                {
                    RI.PutText(17, 71, "**");
                }
            }
            else
            {
                Dialog.Error.Ok("Unable to update LC67. Screen positions 15,71 or 7,35 do not show MMDDCCYY.");
                return null;
            }

            //get employer id
            results.EmployerId = RI.GetText(9, 45, 8).Trim();
            //enter changes
            RI.Hit(ReflectionInterface.Key.Enter);

            return results;
        }

        public bool CheckLC67(string ssn)
        {
            RI.FastPath($"LC67C{ssn}GG");
            //warn the user and exit if no AWG record exists
            if(RI.CheckForText(22,3,"47004") || RI.CheckForText(22,3, "00008"))
            {
                Dialog.Error.Ok("No LC67 Record Found or Incorrect SSN");
                return false;
            }
            //prompt the user to select the record
            if(RI.CheckForText(21,3,"SEL"))
            {
                while(!RI.CheckForText(1, 70, "AWG"))
                {
                    Dialog.Info.Ok("Select the correct record and hit Insert to continue.");
                    RI.PauseForInsert();
                }
            }

            return true;
        }

        public EmployerData GetEmployerData(string employerId)
        {
            bool foundEmployer = false;
            RI.FastPath($"LPEMI{employerId}");

            //find dept 128 or GEN if no dept 128
            if(RI.CheckForText(1,49, "INSTITUTION DEPARTMENT SELECTION"))
            {
                int row = 7;
                string dpt = "128";
                //Exit when we've finished looking for the GEN department if no department is found
                while(!RI.CheckForText(22,3,"46004") || dpt != "GEN")
                {
                    //select the row if it is the right dept
                    if (RI.CheckForText(row, 7, dpt))
                    {
                        RI.PutText(21, 13, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter);
                        foundEmployer = true;
                        break;
                    }
                    row++;

                    //skip blank rows
                    while (RI.CheckForText(row,3," ") && row < 21)
                    {
                        row++;
                    }

                    //go to next page
                    if(row == 21)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        //if no more pages, go back to first page and start looking for GEN dept  
                        if (RI.CheckForText(22, 3, "46004") && dpt == "128")
                        {
                            RI.PutText(2, 73, "01", ReflectionInterface.Key.Enter);
                            dpt = "GEN";
                        }
                        row = 7;                        
                    }
                }
            }

            //sometimes the fastpath automatically selects a record when there is only one
            if (!foundEmployer && !RI.CheckForText(7, 15, "GEN") && !RI.CheckForText(7, 15, "128"))
            {
                Dialog.Error.Ok($"Unable to find employer department(128 or GEN) on LPEM, employer id: {employerId}");
                return null;
            }

            EmployerData data = new EmployerData();
            data.Employer = RI.GetText(5, 21, 40).Trim();
            data.EmployerAddress1 = RI.GetText(8, 21, 40).Trim();
            data.EmployerAddress2 = RI.GetText(9, 21, 40).Trim();
            data.EmployerCity = RI.GetText(11, 21, 30).Trim();
            data.EmployerState = RI.GetText(11, 59, 2);
            data.EmployerZip = RI.GetText(11, 66, 9);
            return data;

            //Dialog.Error.Ok($"Unable to navigate to LPEM for employer id: {employerId}");
            //return null;
        }

    }
}
