using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CCCLOSURES
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        /// <summary>
        /// Gets all regions
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[ccclosures].GetRegionsByAccess")]
        public List<Regions> PopulateRegions(string userName = "")
        {
            userName = userName.IsPopulated() ? userName : Environment.UserName;
            return LDA.ExecuteList<Regions>("[ccclosures].GetRegionsByAccess", DataAccessHelper.Database.Csys,
                Sp("WindowsUserId", userName)).Result;
        }

        /// <summary>
        /// Gets all active schedules from the IVRControl DB
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IVRControl, "GetScheduleData")]
        public List<ScheduleSelctionData> PopulateScheduleData()
        {
            return LDA.ExecuteList<ScheduleSelctionData>("GetScheduleData", DataAccessHelper.Database.IVRControl).Result;
        }

        /// <summary>
        /// Gets All Active Status Codes
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.IVRControl, "GetActiveStatusCodes")]
        public List<StatusCodes> PopulateStatusCodes()
        {
            return LDA.ExecuteList<StatusCodes>("GetActiveStatusCodes", DataAccessHelper.Database.IVRControl).Result;
        }
        
        /// <summary>
        /// Marks the schedule as deleted
        /// </summary>
        /// <param name="selectedItem"></param>
        [UsesSproc(DataAccessHelper.Database.IVRControl, "MarkScheduleDeleted")]
        public void DeleteSchedule(ScheduleSelctionData selectedItem)
        {
            LDA.Execute("MarkScheduleDeleted", DataAccessHelper.Database.IVRControl,
                Sp("StatusScheduleId", selectedItem.StatusScheduleId),
                Sp("DeletedBy", Environment.UserName));
        }

        /// <summary>
        /// Adds new schedule
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.IVRControl, "AddNewSchedule")]
        public void AddSchedule(DateTime start, DateTime end, int statucCodeId, int regionId)
        {
            LDA.Execute("AddNewSchedule", DataAccessHelper.Database.IVRControl,
                Sp("Start", start),
                Sp("End", end),
                Sp("StatusCodeId", statucCodeId),
                Sp("RegionId", regionId),
                Sp("AddedBy", Environment.UserName));
        }

        /// <summary>
        /// Updates current schedule
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.IVRControl, "UpdateSchedule")]
        public void UpdateSchedule(DateTime start, DateTime end, int statucCodeId, int regionId, ScheduleSelctionData data)
        {
            LDA.Execute("UpdateSchedule", DataAccessHelper.Database.IVRControl,
                Sp("StatusScheduleId", data.StatusScheduleId),
                Sp("Start", start),
                Sp("End", end),
                Sp("StatusCodeId", statucCodeId),
                Sp("RegionId", regionId),
                Sp("UpdatedBy", Environment.UserName));
        }

        /// <summary>
        /// Checks for overlapping dates
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.IVRControl, "CheckForOverlappingDates")]
        public bool CheckOverlappintDates(DateTime newStart, DateTime newEnd, int region, ScheduleSelctionData data)
        {
            return LDA.ExecuteSingle<int>("CheckForOverlappingDates", DataAccessHelper.Database.IVRControl,
                Sp("StatusScheduleId", data == null ? 0 : data.StatusScheduleId),
                Sp("StartDate", newStart),
                Sp("EndDate", newEnd),
                Sp("RegionId", region)).Result != 0;
        }

        /// <summary>
        /// Checks the users access to run IVR
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "[ccclosures].CheckUsersRoleForIvr")]
        public bool CheckUserAccess()
        {
            return LDA.ExecuteSingle<int>("[ccclosures].CheckUsersRoleForIvr", DataAccessHelper.Database.Csys,
                Sp("WindowsUserId", Environment.UserName)).Result > 0;
        }

        public SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}