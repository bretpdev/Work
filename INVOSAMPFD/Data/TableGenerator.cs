using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVOSAMPFD
{
    /// <summary>
    /// Adds and completes standard tables to the report.
    /// </summary>
    class TableGenerator
    {
        static ReportTable separationSourceTable = null;
        public void GenerateSeparationSourceTable(ReportSection section)
        {
            if (separationSourceTable == null)
            {
                var table = section.CreateTable("Separation Source");
                table.AddRow("AN", "Application/Promissory Note");
                table.AddRow("AP", "Application");
                table.AddRow("BC", "Borrower Conversion File");
                table.AddRow("BL", "Borrower Letter");
                table.AddRow("CF", "Pre-Claim/Claim Form");
                table.AddRow("CH", "Clearinghouse");
                table.AddRow("CT", "Owner Conversion Transmittal");
                table.AddRow("CW", "Conversion Worksheet From Owner");
                table.AddRow("DF", "Deferment Form");
                table.AddRow("DS", "Disclosure Statement");
                table.AddRow("EG", "Expected Graduation Letter");
                table.AddRow("EI", "Exit Interview Letter");
                table.AddRow("GR", "Guarantor Run/Status Reconciliation RPT");
                table.AddRow("GS", "Guarantee Statement");
                table.AddRow("IN", "In School Consolidation");
                table.AddRow("ML", "Military");
                table.AddRow("NS", "NSLDS");
                table.AddRow("OL", "Owner Letter");
                table.AddRow("PC", "Phone Call Documentation in Conversion File");
                table.AddRow("RD", "Returned Disbursement");
                table.AddRow("SC", "Student Call");
                table.AddRow("SF", "School Enrollment Verification ");
                table.AddRow("SL", "School");
                separationSourceTable = table;
            }
            else
            {
                section.Items.Add(separationSourceTable);
            }
        }
        static ReportTable separationReasonTable = null;
        public void GenerateSeparationReasonTable(ReportSection section)
        {
            if (separationReasonTable == null)
            {
                var table = section.CreateTable("Separation Reason");
                table.AddRow("B", "B");
                table.AddRow("C", "C");
                table.AddRow("D", "D");
                table.AddRow("E", "E");
                table.AddRow("F", "F");
                table.AddRow("H", "H");
                table.AddRow("00", "Enrolled");
                table.AddRow("01", "Graduated");
                table.AddRow("02", "Withdraw");
                table.AddRow("03", "Transfer");
                table.AddRow("04", "Dismissed");
                table.AddRow("05", "On Leave");
                table.AddRow("06", "Deceased");
                table.AddRow("07", "Never Enrolled");
                table.AddRow("08", "Less Than Half Time");
                table.AddRow("09", "Desert Storm");
                table.AddRow("1", "1");
                table.AddRow("10", "Enrolled Half Time");
                table.AddRow("11", "Enrolled Full Time");
                table.AddRow("12", "School Closure");
                table.AddRow("13", "Ineligible Borrower");
                table.AddRow("14", "Expected Grade Letter");
                table.AddRow("15", "Call To Active Duty");
                table.AddRow("16", "Last Date Of Attendance");
                table.AddRow("17", "EGD");
                table.AddRow("18", "No Record Found");
                table.AddRow("19", "Enrolled 3qtr Time");
                table.AddRow("2", "2");
                table.AddRow("3", "3");
                table.AddRow("4", "4");
                table.AddRow("5", "5");
                separationReasonTable = table;
            }
            else
            {
                section.Items.Add(separationReasonTable);
            }
        }
    }
}