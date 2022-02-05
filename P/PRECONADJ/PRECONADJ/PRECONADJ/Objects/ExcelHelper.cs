using System;
using System.Collections.Generic;
using Uheaa.Common;
using Excel = Microsoft.Office.Interop.Excel;

namespace PRECONADJ
{
    public class ExcelHelper
    {
        public static List<LineData> ReadWorksheetToList(Excel.Worksheet workSheet, string tab)
        {
            List<LineData> sheetData = new List<LineData>();
            int row = 2;
            while (row != -1)
            {
                try
                {
                    LineData ld = new LineData();
                    ld.PrincipalBalance = DoubleToBalance((double)(workSheet.Cells[row, 2] as Excel.Range).Value);
                    ld.AdditionalDisbursement = ((workSheet.Cells[row, 3] as Excel.Range).Text != "") ? DoubleToBalance(((double)(workSheet.Cells[row, 3] as Excel.Range).Value)) : "";
                    ld.InterestRate = ((double)(workSheet.Cells[row, 4] as Excel.Range).Value).ToString();
                    string payDate = ((workSheet.Cells[row, 8] as Excel.Range).Text).ToString();
                    if (!payDate.ToDateNullable().HasValue)//If next row has an invalid date, we've reached the end
                        break;
                    ld.PaymentDates = payDate.ToDateNullable().Value;
                    ld.PaymentAmount = DoubleToBalance((double)(workSheet.Cells[row, 9] as Excel.Range).Value);
                    ld.Cap = ((workSheet.Cells[row, 10] as Excel.Range).Text).ToString();
                    ld.PrincipalApplied = DoubleToBalance((double)(workSheet.Cells[row, 11] as Excel.Range).Value);
                    ld.InterestApplied = DoubleToBalance((double)(workSheet.Cells[row, 12] as Excel.Range).Value);
                    sheetData.Add(ld);
                    row++;
                    //If next row has no data we've reached the end
                    if ((workSheet.Cells[row, 2] as Excel.Range).Text == "")
                        row = -1;
                }
                catch (Exception ex)
                {
                    Dialog.Error.Ok($"There is problem with Tab: {tab} Line: {row} Exception: {ex.ToString()}", "Format Issue");
                    return null;
                }
            }
            return sheetData;
        }

        public static string TranslateToTabName(int num)
        {
            if (num < 0)
                return null;
            string stringNum = num.ToString().PadLeft(3, '0');
            if (stringNum.Length > 3)
                return null;
            return stringNum;
        }

        public static string DoubleToBalance(double d)
        {
            return string.Format("{0:0.00}", d);
        }
    }
}