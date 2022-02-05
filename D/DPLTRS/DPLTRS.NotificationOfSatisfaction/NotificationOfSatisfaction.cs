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

namespace DPLTRS.NotificationOfSatisfaction
{
    public class LoanManagementLetters_NotificationOfSatisfaction : ScriptBase
    {
        LetterHelper lh;
        public ProcessLogRun logRun { get; set; }
        public DPLTRS.DataAccess DA { get; set; }

        public LoanManagementLetters_NotificationOfSatisfaction(ReflectionInterface ri)
            : base(ri, "DPLTRS")
        {
            lh = new LetterHelper(ri, ScriptId);
            logRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false);
            DA = new DataAccess(logRun);
        }

        public override void Main()
        {
            string comment = "";
            LC10Data data = new LC10Data();
            SystemBorrowerDemographics demos = null;
            NOTSATInfo info = null;
            //Prompt the user for an Ssn and a letter reason
            using (var form = new NOTSAT())
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
                        catch(Exception e)
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

            //Validate that the reason selected makes sense
            if (!ValidateReason(info, data))
            {
                return;
            }

            //Select LC67 AWG Record
            if (!CheckLC67(info.Ssn))
            {
                return;
            }

            //Update LC67 AWG Record
            LC67UpdateResults lc67Results = UpdateLC67(info, demos);
            if (lc67Results == null)
            {
                return;
            }
            else
            {
                comment += lc67Results.Comment;
            }

            //Get employer information
            EmployerData employerData = GetEmployerData(lc67Results.EmployerId);
            if (employerData == null)
            {
                return;
            }

            //set variable text
            string varText = "";
            if (info.Reason == NOTSAT.NOTSATReason.Bankruptcy)
            {
                varText = "The Order of Withholding from Earnings is being released as the borrower has filed for bankruptcy and UHEAA is required to comply with the bankruptcy automatic stay.  You may be contacted to resume the wage garnishment when the bankruptcy has ended.";
            }
            else if (info.Reason == NOTSAT.NOTSATReason.Management)
            {
                varText = "After the date of this release, the employer is no longer required to withhold income from the debtor's pay.  No further deductions should be made against the wages of the debtor.";
            }
            else
            {
                varText = "After the date of this release, the employer is no longer required to withhold income from the debtor's pay.  The amount of the order has been paid.  No further deductions should be made against the wages of the debtor.";
            }

            //Add LC20 EI Override Indicator
            if (!AddLC20EIOverrideIndicator(demos.Ssn))
            {
                return;
            }

            //LP50
            comment += $"mailed NOTSAT to {employerData.Employer} ({lc67Results.EmployerId})";
            AddCommentInLP50(demos.Ssn, "LT", "81", "LPAWG", comment, ScriptId);

            //Create Letter
            bool result = CreateLetter("NOTSAT", demos, employerData, data, varText);
            if (result)
            {
                Dialog.Info.Ok("The letter has been generated.");
            }
            else
            {
                Dialog.Warning.Ok("The letter has already been generated today, No new letter created. If you believe the letter has not been generated today, please contact Systems Support.");
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

        public bool ValidateReason(NOTSATInfo info, LC10Data data)
        {
            //Begin Processing Borrower
            if (info.Reason == NOTSAT.NOTSATReason.SmallBalance)
            {
                data = GetLC10Balances(info.Ssn, true);
                if (data.LC10Sta == "X")
                {
                    Dialog.Error.Ok("The current letter cannot be printed as the borrower has no information displayed on LC10.");
                    return false;
                }
                if (data.PrincCur + data.IntCur > 25 || data.LegalCur + data.OtherCur + data.CCCur > 100)
                {
                    Dialog.Error.Ok("A small balance/closed account Notice of Satisfaction cannot be generated as principal and interest is greater than $25 or other costs are greater than $100.");
                    return false;
                }
            }

            if (info.Reason == NOTSAT.NOTSATReason.Bankruptcy)
            {
                if (!CheckLC05LoansInBankruptcy(info.Ssn))
                {
                    return false;
                }
            }

            if (info.Reason == NOTSAT.NOTSATReason.Management)
            {
                string user = lh.GetUserFromLP40();
                bool? hasAccess = DA.CheckUserHasAccess(user, "LMCollectorLetters");
                if (!hasAccess.HasValue || hasAccess.Value == false)
                {
                    Dialog.Error.Ok("You are not authorized to generate a management/other Notice of Satisfaction.");
                    return false;
                }
            }
            return true;
        }

        public bool CreateLetter(string letterId, SystemBorrowerDemographics demos, EmployerData employerData, LC10Data lc10Data, string varText)
        {
            string letterData = CreateLetterData(demos, employerData, lc10Data, varText);
            int? id = EcorrProcessing.AddRecordToPrintProcessing(ScriptId, letterId, letterData, demos.AccountNumber.Replace(" ", ""), "MA2329");
            return id.HasValue;
        }

        public string CreateLetterData(SystemBorrowerDemographics demos, EmployerData employerData, LC10Data lc10Data, string varText)
        {
            //File Header: "AN,SSN,KeyLine,Name,EmployerName,EmpAddress1,EmpAddress2,EmpCity,EmpState,EmpZIP,Balance,VarText"
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
            string balance = string.Format("{0:n}", lc10Data.Balance);
            return $"{accountNumber},{maskedSsn},{keyLine},{name},\"{employerData.Employer}\",\"{employerData.EmployerAddress1}\",\"{employerData.EmployerAddress2}\",\"{employerData.EmployerCity}\",\"{employerState}\",\"{employerZip}\",\"{balance}\",\"{varText}\"";
        }

        public LC67UpdateResults UpdateLC67(NOTSATInfo info, SystemBorrowerDemographics demos)
        {
            LC67UpdateResults results = new LC67UpdateResults();

            //update inactive date to today and reason to 01 if pif option is selected
            if (info.Reason == NOTSAT.NOTSATReason.SmallBalance && RI.CheckForText(15, 71, "MMDDCCYY") && RI.CheckForText(7, 35, "MMDDCCYY"))
            {
                string dateString = DateTime.Now.ToString("MMddyyyy");
                RI.PutText(7, 35, dateString);
                RI.PutText(8, 19, "26");
                results.Comment = "updated AWG withdrawn date to " + dateString + " and withdrawn reason to 26 for small balance, ";
            }
            //verify the record is closed if resend option is selected, warn the user and exit if not
            else if (info.Reason == NOTSAT.NOTSATReason.EmployerRequest)
            {
                if (RI.CheckForText(8, 19, "__") && RI.CheckForText(16, 63, "__"))
                {
                    Dialog.Error.Ok("A Notice of Satisfaction cannot be resent at the employer's request as the AWG record is still open indicating a notice has not been sent to the employer.");
                    return null;
                }
            }
            //update the withdrawn reason to 06 if bankruptcy is selected
            else if (RI.CheckForText(15, 71, "MMDDCCYY") && RI.CheckForText(7, 35, "MMDDCCYY"))
            {
                string dateString = DateTime.Now.ToString("MMddyyyy");
                RI.PutText(7, 35, dateString);
                //update the withdrawn reason to 06 if bankruptcy is selected
                if (info.Reason == NOTSAT.NOTSATReason.Bankruptcy)
                {
                    RI.PutText(8, 19, "06");
                    results.Comment = "updated AWG withdrawn date to " + dateString + " and withdrawn reason to 06 for bankruptcy, ";
                }
                //update the withdrawn reason to 99 if management is selected
                else if (info.Reason == NOTSAT.NOTSATReason.Management)
                {
                    RI.PutText(8, 19, "99");
                    results.Comment = "updated AWG withdrawn date to " + dateString + " and withdrawn reason to 99, ";
                }
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
            if (RI.CheckForText(22, 3, "47004") || RI.CheckForText(22, 3, "00008"))
            {
                Dialog.Error.Ok("No LC67 Record Found or Incorrect SSN");
                return false;
            }
            //prompt the user to select the record
            if (RI.CheckForText(21, 3, "SEL"))
            {
                while (!RI.CheckForText(1, 70, "AWG"))
                {
                    Dialog.Info.Ok("Select the correct record and hit Insert to continue.");
                    RI.PauseForInsert();
                }
            }

            return true;
        }

        public bool CheckLC05LoansInBankruptcy(string ssn)
        {
            RI.FastPath($"LC05I{ssn}");

            if (RI.GetText(22, 3, 5) == "47004")
            {
                Dialog.Error.Ok("The current letter cannot be printed as the borrower has no loans on LC05.");
                return false;
            }

            RI.PutText(21, 13, "01", ReflectionInterface.Key.Enter);
            do
            {
                if (RI.CheckForText(4, 26, "07"))
                {
                    return true;
                }

                RI.Hit(ReflectionInterface.Key.F8);
            } while (!RI.CheckForText(22, 3, "46004"));

            Dialog.Error.Ok("A bankruptcy Notice of Satisfaction cannot be generated as the borrower does not have any loans in bankruptcy.");
            return false;
        }

        public LC10Data GetLC10Balances(string ssn, bool active)
        {
            LC10Data data = new LC10Data();
            if (active)
            {
                RI.FastPath($"LC10I{ssn}");
            }
            else
            {
                RI.FastPath($"LC10I{ssn};Y");
            }
            //RI.Hit(ReflectionInterface.Key.Enter);

            if (RI.GetText(22, 3, 5) == "48012" || RI.GetText(22, 3, 5) == "45003")
            {
                data.LC10Sta = "X";
                return data;
            }
            else
            {
                data.TotalDef = RI.GetDisplayText(7, 36, 10).ToDecimal();
                data.PrincCol = RI.GetDisplayText(8, 36, 10).ToDecimal();
                data.PrincCur = data.TotalDef - data.PrincCol;
                data.IntAcc = RI.GetDisplayText(9, 36, 10).ToDecimal();
                data.IntCol = RI.GetDisplayText(10, 36, 10).ToDecimal();
                data.IntCur = data.IntAcc - data.IntCol;
                data.LegalAcc = RI.GetDisplayText(11, 36, 10).ToDecimal();
                data.LegalCol = RI.GetDisplayText(12, 36, 10).ToDecimal();
                data.LegalCur = data.LegalAcc - data.LegalCol;
                data.OtherAcc = RI.GetDisplayText(13, 36, 10).ToDecimal();
                data.OtherCol = RI.GetDisplayText(14, 36, 10).ToDecimal();
                data.OtherCur = data.OtherAcc - data.OtherCol;
                data.CCAcc = RI.GetDisplayText(15, 36, 10).ToDecimal();
                data.CCCol = RI.GetDisplayText(16, 36, 10).ToDecimal();
                data.CCCur = data.CCAcc - data.CCCol;
                data.CCProj = RI.GetDisplayText(17, 36, 10).ToDecimal();
                data.TotalAcc = data.TotalDef + data.IntAcc + data.LegalAcc + data.OtherAcc + data.CCAcc + data.CCProj;
                data.TotalCol = data.PrincCol + data.IntCol + data.LegalCol + data.OtherCol + data.CCCol;
                data.Balance = RI.GetDisplayText(18, 36, 10).ToDecimal();
                return data;
            }
        }

        public EmployerData GetEmployerData(string employerId)
        {
            bool foundEmployer = false;
            RI.FastPath($"LPEMI{employerId}");

            //find dept 128 or GEN if no dept 128
            if (RI.CheckForText(1, 49, "INSTITUTION DEPARTMENT SELECTION"))
            {
                int row = 7;
                string dpt = "128";
                //Exit when we've finished looking for the GEN department if no department is found
                while (!RI.CheckForText(22, 3, "46004") || dpt != "GEN")
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
                    while (RI.CheckForText(row, 3, " ") && row < 21)
                    {
                        row++;
                    }

                    //go to next page
                    if (row == 21)
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

            //Sometimes the fastpath automatically selects a record if there is only one
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
