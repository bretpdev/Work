using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using Uheaa.Common.ProcessLogger;
using System.Data;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace HRBRIDGE
{
    class BambooHRProcessor : ProcessorBase
    {
        private HttpClient Client { get; set; }
        private string CompanySubdomain { get; set; } = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? "ushe" : "ushetest";
        public Dictionary<int, AliasRecord> AliasDictionary { get; set; }
        public BambooHRProcessor(ProcessLogRun logRun) : base(logRun)
        {
            //ADDRESS
            Client = new HttpClient();
            Client.BaseAddress = new Uri($"https://api.bamboohr.com/api/gateway.php/{CompanySubdomain}/");
            Client.DefaultRequestHeaders.Accept.Clear();

            //CONTENT TYPE HEADER
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //Client.DefaultRequestHeaders.Add("Accept", "application/json");

            //AUTHORIZATION HEADER
            string authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(DA.GetKey("BambooHR").Key + ":x")); //Bamboo has custom formatting here
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
        }

        public void Process()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        //This seems to be a shortcut to do what ProcessEmployees does
        public async Task ProcessCompensationData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxCompensationUpdated();
                string table = "compensation";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                Compensation employees = await GetAsync<Compensation>(Client, url);

                //iterate through employees
                List<CompensationSSMS> compensation = new List<CompensationSSMS>();
                foreach(var employee in employees.Employees.Keys)
                {
                    foreach (var employeeRecords in employees.Employees[employee].Rows)
                    {
                        CompensationSSMS record = new CompensationSSMS();
                        record.UpdatedAt = employees.Employees[employee].LastChanged;

                        record.EmployeeId = employee.ToInt();
                        record.Comment = employeeRecords.Comment;
                        record.Exempt = employeeRecords.Exempt;
                        record.PaidPer = employeeRecords.PaidPer;
                        record.PaySchedule = employeeRecords.PaySchedule;
                        record.Rate = employeeRecords.Rate;
                        record.Reason = employeeRecords.Reason;
                        record.StartDate = employeeRecords.StartDate;
                        record.Type = employeeRecords.Type;

                        compensation.Add(record);
                    }
                }    
                

                var result = DA.UpdateCompensation(compensation);
                if (result)
                {
                    LogRun.AddNotification("Compensation Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Compensation. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task ProcessAllocationData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxAllocationUpdated();
                string table = "customAllocation";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                Allocation allocations = await GetAsync<Allocation>(Client, url);

                //iterate through employees
                List<AllocationSSMS> allocation = new List<AllocationSSMS>();
                foreach (var employee in allocations.Employees.Keys)
                {
                    var updatedAllocations = allocations.Employees[employee];

                    foreach (var row in allocations.Employees[employee].Rows)
                    {
                        AllocationSSMS record = new AllocationSSMS();
                        record.UpdatedAt = updatedAllocations.LastChanged;
                        record.EmployeeId = employee.ToInt();
                        record.CostCenter = row.CostCenter;
                        record.BusinessUnit = row.BusinessUnit;
                        record.AllocationEffectiveDate = row.AllocationEffectiveDate;
                        record.Account = row.Account;
                        record.FTE = row.FTE;
                        record.SquareFootage = row.SquareFootage;
                        allocation.Add(record);
                    }
                }

                var result = DA.UpdateAllocation(allocation);
                if (result)
                {
                    LogRun.AddNotification("Allocation Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Allocation. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task ProcessJobInfoData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxJobInfoUpdated();
                string table = "jobInfo";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                JobInfo jobInfo = await GetAsync<JobInfo>(Client, url);

                //iterate through employees
                List<JobInfoSSMS> info = new List<JobInfoSSMS>();
                foreach (var employee in jobInfo.Employees.Keys)
                {
                    var updatedInfo = jobInfo.Employees[employee];

                    foreach (var row in jobInfo.Employees[employee].Rows)
                    {
                        JobInfoSSMS record = new JobInfoSSMS();
                        record.UpdatedAt = updatedInfo.LastChanged;
                        record.EmployeeId = employee.ToInt();
                        record.Date = row.Date;
                        record.Department = row.Department;
                        record.Division = row.Division;
                        record.JobTitle = row.JobTitle;
                        record.Location = row.Location;
                        record.ReportsTo = row.ReportsTo;
                        info.Add(record);
                    }
                }

                var result = DA.UpdateJobInfo(info);
                if (result)
                {
                    LogRun.AddNotification("Job Info Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process JobInfo. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }


        public async Task ProcessParkingData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxParkingUpdated();
                string table = "customParking";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                Parking parkingRecords = await GetAsync<Parking>(Client, url);

                //iterate through employees
                List<ParkingSSMS> parking = new List<ParkingSSMS>();
                foreach (var employee in parkingRecords.Employees.Keys)
                {
                    var updatedParking = parkingRecords.Employees[employee];

                    foreach (var row in parkingRecords.Employees[employee].Rows)
                    {
                        ParkingSSMS record = new ParkingSSMS();
                        record.UpdatedAt = updatedParking.LastChanged;
                        record.EmployeeId = employee.ToInt();
                        record.Garage = row.Garage;
                        record.Type = row.Type;
                        record.FobId = row.FobID;

                        parking.Add(record);
                    }
                }

                var result = DA.UpdateParking(parking);
                if (result)
                {
                    LogRun.AddNotification("Parking Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Parking. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task ProcessJobCodeData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxJobCodeUpdated();
                string table = "customJobCode1";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                JobCode jobCodeRecords = await GetAsync<JobCode>(Client, url);

                //iterate through employees
                List<JobCodeSSMS> jobCode = new List<JobCodeSSMS>();
                foreach (var employee in jobCodeRecords.Employees.Keys)
                {
                    var updatedJobCode = jobCodeRecords.Employees[employee];

                    foreach (var row in jobCodeRecords.Employees[employee].Rows)
                    {
                        JobCodeSSMS record = new JobCodeSSMS();
                        record.UpdatedAt = updatedJobCode.LastChanged;
                        record.EmployeeId = employee.ToInt();
                        record.JobCodeEffectiveDate = row.JobCodeEffectiveDate;
                        record.JobCode = row.JobCode;
                        record.UOfUJobTitle = row.UOfUJobTitle;

                        jobCode.Add(record);
                    }
                }

                var result = DA.UpdateJobCodes(jobCode);
                if (result)
                {
                    LogRun.AddNotification("Job Codes Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Job Codes. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task ProcessEmpoymentStatusData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxEmploymentStatusUpdated();
                string table = "employmentStatus";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                EmploymentStatus employmentRecords = await GetAsync<EmploymentStatus>(Client, url);

                //iterate through employees
                List<EmploymentStatusSSMS> employmentStatus = new List<EmploymentStatusSSMS>();
                foreach (var employee in employmentRecords.Employees.Keys)
                {
                    var updatedEmployment = employmentRecords.Employees[employee];

                    foreach (var row in employmentRecords.Employees[employee].Rows)
                    {
                        EmploymentStatusSSMS record = new EmploymentStatusSSMS();
                        record.UpdatedAt = updatedEmployment.LastChanged;
                        record.EmployeeId = employee.ToInt();
                        record.Date = row.Date;
                        record.ElligableForRehire = row.ElligableForRehire;
                        record.EmploymentStatus = row.EmploymentStatus;
                        record.EmploymentStatusComment = row.EmploymentStatusComment;
                        record.TerminationReason = row.TerminationReason;
                        record.TerminationType = row.TerminationType;

                        employmentStatus.Add(record);
                    }
                }

                var result = DA.UpdateEmploymentStatus(employmentStatus);
                if (result)
                {
                    LogRun.AddNotification("Employment Status Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Employment Status. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }


        public async Task ProcessFTEData()
        {
            try
            {
                DateTime lastUpdatedSuccessfully = DA.GetMaxFTEUpdated();
                string table = "customFTE2";
                string url = $"v1/employees/changed/tables/{table}?since={ToISO8601DateString(lastUpdatedSuccessfully)}"; //the api specifies the ISO 8601 date format
                FTE FTERecords = await GetAsync<FTE>(Client, url);

                //iterate through employees
                List<FTESSMS> FTE = new List<FTESSMS>();
                foreach (var employee in FTERecords.Employees.Keys)
                {
                    var updatedFTERecord = FTERecords.Employees[employee];

                    foreach (var row in FTERecords.Employees[employee].Rows)
                    {
                        FTESSMS record = new FTESSMS();
                        record.UpdatedAt = updatedFTERecord.LastChanged;
                        record.EmployeeId = employee.ToInt();
                        record.FTEEffectiveDate = row.FTEEffectiveDate;
                        record.FTE = row.FTE;
                        record.Notes = row.Notes;

                        FTE.Add(record);
                    }
                }


                var result = DA.UpdateFTE(FTE);
                if (result)
                {
                    LogRun.AddNotification("FTE Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process FTE. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task ProcessCustomAliasReport()
        {
            try
            {
                //Get information, will probably be an enumerable
                var reportId = EnterpriseFileSystem.GetPath("HRBRIDGEAliasReportId");
                string url = $"v1/reports/{reportId.ToInt()}";//106

                var employees = await GetAsync<Alias>(Client, url);

                ////Getting employees failed
                if (employees == default(Alias))
                {
                    return;
                }

                Dictionary<int, AliasRecord> aliasDictionary = new Dictionary<int, AliasRecord>();

                //Set up the alias dictionary
                foreach (var employee in employees.Employees)
                {
                    if (!employee.PreferredName.IsNullOrEmpty())
                    {
                        aliasDictionary.Add(employee.EmployeeId, employee);
                    }
                }

                AliasDictionary = aliasDictionary;
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Alias Custom Report. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        public async Task ProcessCustomEmployeeReport()
        {
            try
            {
                //Get information, will probably be an enumerable
                var reportId = EnterpriseFileSystem.GetPath("HRBRIDGECustomReportId");
                string url = $"v1/reports/{reportId.ToInt()}";//102

                var employees = await GetAsync<CustomEmployeeReport>(Client, url);

                ////Getting employees failed
                if (employees == default(CustomEmployeeReport))
                {
                    return;
                }

                List<CustomReport> customPopulation = new List<CustomReport>();

                //Query information for all employees
                foreach (var employee in employees.Employees)
                {
                    CustomReport record = new CustomReport();
                    record.EmployeeId = employee.Id;
                    record.FirstName = employee.FirstName;
                    record.LastName = employee.LastName;
                    record.WorkEmail = employee.WorkEmail;
                    record.JobTitle = employee.JobTitle;
                    record.ReportsTo = employee.ReportsTo;
                    record.Department = employee.Department;
                    record.Location = employee.Location;
                    record.Division = employee.Division;
                    record.BirthDate = employee.Birthdate;
                    record.Clearance = employee.Clearance;
                    record.DepartmentId = employee.DepartmentId;
                    record.EEOJobCategory = employee.EEOJobCategory;
                    record.EmployeeLevel = employee.EmployeeLevel;
                    record.EmployeeNumber = employee.EmployeeNumber;
                    record.Ethnicity = employee.Ethnicity;
                    record.Gender = employee.Gender;
                    //default the hire date to 1900 if the hire date provided by bambooHR is invalid
                    var hireDate = employee.HireDate.ToDateNullable();
                    record.HireDate = hireDate.HasValue ? hireDate.Value : new DateTime(1900, 1, 1);
                    record.SCACode = employee.SCACode;
                    record.VacationCategory = employee.VacationCategory;
                    record.EffectiveDate = employee.EffectiveDate.ToDateNullable();

                    customPopulation.Add(record);
                }

                var result = DA.UpdateEmployeeCustomReport(customPopulation);
                if (result)
                {
                    LogRun.AddNotification("Employee Custom Report Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }

            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Employee Custom Report. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        public async Task ProcessEmployees()
        {
            try
            {
                //Get information, will probably be an enumerable
                string url = "v1/employees/directory/";
                EmployeeDirectory employees = await GetAsync<EmployeeDirectory>(Client, url);

                //Getting employees failed
                if (employees == default(EmployeeDirectory))
                {
                    return;
                }

                List<EmployeeSSMS> employeesSSMS = new List<EmployeeSSMS>();
                //Query information for all employees
                foreach (var employee in employees.Employees)
                {
                    EmployeeSSMS record = new EmployeeSSMS();
                    record.EmployeeId = employee.Id;
                    record.FirstName = employee.FirstName;
                    record.LastName = employee.LastName;
                    record.WorkEmail = employee.WorkEmail;
                    record.JobTitle = employee.JobTitle;
                    record.Supervisor = employee.Supervisor;
                    record.Department = employee.Department;
                    record.Location = employee.Location;
                    record.Division = employee.Division;

                    employeesSSMS.Add(record);
                }

                var result = DA.UpdateEmployees(employeesSSMS);
                if (result)
                {
                    LogRun.AddNotification("Employee Directory Updates Completed Successfully", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }

                //Custom field processing needs to be done on an employee to employee basis because the API only allows it that way
                foreach(var employee in employees.Employees)
                {
                    await ProcessEmployee(employee.Id);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Employee Directory. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        public async Task ProcessEmployee(int employeeId)
        {
            try
            {
                //Get information, will probably be an enumerable
                string url = $"v1/employees/{employeeId.ToString()}/?fields=hireDate,ethnicity,serviceDate,employeeNumber,eeo,customClearance,dateOfBirth,customEmployeeLevel,gender,customSCACode,customVacationCategory,customDepartmentID";
                EmployeeCustomFields employee = await GetAsync<EmployeeCustomFields>(Client, url);

                //Getting employees failed
                if (employee == default(EmployeeCustomFields))
                {
                    return;
                }

                var result = DA.UpdateEmployeeCustomFields(employee);
                if (result)
                {
                    LogRun.AddNotification($"Employee Custom Fields Updates Completed Successfully for EmployeeId {employeeId.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Employee Custom Fields for EmployeeId {employeeId.ToString()}. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }

        public async Task RunAsync()
        {
            //TODO Parallelize if necessary
            await ProcessCustomAliasReport();
            await ProcessCustomEmployeeReport();
            await ProcessEmployees();
            await ProcessCompensationData();
            await ProcessAllocationData();
            await ProcessJobCodeData();
            await ProcessJobInfoData();
            await ProcessEmpoymentStatusData();
            await ProcessFTEData();
            await ProcessParkingData();
        }
    }
}
