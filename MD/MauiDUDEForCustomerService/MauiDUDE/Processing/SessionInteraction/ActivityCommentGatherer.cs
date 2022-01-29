using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace MauiDUDE
{
    public class ActivityCommentGatherer
    {
        public enum DaysOrNumberOf
        {
            Days = 0,
            NumberOf = 1
        }

        private string _personId;
        private ReflectionInterface RI;

        public ActivityCommentGatherer(string personId)
        {
            _personId = personId;
            RI = SessionInteractionComponents.RI;
        }

        /// <summary>
        /// Creates data table with Compass acivity history populated in it
        /// </summary>
        public DataTable PopulateActivityHistoryFromTD2A(DaysOrNumberOf daysOrNumberOf, int count, DataAccessHelper.Region region)
        {
            //this is the table that will fill the datagrid
            DataTable activityHistoryTable = new DataTable();
            activityHistoryTable.Columns.Add("Request Code");
            activityHistoryTable.Columns.Add("Response Code");
            activityHistoryTable.Columns.Add("Request Description");
            activityHistoryTable.Columns.Add("Request Date");
            activityHistoryTable.Columns.Add("Response Date");
            activityHistoryTable.Columns.Add("Requestor");
            activityHistoryTable.Columns.Add("Performed");
            activityHistoryTable.Columns.Add("Text");
            int currentCount = 0;

            RI.Stup(region, DataAccessHelper.CurrentMode);

            RI.FastPath($"TX3ZITD2A{_personId}");
            if(RI.CheckForText(1,72,"TDX2B") && daysOrNumberOf == DaysOrNumberOf.Days && count == 0)
            {
                RI.PutText(4, 66, "X");
            }
            else if(RI.CheckForText(1,72,"TDX2B") && daysOrNumberOf == DaysOrNumberOf.Days)
            {
                DateTime startDate = DateTime.Today.AddDays(count * -1);
                RI.PutText(21, 16, startDate.ToString("MMddyy"));
                RI.PutText(21, 30, DateTime.Today.ToString("MMddyy"));
            }
            else if(RI.CheckForText(1,72,"TDX2B"))
            {
                RI.PutText(4, 66, "X");
            }
            RI.Hit(Key.Enter);
            if(RI.CheckForText(1,72,"TDX2C"))
            {
                RI.PutText(5, 14, "X", Key.Enter);
            }

            if(!RI.CheckForText(23,2, "01019 ENTERED KEY NOT FOUND"))
            {
                //Get borrower activity records
                do
                {
                    currentCount++;
                    string[] columnValues = new string[8];
                    columnValues[0] = RI.GetText(13, 2, 5);
                    columnValues[1] = RI.GetText(15, 2, 5);
                    columnValues[2] = RI.GetText(13, 8, 20);
                    columnValues[3] = RI.GetText(13, 31, 8);
                    columnValues[4] = RI.GetText(15, 31, 8);
                    columnValues[5] = RI.GetText(13, 40, 9);
                    columnValues[6] = RI.GetText(15, 51, 9);
                    columnValues[7] = $"{RI.GetText(17,2,78)} {RI.GetText(18,2,78)} {RI.GetText(19,2,78)}";
                    activityHistoryTable.Rows.Add(columnValues);
                    RI.Hit(Key.F8);
                    if(RI.CheckForText(23,2, "01033 PRESS ENTER TO DISPLAY MORE DATA"))
                    {
                        RI.Hit(Key.Enter);
                        RI.PutText(5, 14, "X", Key.Enter);
                    }
                } while (!RI.CheckForText(23, 2, "90007") && (daysOrNumberOf != DaysOrNumberOf.NumberOf || currentCount != count));
            }
            return activityHistoryTable;
        }

        public DataTable PopulateActivityHistoryFromLP50(DaysOrNumberOf daysOrNumberOf, int count)
        {
            //this is the table that will fill the datagrid
            DataTable activityHistoryTable = new DataTable();
            activityHistoryTable.Columns.Add("Activity Date"); //, GetType(Date))
            activityHistoryTable.Columns.Add("Activity Type");
            activityHistoryTable.Columns.Add("Contact Type");
            activityHistoryTable.Columns.Add("Action Code");
            activityHistoryTable.Columns.Add("Action Code Description");
            activityHistoryTable.Columns.Add("User ID");
            activityHistoryTable.Columns.Add("Activity Comment");

            DateTime startDate = DateTime.Today.AddDays(count * -1);
            int currentCount = 0;

            RI.Stup(DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
            RI.FastPath($"LP50I{_personId}");
            if(count == 0 || daysOrNumberOf == DaysOrNumberOf.NumberOf)
            {
                //get all days
                RI.PutText(5, 14, "X", Key.Enter);
            }
            else
            {
                RI.PutText(18, 29, startDate.ToString("MMddyyyy"));
                RI.PutText(18, 41, DateTime.Today.ToString("MMddyyyy"));
            }
            if(RI.CheckForText(1,58, "ACTIVITY SUMMARY SELECT"))
            {
                //found records
                RI.PutText(3, 13, "X", Key.Enter);
            }

            if(RI.CheckForText(1,58, "ACTIVITY DETAIL DISPLAY"))
            {
                do
                {
                    string[] dataRow = new string[7];
                    dataRow[0] = RI.GetText(7, 15, 8).ToDateFormat();
                    dataRow[1] = RI.GetText(7, 2, 2);
                    dataRow[2] = RI.GetText(7, 5, 2);
                    dataRow[3] = RI.GetText(8, 2, 5);
                    dataRow[4] = RI.GetText(7, 34, 23);
                    dataRow[5] = RI.GetText(8, 69, 7);
                    dataRow[6] = $"{RI.GetText(13, 2, 78)} {RI.GetText(14, 2, 78)} {RI.GetText(14, 2, 78)} {RI.GetText(15, 2, 78)} {RI.GetText(16, 2, 78)} {RI.GetText(17, 2, 78)} {RI.GetText(18,2,78)} {RI.GetText(19,2,78)} {RI.GetText(20, 2, 78)} {RI.GetText(21, 2, 78)}";
                    activityHistoryTable.Rows.Add(dataRow);
                    RI.Hit(Key.F8);
                } while (!RI.CheckForText(22, 3, "46004 NO MORE DATA TO DISPLAY") && (daysOrNumberOf != DaysOrNumberOf.NumberOf || currentCount != count));
            }

            return activityHistoryTable;
        }
    }
}
