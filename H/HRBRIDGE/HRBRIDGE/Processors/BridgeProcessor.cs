using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace HRBRIDGE
{
    class BridgeProcessor : ProcessorBase
    {
        private HttpClient UheaaClient { get; set; }
        private HttpClient My529Client { get; set; }
        private string UheaaCompanySubdomain { get; set; } = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? "uheaa-utah" : "ssttesting-utah";
        private string My529CompanySubdomain { get; set; } = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? "my529-utah" : "ssttesting-utah";
        private Dictionary<int, AliasRecord> AliasDictionary { get; set; }
        private IEnumerable<EmployeeDirectoryRecord> EmployeeUpdates { get; set; }
        private string My529Destination { get; set; } = "MY529";
        private string UheaaDestination { get; set; } = "UHEAA";

        public BridgeProcessor(ProcessLogRun logRun, Dictionary<int, AliasRecord> aliasDictionary) : base(logRun)
        {
            AliasDictionary = aliasDictionary ?? new Dictionary<int, AliasRecord>();

            //UHEAA SITE
            //ADDRESS
            UheaaClient = new HttpClient();
            UheaaClient.BaseAddress = new Uri($"https://{UheaaCompanySubdomain}.bridgeapp.com/");
            UheaaClient.DefaultRequestHeaders.Accept.Clear();

            //CONTENT TYPE HEADER
            UheaaClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //AUTHORIZATION HEADER
            ApiKey uheaaKey = DA.GetKey("BridgeUheaa");
            string uheaaAuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(uheaaKey.Key + ":" + uheaaKey.Secret));
            UheaaClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", uheaaAuthInfo);

            //MY529 SITE
            //ADDRESS
            My529Client = new HttpClient();
            My529Client.BaseAddress = new Uri($"https://{My529CompanySubdomain}.bridgeapp.com/");
            My529Client.DefaultRequestHeaders.Accept.Clear();

            //CONTENT TYPE HEADER
            UheaaClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //AUTHORIZATION HEADER
            ApiKey my529Key = DA.GetKey("BridgeMy529");
            string my529AuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(my529Key.Key + ":" + my529Key.Secret));
            My529Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", my529AuthInfo);
        }




        public void Process()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public async Task<CustomFieldListItemRecord> ProcessCustomFields(HttpClient client)
        {
            try
            {
                //Get information, will probably be an enumerable
                string url = $"api/author/custom_fields";
                CustomFieldListItemRecord fields = await GetAsync<CustomFieldListItemRecord>(client, url);
                return fields;
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Bridge Custom Fields. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
        }

        //This seems to be a shortcut to do what ProcessEmployees does
        public async Task ProcessEmployeeTableData(List<EmployeeBridgeSSMS> employees, List<CompensationSSMS> compensation, CustomFieldListItemRecord uheaaCustomFields, CustomFieldListItemRecord my529CustomFields)
        {
            try
            {
                //Get information, will probably be an enumerable
                string url = $"api/author/users?includes[]=manager&includes[]=custom_fields&limit={int.MaxValue.ToString()}";
                UserDirectoryRecord my529users = await GetAsync<UserDirectoryRecord>(My529Client, url);
                UserDirectoryRecord uheaaUsers = await GetAsync<UserDirectoryRecord>(UheaaClient, url);
                var destinations = DA.GetDestinations();

                //await DeleteAllUsers(My529Client, my529users);
                foreach (var employee in employees)
                {
                    if(employee == null || employee.Location == null || employee.Department == null)
                    {
                        string name = employee == null ? "NULL EMPLOYEE" : employee.FirstName + " " + employee.LastName;
                        LogRun.AddNotification($"Employee missing Location or Department. Employee Name {name}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        continue;
                    }

                    //determine which website the employee needs to be imported to(possibly both)
                    var locationDestinations = destinations.Where(p => p.DestinationSource.ToUpper() == "LOCATION" && p.DestinationValue.ToUpper().Replace(" ","") == employee.Location.ToUpper().Replace(" ", ""));
                    var departmentDestinations = destinations.Where(p => p.DestinationSource.ToUpper() == "DEPARTMENT" && p.DestinationValue.ToUpper().Replace(" ", "") == employee.Department.ToUpper().Replace(" ", ""));
                    var employeeCompensation = compensation.Where(p => p.EmployeeId == employee.EmployeeId).FirstOrDefault();

                    //If the employee has a destination or location mapping to My529, consolidate them to existing Bridge
                    if (locationDestinations.Where(p => p.Destination.ToUpper() == My529Destination).Count() > 0 || departmentDestinations.Where(p => p.Destination.ToUpper() == My529Destination).Count() > 0)
                    {
                        var result = await ReconcileEmployee(My529Client, employee, employeeCompensation, my529users, my529CustomFields, employees);
                    }

                    //If the employee has a destination or location mapping to My529, consolidate them to existing Bridge
                    if (locationDestinations.Where(p => p.Destination.ToUpper() == UheaaDestination).Count() > 0 || departmentDestinations.Where(p => p.Destination.ToUpper() == UheaaDestination).Count() > 0)
                    {
                        var result = await ReconcileEmployee(UheaaClient, employee, employeeCompensation, uheaaUsers, uheaaCustomFields, employees);
                    }
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process Bridge user directory. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task ProcessBlacklist(List<BlacklistRecord> employees)
        {
            try
            {
                //Get information, will probably be an enumerable
                string url = $"api/author/users?includes[]=manager&includes[]=custom_fields&limit={int.MaxValue.ToString()}";
                UserDirectoryRecord my529users = await GetAsync<UserDirectoryRecord>(My529Client, url);
                UserDirectoryRecord uheaaUsers = await GetAsync<UserDirectoryRecord>(UheaaClient, url);

                //await DeleteAllUsers(My529Client, my529users);
                foreach (var employee in employees)
                {
                    //If the employee has a destination or location mapping to My529, consolidate them to existing Bridge
                    if (employee.Destination.ToUpper() == My529Destination)
                    {
                        var result = await DeleteEmployeeIfExists(My529Client, employee, my529users);
                    }

                    //If the employee has a destination or location mapping to My529, consolidate them to existing Bridge
                    if (employee.Destination.ToUpper() == UheaaDestination)
                    {
                        var result = await DeleteEmployeeIfExists(UheaaClient, employee, uheaaUsers);
                    }
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message ?? "";
                LogRun.AddNotification($"Failed to process blacklist. Received internal exception {exceptionMessage}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

        }

        public async Task<bool> DeleteEmployeeIfExists(HttpClient client, BlacklistRecord employee, UserDirectoryRecord users)
        {
            var user = users.Users.Where(p => p.Uid == employee.UID).FirstOrDefault();
            if (user != null)
            {
                bool deleted = await DeleteUser(client, user);
                if (deleted)
                {
                    return true;
                }
                else
                {
                    LogRun.AddNotification($"Failed to delete blacklist user. Employee: {employee.UID}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> ReconcileEmployee(HttpClient client, EmployeeBridgeSSMS employee, CompensationSSMS compensation, UserDirectoryRecord users, CustomFieldListItemRecord customFields, List<EmployeeBridgeSSMS> employees)
        {
            //TESTING
            //users = await GetAsync<UserDirectoryRecord>(client, $"api/author/users?includes[]=manager&includes[]=custom_fields");
            //await DeleteUser(client, users.Users.First());
            //users = await GetAsync<UserDirectoryRecord>(client, $"api/author/users?includes[]=manager&includes[]=custom_fields");
            //

            var user = users.Users.Where(p => p.Uid == employee.EmployeeNumber).FirstOrDefault();

            //Delete the user on Bridge if the employee was terminated
            if (user == null && employee.DeletedAt.HasValue)
            {
                DA.UpdateEmployeeCompleted(employee.EmployeeId);
                LogRun.AddNotification($"Failed to delete user. User does not exist on Bridge. Employee: {employee.EmployeeId}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }
            else if (employee.DeletedAt.HasValue)
            {
                bool deleted = await DeleteUser(client, user);
                if(deleted)
                {
                    DA.UpdateEmployeeCompleted(employee.EmployeeId);
                    return true;
                }
                else
                {
                    LogRun.AddNotification($"Failed to delete user. Employee: {employee.EmployeeId}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false;
                }
            }

            //Create the user if they don't already exist, the user will still need to be update since the create API
            //does not allow for custom fields
            if (user == null && !employee.DeletedAt.HasValue)
            {
                UserAddRecord rec = new UserAddRecord();
                rec.Uid = employee.EmployeeNumber;
                rec.HrisId = employee.EmployeeNumber;
                rec.FirstName = employee.FirstName;
                rec.LastName = employee.LastName;
                rec.FullName = (employee.FirstName ?? "") + " " + (employee.LastName ?? "");//employee.EmployeeNumber
                rec.Location = employee.Location;
                rec.Department = employee.Department;
                rec.JobTitle = employee.JobTitle;
                rec.Email = employee.WorkEmail;


                UserPostUpdateRecord postRecord = new UserPostUpdateRecord();
                postRecord.Users = new List<UserAddRecord>() { rec };

                string url = "api/admin/users";
                var result = await PostAsync<UserPostUpdateRecord>(client, url, postRecord);
                if (!result)
                { 
                    //message was logged in PostAsync
                    return false;
                }
                //If succeeded we need to pull the updated directory, this can be done on an individual record basis, but the user size should be small
                users = await GetAsync<UserDirectoryRecord>(client, $"api/author/users?includes[]=manager&includes[]=custom_fields&limit={int.MaxValue.ToString()}");
            }

            //After creating a record, we update it so that custom fields can be added and a manager id linked
            return await UpdateUser(client, employee, compensation, users, customFields, employees);
            
        }

        public async Task<bool> UpdateUser(HttpClient client, EmployeeBridgeSSMS employee, CompensationSSMS compensation, UserDirectoryRecord users, CustomFieldListItemRecord customFields, List<EmployeeBridgeSSMS> employees)
        {
            var user = users.Users.Where(p => p.Uid == employee.EmployeeNumber).FirstOrDefault();

            //After creating a record, we update it so that custom fields can be added and a manager id linked
            UserUpdateRecord record = new UserUpdateRecord();
            record.Uid = employee.EmployeeNumber;
            record.HrisId = employee.EmployeeNumber;
            record.FirstName = employee.FirstName;
            record.LastName = employee.LastName;
            record.FullName = (employee.FirstName ?? "") + " " + (employee.LastName ?? "");//employee.EmployeeNumber
            record.Location = employee.Location;
            record.Department = employee.Department;
            record.JobTitle = employee.JobTitle;
            record.Email = employee.WorkEmail;

            var aliasManager = AliasDictionary.Values.Where(p => (p.PreferredName ?? "").ToUpper().Replace(" ", "") + (p.LastName ?? "").ToUpper().Replace(" ", "") == (employee.Supervisor ?? "").ToUpper().Replace(" ", "")).FirstOrDefault();
            var linkedManager = users.Users.Where(p => (p.FirstName ?? "").ToUpper().Replace(" ", "") + (p.LastName ?? "").ToUpper().Replace(" ", "") == (employee.Supervisor ?? "").ToUpper().Replace(" ", "")).FirstOrDefault();
            if(linkedManager == null && aliasManager != null)
            {
                string managerName = DA.GetEmployeeNameFromId(aliasManager.EmployeeId);
                if(managerName != null)
                {
                    linkedManager = users.Users.Where(p => (p.FirstName ?? "").ToUpper().Replace(" ", "") + (p.LastName ?? "").ToUpper().Replace(" ", "") == (managerName ?? "").ToUpper().Replace(" ", "")).FirstOrDefault();
                }    
            }
            
            if (linkedManager == null && !employee.Supervisor.IsNullOrEmpty())
            {
                LogRun.AddNotification($"Failed to link manager {employee.Supervisor} for employee {employee.FirstName} {employee.LastName}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            if(linkedManager != null && !employee.Supervisor.IsNullOrEmpty())
            {
                record.ManagerId = "uid:" + linkedManager.Uid;
            }

            //Add a new custom field onto the employee if adding a new employee record
            List<CustomField> fields = new List<CustomField>();
            var field = customFields.CustomFields.Where(p => p.Name.ToUpper() == "DIVISION").FirstOrDefault();
            if (field?.Name.ToUpper() == "DIVISION")
            {
                string existingFieldId = GetExistingFieldId(users, user, field);
                fields.Add(new CustomField() { Id = existingFieldId, CustomFieldId = field.Id, Value = employee.Division });
            }
            else
            {
                LogRun.AddNotification($"Unable to find custom field Division in list of custom fields on {client.BaseAddress}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            field = customFields.CustomFields.Where(p => p.Name.ToUpper() == "NEW HIRE").FirstOrDefault();
            if (field?.Name.ToUpper() == "NEW HIRE")
            {
                string existingFieldId = GetExistingFieldId(users, user, field);
                fields.Add(new CustomField() { Id = existingFieldId, CustomFieldId = field.Id, Value = employee.NewHire });
            }
            else
            {
                LogRun.AddNotification($"Unable to find custom field New Hire in list of custom fields on {client.BaseAddress}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            field = customFields.CustomFields.Where(p => p.Name.ToUpper() == "EMPLOYEE LEVEL").FirstOrDefault();
            if (field?.Name.ToUpper() == "EMPLOYEE LEVEL")
            {
                string existingFieldId = GetExistingFieldId(users, user, field);
                fields.Add(new CustomField() { Id = existingFieldId, CustomFieldId = field.Id, Value = employee.EmployeeLevel });
            }
            else
            {
                LogRun.AddNotification($"Unable to find custom field Employee Level in list of custom fields on {client.BaseAddress}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            field = customFields.CustomFields.Where(p => p.Name.ToUpper() == "DEPT ID").FirstOrDefault();
            if (field?.Name.ToUpper() == "DEPT ID")
            {
                string existingFieldId = GetExistingFieldId(users, user, field);
                fields.Add(new CustomField() { Id = existingFieldId, CustomFieldId = field.Id, Value = employee.DepartmentId });
            }
            else
            {
                LogRun.AddNotification($"Unable to find custom field Department Id in list of custom fields on {client.BaseAddress}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }


            if (compensation != null)
            {
                field = customFields.CustomFields.Where(p => p.Name.ToUpper() == "FLSA").FirstOrDefault();
                if (field.Name.ToUpper() == "FLSA")
                {
                    string existingFieldId = GetExistingFieldId(users, user, field);
                    fields.Add(new CustomField() {Id = existingFieldId, CustomFieldId = field.Id, Value = compensation.Exempt });
                }
                else
                {
                    LogRun.AddNotification($"Unable to find custom field Division in list of custom fields on {client.BaseAddress}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
            record.CustomFields = fields;

            UserPutUpdateRecord putRecord = new UserPutUpdateRecord();
            putRecord.User = record;

            string url = $"api/author/users/{user.Id}";
            var result = await PutAsync<UserPutUpdateRecord>(client, url, putRecord);
            if (result)
            {
                DA.UpdateEmployeeCompleted(employee.EmployeeId);
                if (compensation != null)
                {
                    DA.UpdateCompensationCompleted(employee.EmployeeId);
                }

                //If succeeded we need to pull the updated directory, this can be done on an individual record basis, but the user size should be small
                users = await GetAsync<UserDirectoryRecord>(client, $"api/author/users?includes[]=manager&includes[]=custom_fields&limit={int.MaxValue.ToString()}");
                return true;
            }
            return false;
        }

        public string GetExistingFieldId(UserDirectoryRecord users, UserRecord user, CustomFieldListItem field)
        {
            //the user was just created we can assume they have no custom fields
            if(user == null)
            {
                return null;
            }

            foreach (var existingField in user.Links?.CustomFieldValues)
            {
                foreach (var linkedField in users.Linked.CustomFieldValues.Where(p => p.Id == existingField))
                {
                    if (linkedField.Links.LinkCustomField.Id == field.Id)
                    {
                        return linkedField.Id;
                    }
                }
            }
            return null;
        }

        //I need this for testing since I am swapping primary keys
        public async Task DeleteAllUsers(HttpClient client, UserDirectoryRecord users)
        {
            foreach(var user in users.Users)
            {
                string url = $"/api/admin/users/{user.Id}";
                var result = await DeleteAsync(client, url);
            }
        }

        public async Task<bool> DeleteUser(HttpClient client, UserRecord user)
        {
            string url = $"/api/admin/users/{user.Id}";
            var result = await DeleteAsync(client, url);
            return result;
        }

        public async Task RunAsync()
        {
            //This query checks if there are people who are no longer new hires who need to be set to no longer be new hires on bridge
            DA.UpdateNewHires();
            var employeesToUpdate = DA.GetEmployees();
            //employeesToUpdate = employeesToUpdate.Where(p => p.FirstName.ToUpper() == "PAUL" && p.LastName.ToUpper() == "TEST").ToList();
            var compensationToUpdate = DA.GetCompensation();
            var uheaaCustomFields = await ProcessCustomFields(UheaaClient);
            var my529CustomFields = await ProcessCustomFields(UheaaClient);

            //Remove records in blacklist
            var blacklist = DA.GetBlacklist();
            await ProcessBlacklist(blacklist);

            //Get information, will probably be an enumerable
            await ProcessEmployeeTableData(employeesToUpdate, compensationToUpdate, uheaaCustomFields, my529CustomFields);
        }
    }
}
