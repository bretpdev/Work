using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace HRBRIDGE
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        private ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetKey")]
        public ApiKey GetKey(string keyName)
        {
            return LDA.ExecuteSingle<ApiKey>("hrbridge.GetKey", DataAccessHelper.Database.HumanResources, SqlParams.Single("KeyName", keyName)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateEmployees")]
        public bool UpdateEmployees(List<EmployeeSSMS> employees)
        {
            var result = LDA.Execute("hrbridge.UpdateEmployees", DataAccessHelper.Database.HumanResources, SqlParams.Single("Employees", employees.ToDataTable()));
            if(!result)
            {
                LogRun.AddNotification("Failed to update employees", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateCompensation")]
        public bool UpdateCompensation(List<CompensationSSMS> compensation)
        {
            var result = LDA.Execute("hrbridge.UpdateCompensation", DataAccessHelper.Database.HumanResources, SqlParams.Single("Compensation", compensation.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update compensation", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxCompensationUpdated")]
        public DateTime GetMaxCompensationUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxCompensationUpdated", DataAccessHelper.Database.HumanResources).Result;            
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxJobInfoUpdated")]
        public DateTime GetMaxJobInfoUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxJobInfoUpdated", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetEmployees")]
        public List<EmployeeBridgeSSMS> GetEmployees()
        {
            return LDA.ExecuteList<EmployeeBridgeSSMS>("hrbridge.GetEmployees", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetEmployeeNameFromId")]
        public string GetEmployeeNameFromId(int employeeId)
        {
            return LDA.ExecuteList<string>("hrbridge.GetEmployeeNameFromId", DataAccessHelper.Database.HumanResources, SqlParams.Single("EmployeeId", employeeId)).Result.FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetCompensation")]
        public List<CompensationSSMS> GetCompensation()
        {
            return LDA.ExecuteList<CompensationSSMS>("hrbridge.GetCompensation", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxAllocationUpdated")]
        public DateTime GetMaxAllocationUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxAllocationUpdated", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateAllocation")]
        public  bool UpdateAllocation(List<AllocationSSMS> allocation)
        {
            var result = LDA.Execute("hrbridge.UpdateAllocation", DataAccessHelper.Database.HumanResources, SqlParams.Single("Allocation", allocation.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update Allocation", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxParkingUpdated")]
        public DateTime GetMaxParkingUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxParkingUpdated", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateParking")]
        public bool UpdateParking(List<ParkingSSMS> parking)
        {
            var result = LDA.Execute("hrbridge.UpdateParking", DataAccessHelper.Database.HumanResources, SqlParams.Single("Parking", parking.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update Parking", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxJobCodeUpdated")]
        public DateTime GetMaxJobCodeUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxJobCodeUpdated", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxEmploymentStatusUpdated")]
        public DateTime GetMaxEmploymentStatusUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxEmploymentStatusUpdated", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateJobCodes")]
        public bool UpdateJobCodes(List<JobCodeSSMS> jobCode)
        {
            var result = LDA.Execute("hrbridge.UpdateJobCodes", DataAccessHelper.Database.HumanResources, SqlParams.Single("JobCodes", jobCode.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update JobCodes", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateJobInfo")]
        public bool UpdateJobInfo(List<JobInfoSSMS> jobInfo)
        {
            var result = LDA.Execute("hrbridge.UpdateJobInfo", DataAccessHelper.Database.HumanResources, SqlParams.Single("JobInfo", jobInfo.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update JobInfo", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateEmploymentStatus")]
        public bool UpdateEmploymentStatus(List<EmploymentStatusSSMS> employmentStatus)
        {
            var result = LDA.Execute("hrbridge.UpdateEmploymentStatus", DataAccessHelper.Database.HumanResources, SqlParams.Single("EmploymentStatus", employmentStatus.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update EmploymentStatus", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateEmployeeCompleted")]
        public bool UpdateEmployeeCompleted(int employeeId)
        {
            var result = LDA.Execute("hrbridge.UpdateEmployeeCompleted", DataAccessHelper.Database.HumanResources, SqlParams.Single("EmployeeId", employeeId));
            if (!result)
            {
                LogRun.AddNotification("Failed to update EmployeeCompleted", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateCompensationCompleted")]
        public bool UpdateCompensationCompleted(int employeeId)
        {
            var result = LDA.Execute("hrbridge.UpdateCompensationCompleted", DataAccessHelper.Database.HumanResources, SqlParams.Single("EmployeeId", employeeId));
            if (!result)
            {
                LogRun.AddNotification("Failed to update CompensationCompleted", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }


        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetMaxFTEUpdated")]
        public DateTime GetMaxFTEUpdated()
        {
            return LDA.ExecuteSingle<DateTime>("hrbridge.GetMaxFTEUpdated", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateFTE")]
        internal bool UpdateFTE(List<FTESSMS> FTE)
        {
            var result = LDA.Execute("hrbridge.UpdateFTE", DataAccessHelper.Database.HumanResources, SqlParams.Single("FTE", FTE.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update FTE", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetDestinations")]
        public List<DestinationRecord> GetDestinations()
        {
            return LDA.ExecuteList<DestinationRecord>("hrbridge.GetDestinations", DataAccessHelper.Database.HumanResources).Result;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateEmployeeCustomFields")]
        public bool UpdateEmployeeCustomFields(EmployeeCustomFields employee)
        {
            //We add the item to a list so we can create a data table out of it so that the sql parameters are neater
            var employeeList = new List<EmployeeCustomFields>();
            employeeList.Add(employee);
            return LDA.Execute("hrbridge.UpdateEmployeeCustomFields", DataAccessHelper.Database.HumanResources, SqlParams.Single("Employee", employeeList.ToDataTable()));
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateNewHires")]
        public void UpdateNewHires()
        {
            var result = LDA.Execute("hrbridge.UpdateNewHires", DataAccessHelper.Database.HumanResources);
            if (!result)
            {
                LogRun.AddNotification("Failed to update UpdateNewHires", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.UpdateEmployeeCustomReport")]
        public bool UpdateEmployeeCustomReport(List<CustomReport> employees)
        {
            var result = LDA.Execute("hrbridge.UpdateEmployeeCustomReport", DataAccessHelper.Database.HumanResources, SqlParams.Single("Employees", employees.ToDataTable()));
            if (!result)
            {
                LogRun.AddNotification("Failed to update UpdateEmployeeCustomReport", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.HumanResources, "hrbridge.GetBlacklist")]
        public List<BlacklistRecord> GetBlacklist()
        {
            var result = LDA.ExecuteList<BlacklistRecord>("hrbridge.GetBlacklist", DataAccessHelper.Database.HumanResources);
            if(!result.DatabaseCallSuccessful)
            {
                LogRun.AddNotification($"Failed to retrieve Blacklist Ex: {result?.CaughtException?.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return new List<BlacklistRecord>();
            }
            return result.Result;
        }
    }
}
