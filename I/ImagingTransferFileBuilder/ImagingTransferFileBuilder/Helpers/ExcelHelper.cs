using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ImagingTransferFileBuilder
{
    public class NoSheetFound : Exception { }
    public struct Borrower
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GuarantyDate { get; set; }
        public string DealID { get; set; }
        public string LoanID { get; set; }
    }
    public static class ExcelHelper
    {
        private enum BorrowerFields
        {
            SSN,
            FirstName,
            LastName,
            GuarantyDate,
            DealID, //AKA award id
            LoanID
        }
        public static Dictionary<string, Borrower> GetBorrowers(string location, string sheetName, bool includeDealID)
        {
            DualReturn dr = GetBorrowersRaw(location, sheetName, includeDealID);
            if (dr != null) return dr.DistinctBorrowers;
            return null;
        }

        public static List<Borrower> GetAllBorrowers(string location, string sheetName, bool includeDealID)
        {
            DualReturn dr = GetBorrowersRaw(location, sheetName, includeDealID);
            if (dr != null) return dr.AllBorrowers;
            return null;
        }
        private class DualReturn
        {
            public Dictionary<string, Borrower> DistinctBorrowers { get; set; }
            public List<Borrower> AllBorrowers { get; set; }
        }
        private static DualReturn GetBorrowersRaw(string location, string sheetName, bool includeDealID)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbooks workbooks = excel.Workbooks;
            Workbook wb = workbooks.Open(location, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            Sheets sheets = wb.Worksheets;
            Worksheet ws = null;
            try
            {
                ws = (Worksheet)sheets[sheetName];
            }
            catch (COMException)
            {
                workbooks.Close();
                Progress.Failure();
                throw new NoSheetFound();
            }
            Range range = ws.UsedRange;
            Dictionary<BorrowerFields, int> columns = new Dictionary<BorrowerFields, int>();
            Dictionary<BorrowerFields, string> colNames = new Dictionary<BorrowerFields, string>();
            colNames[BorrowerFields.SSN] = "SSN";
            colNames[BorrowerFields.FirstName] = "DM_PRS_1";
            colNames[BorrowerFields.LastName] = "DM_PRS_LST";
            colNames[BorrowerFields.GuarantyDate] = "LD_LON_GTR";
            if (includeDealID)
                colNames[BorrowerFields.DealID] = "Deal ID";
            colNames[BorrowerFields.LoanID] = "AWARD_ID";
            for (int i = 1; i <= range.Columns.Count; i++)
            {
                string val = (string)((Range)range.Cells[1, i]).Value2;
                foreach (string name in colNames.Values)
                {
                    if (name == val)
                    {
                        columns[colNames.Where(o => o.Value == name).Single().Key] = i;
                        break;
                    }
                }

            }
            Func<bool, string, bool> CheckCol = new Func<bool, string, bool>(
                (x, y) =>
                {
                    if (!x)
                    {
                        Results.LogError("Column {0} not found in excel document.", y);
                        Progress.Failure();
                        return false;
                    }
                    return true;
                });
            bool passed = true;
            foreach (BorrowerFields bf in colNames.Keys)
                passed &= CheckCol(columns.Keys.Contains(bf), colNames[bf]);

            Dictionary<string, Borrower> distinctBorrowers = new Dictionary<string, Borrower>();
            List<Borrower> allBorrowers = new List<Borrower>();
            if (passed)
            {
                for (int i = 2; i <= range.Rows.Count; i++)
                {
                    Borrower b = new Borrower();
                    b.SSN = (string)((Range)range.Cells[i, columns[BorrowerFields.SSN]]).Text;
                    if (string.IsNullOrEmpty(b.SSN)) continue;
                    b.FirstName = (string)((Range)range.Cells[i, columns[BorrowerFields.FirstName]]).Text;
                    b.LastName = (string)((Range)range.Cells[i, columns[BorrowerFields.LastName]]).Text;
                    b.GuarantyDate = (string)((Range)range.Cells[i, columns[BorrowerFields.GuarantyDate]]).Text;
                    b.GuarantyDate = DateTime.Parse(b.GuarantyDate).ToString("MM/dd/yyyy");
                    b.LoanID = (string)((Range)range.Cells[i, columns[BorrowerFields.LoanID]]).Text;
                    if (includeDealID)
                        b.DealID = (string)((Range)range.Cells[i, columns[BorrowerFields.DealID]]).Text;
                    allBorrowers.Add(b);
                    if (distinctBorrowers.ContainsKey(b.SSN))
                    {
                        Borrower old = distinctBorrowers[b.SSN];
                        if (DateTime.Parse(old.GuarantyDate) < DateTime.Parse(b.GuarantyDate))
                        {
                            b = old;
                        }
                    }
                    distinctBorrowers[b.SSN] = b;
                }
            }

            workbooks.Close();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(sheets);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(range);
            GC.Collect();
            DualReturn dr = new DualReturn();
            dr.DistinctBorrowers = distinctBorrowers;
            dr.AllBorrowers = allBorrowers;
            if (passed)
                return dr;
            else
                return null;
        }

    }
}
