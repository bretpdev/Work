using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CFSARPTFED
{
    class CornerStoneQuarterlyReviewRandomSamples : ReportsBase
    {
        public CornerStoneQuarterlyReviewRandomSamples(ReportData reports, ErrorReport err)
            : base(err)
        {
            Report = reports;
        }

        public void CreateTheReport()
        {
            string fileToProcess = GetTheSasFile();
            if (fileToProcess.IsNullOrEmpty())
                return;

            List<Unwc14FileData> fileData = LoadTheFile(fileToProcess);

            using (ExcelGenerator excel = new ExcelGenerator(Report.ReportName + "xlsx"))
            {
                excel.SetActiveWorksheet(1, "Account Extracts");
                int row = 1;
                foreach (Unwc14FileData fileLine in fileData)
                {
                    excel.InsertData("A" + row, "A" + row, fileLine.DateExtracted);
                    excel.InsertData("B" + row, "B" + row, fileLine.Ssn);
                    excel.InsertData("C" + row, "C" + row, fileLine.FirstName);
                    excel.InsertData("D" + row, "D" + row, fileLine.LastName);
                    excel.InsertData("E" + row, "E" + row, fileLine.Street);
                    excel.InsertData("F" + row, "F" + row, fileLine.City);
                    excel.InsertData("G" + row, "G" + row, fileLine.State);
                    excel.InsertData("H" + row, "H" + row, fileLine.Zip);
                    excel.InsertData("I" + row, "I" + row, fileLine.Birthdate);
                    excel.InsertData("J" + row, "J" + row, fileLine.LoanType);
                    excel.InsertData("K" + row, "K" + row, fileLine.LoanProgram);
                    excel.InsertData("L" + row, "L" + row, fileLine.InterestRate);
                    excel.InsertData("M" + row, "M" + row, fileLine.LoanStatus);
                    excel.InsertData("N" + row, "N" + row, fileLine.PrincipalBalance);
                    excel.InsertData("O" + row, "O" + row, fileLine.Interest);
                    excel.InsertData("P" + row, "P" + row, fileLine.TotalBalance);
                    excel.InsertData("Q" + row, "Q" + row, fileLine.LastDisbursement);
                    excel.InsertData("R" + row, "R" + row, fileLine.LastDisbDate);
                    excel.InsertData("S" + row, "S" + row, fileLine.LastPaymentTotal);
                    excel.InsertData("T" + row, "T" + row, fileLine.LastPaymentAmountLoan);
                    excel.InsertData("U" + row, "U" + row, fileLine.DateAssignedToDmcs);
                    excel.InsertData("V" + row, "V" + row, fileLine.OriginationSource);
                    excel.InsertData("W" + row, "W" + row, fileLine.DateAdded);
                    excel.InsertData("X" + row, "X" + row, fileLine.CrbReportDate);
                    excel.InsertData("Y" + row, "Y" + row, fileLine.CrbReportStatus);
                    row++;
                }

                excel.SetActiveWorksheet(2, "Code Definitions");
                CreateCodeDefinitions(excel);

                File.Delete(fileToProcess);
            }
        }

        private void CreateCodeDefinitions(ExcelGenerator excel)
        {
            Font timesNewRoman = new Font("Times New Roman", 10);
            Font boldArial = new Font("Arial Black", 10, FontStyle.Bold);
            int row = 1;

            row = AddData(excel, boldArial, "CODE", "DEFERMENT TYPE DEFINITIONS", "CODE", "FORBEARANCE TYPE DEFINITIONS", "CODE", "LOAN TYPE DEFINITIONS", row);
            row = AddData(excel, timesNewRoman, "A", "In School Full Time", "3", "MILITARY", "CL", "CONSOL", row);
            row = AddData(excel, timesNewRoman, "L", "National Oceanic and Atmospheric", "Y", "LATE NOTIFICATION FORBEARANCE", "PL", "FED PLUS LOAN", row);
            row = AddData(excel, timesNewRoman, "D", "Temporary Total Disability", "B", "PDG ALIGNMENT FORBEARANCE", "GB", "GRAD PLUS", row);
            row = AddData(excel, timesNewRoman, "R", "Rehabilitation Training", "3", "ADMIN MILTRY MOBILIZATION FORBEARANCE", "CL", "SPOUSAL CONSOL", row);
            row = AddData(excel, timesNewRoman, "H", "Military/Public Health Service", "E", "TEMPORARY HARDSHIP", "SF", "FED STAFFORD", row);
            row = AddData(excel, timesNewRoman, "K", "Action Volunteer", "E", "GRACE", "CL", "SUB CONSOL", row);
            row = AddData(excel, timesNewRoman, "F", "Graduate Fellowship", "G", "INTERNSHIP/RESIDENCY", "CL", "SUB SPOUSAL CONSOL", row);
            row = AddData(excel, timesNewRoman, "K", "Peace Corps Volunteer", "?", "HHS APPROVED", "CL", "UNSUB CONSOL", row);
            row = AddData(excel, timesNewRoman, "I", "Internship/Residency no Cert Required", "B", "ADMINISTRATIVE FORBEARANCE", "CL", "UNSUB SPOUSAL CONSOL", row);
            row = AddData(excel, timesNewRoman, "I", "Intership/Residency Cert Required", "Y", "BANKRUPTCY", "SU", "FED UNSUB STAFFORD", row);
            row = AddData(excel, timesNewRoman, "X", "Volunteer in Tax Exempt Program", "R", "REHABILITATION", "D7", "DIRECT PLUS CONSOL", row);
            row = AddData(excel, timesNewRoman, "M", "Mothers Entering the Workforce", "3", "DESERT/TRANSITION", "D3", "DIRECT PLUS GRAD", row);
            row = AddData(excel, timesNewRoman, "M", "Pregnant of Caring for a Newborn", "Y", "DEATH", "D4", "DIRECT PLUS", row);
            row = AddData(excel, timesNewRoman, "U", "Unemployment", "D", "PERMANENT DISABILITY", "D6", "DIRECT SUB CONSOL", row);
            row = AddData(excel, timesNewRoman, "T", "Teacher Shortage Area", "B", "ALIGN REPAYMENT", "D6", "DIRECT SPOUSAL CONSOL", row);
            row = AddData(excel, timesNewRoman, "S", "Enrolled In School Full Time", "Y", "CLOSED SCHOOL", "D6", "DL SUB SPOUSAL CONSOL", row);
            row = AddData(excel, timesNewRoman, "S", "Semester Bridge", "B", "TRANSFER", "D1", "DIRECT SUB STAFFORD", row);
            row = AddData(excel, timesNewRoman, "W", "Desert Shield", "B", "DOD PAYMENT", "D5", "DIRECT UNSUB CONSOL", row);
            row = AddData(excel, timesNewRoman, "S", "Half Time School", "?", "RSDS REQ", "D2", "DIRECT UNSUB STAFFORD", row);
            row = AddData(excel, timesNewRoman, "4", "Parent of Student Enrolled Full Time", "E", "NATIONAL COMMUNITY SERVICE", "D5", "DL UNSUB SPOUSAL CONSOL", row);
            row = AddData(excel, timesNewRoman, "R", "Parent of Student in Rehab Training", "Y", "LOAN FORGIVENESS", "D8", "DIRECT TEACH LOAN", row);
            row = AddData(excel, timesNewRoman, "4", "Parent of Student in Grad Fellowship", "Y", "LOAN DEBT BURDEN FORBEARANCE", "FI", "FED INSURED STUD LOAN", row);
            row = AddData(excel, timesNewRoman, "4", "Parent of Student Enrolled Half Time", "E", "VARIABLE INTEREST RATE", "SLS", "FEDERAL SLS LOAN", row);
            row = AddData(excel, timesNewRoman, "S", "Heal Full Time School Enrollment", "Q", "INCOME SENSITIVE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "A", "Heal Active Duty in Armed Forces", "Z", "NATURAL DISASTER FORBEARANCEE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "F", "Heal Graduate Fellowship", "S", "IN SCHOOL", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "H", "Heal National Health", "E", "MEDICAL", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "K", "Heal Peace Corps", "Y", "COLLECTION SUSPENSION FORBEARANCE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "I", "Heal Intern/Residency", "E", "DEPENDENT STUDENT TEMPORARY HARDSHIP", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "C", "Economic Hardship", "E", "F-QUALITY EDUCATION", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "S", "Heal Primary Care", "Q", "REDUCED PAYMENT FORBEARANCE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "S", "Heal Chiropractic", "E", "EXCEPTIONAL DISCRETIONARY", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "S", "Veterinary", "Y", "DISCHARGE-DEATH", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "S", "Interim", "E", "INELIGIBLE FOR DEFERMENT", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "K", "Peace Corp", "E", "DELINQUENCY FORBEARANCE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "?", "Exceptional Regulation", "B", "REPURCHASE NON BANK CLAIM", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "W", "New Military", "E", "DSC-AWAITING APPLICATION", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "J", "Military Post Active Duty Student", "E", "DSC-UNPAID REFUND", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "C", "Econ Hardship Defer – FED Direct/Perkins", "Y", "LOCAL OR NATIONAL EMRG-MANDTRY", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "C", "Econ Hardship Defer – Public Assistance", "B", "DISASTER ADMIN - FORBEARANCE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "C", "Econ Hardship Defer – Peace Corps", "E", "LOCAL OR NATL EMERG", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "4", "Parent Plus In-School", "Z", "DISASTER-MANDATORY ADMIN FORBEARANCE", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "5", "Post Enrollment", "E", "IBR/ICR PROCESSING", string.Empty, string.Empty, row);
            row = AddData(excel, timesNewRoman, "?", "Unknown", string.Empty, string.Empty, string.Empty, string.Empty, row);

            excel.SetBackground("B1", "C1", Color.FromArgb(178, 178, 178));
            excel.SetBackground("E1", "F1", Color.FromArgb(178, 178, 178));
            excel.SetBackground("H1", "I1", Color.FromArgb(178, 178, 178));

            excel.SetBorder("B1", "C45");
            excel.SetBorder("E1", "F44");
            excel.SetBorder("H1", "I24");
        }

        private int AddData(ExcelGenerator excel, Font font, string code1, string text1, string code2, string text2, string code3, string text3, int row)
        {
            excel.InsertData("B" + row, "B" + row, code1, font, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("C" + row, "C" + row, text1, font, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("E" + row, "E" + row, code2, font, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("F" + row, "F" + row, text2, font, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("H" + row, "H" + row, code3, font, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);
            excel.InsertData("I" + row, "I" + row, text3, font, false, ExcelGenerator.HCellAlignment.Left, ExcelGenerator.VCellAlignment.Bottom);

            return ++row;
        }

        private List<Unwc14FileData> LoadTheFile(string file)
        {
            List<Unwc14FileData> filedata = new List<Unwc14FileData>();
            using (StreamReader sr = new StreamReader(file))
            {
                //We want to read in the header and save it in the list.  The header in the file will be the same as the header in the excel sheet.
                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    filedata.Add(new Unwc14FileData()
                    {
                        DateExtracted = temp[0],
                        Ssn = FormatSsn(temp[1]),
                        FirstName = temp[2],
                        LastName = temp[3],
                        Street = temp[4],
                        City = temp[5],
                        State = temp[6],
                        Zip = temp[7],
                        Birthdate = temp[8],
                        LoanType = temp[9],
                        LoanProgram = temp[10],
                        InterestRate = FormatInterestRate(temp[11]),
                        LoanStatus = temp[12],
                        PrincipalBalance = temp[13],
                        Interest = temp[14],
                        TotalBalance = temp[15],
                        LastDisbursement = temp[16],
                        LastDisbDate = temp[17],
                        LastPaymentTotal = temp[18],
                        LastPaymentAmountLoan = temp[19],
                        DateAssignedToDmcs = temp[20],
                        OriginationSource = temp[21],
                        DateAdded = temp[22],
                        CrbReportDate = temp[23],
                        CrbReportStatus = temp[24]
                    });
                }
            }

            return filedata;
        }

        private string FormatInterestRate(string rate)
        {
            switch (rate.Length)
            {
                case 3:
                    return rate.Insert(3, "00");
                case 4:
                    return rate.Insert(4, "0");
                default:
                    return rate;
            }
        }

        private string FormatSsn(string ssn)
        {
            switch (ssn.Length)
            {
                case 4:
                    return ssn.Insert(0, "'00000");
                case 5:
                    return ssn.Insert(0, "'0000");
                case 6:
                    return ssn.Insert(0, "'000");
                case 7:
                    return ssn.Insert(0, "'00");
                case 8:
                    return ssn.Insert(0, "'0");
                default:
                    return ssn;
            }
        }
    }

    class Unwc14FileData
    {
        public string DateExtracted { get; set; }
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Birthdate { get; set; }
        public string LoanType { get; set; }
        public string LoanProgram { get; set; }
        public string InterestRate { get; set; }
        public string LoanStatus { get; set; }
        public string PrincipalBalance { get; set; }
        public string Interest { get; set; }
        public string TotalBalance { get; set; }
        public string LastDisbursement { get; set; }
        public string LastDisbDate { get; set; }
        public string LastPaymentTotal { get; set; }
        public string LastPaymentAmountLoan { get; set; }
        public string DateAssignedToDmcs { get; set; }
        public string OriginationSource { get; set; }
        public string DateAdded { get; set; }
        public string CrbReportDate { get; set; }
        public string CrbReportStatus { get; set; }
    }
}
