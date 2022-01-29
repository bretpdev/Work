using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace Uheaa.Common.WebApi
{
    public class WebApiDataAccess
    {
        readonly string Url;
        readonly string AuthenticateUrl;
        private string UserToken;
        Action onConnectionError;
        public WebApiDataAccess(DataAccessHelper.Mode mode, string token = null, Action onConnectionError = null)
        {
            this.onConnectionError = onConnectionError;
            if (mode == DataAccessHelper.Mode.Live)
            {
                Url = "https://webuheaaapi.uheaa.org/";
                AuthenticateUrl = "https://access.webuheaaapp.uheaa.org";
            }
            else
            {
                Url = "https://webuheaaapidev.uheaa.org/";
                AuthenticateUrl = "https://access.webuheaaappdev.uheaa.org";
            }
            UserToken = token;
            if (UserToken == null)
                Authenticate();
        }
        public WebDataResult<bool> Execute(string commandName, DB db, params SqlParameter[] parameters)
        {
            var dtResult = ExecuteDataTable(commandName, db, parameters);
            var result = new WebDataResult<bool>(dtResult);
            result.Result = dtResult.DatabaseCallSuccessful;
            return result;
        }

        public WebDataResult<T> ExecuteSingle<T>(string commandName, DB db, params SqlParameter[] parameters)
        {
            var listResult = ExecuteList<T>(commandName, db, parameters);
            var newResult = new WebDataResult<T>(listResult);
            if (listResult.Result != null)
                newResult.Result = listResult.Result.SingleOrDefault();
            return newResult;
        }

        public WebDataResult<DataSet> ExecuteDataTable(string commandName, DB db, params SqlParameter[] parameters)
        {
            var managedResult = new WebDataResult<DataSet>() { DatabaseCallSuccessful = true };
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls; //enable HTTPS
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpClient httpClient = new HttpClient();


                var compiledUrl = Url + db.ToString() + "/Execute";
                var formValues = new List<KeyValuePair<string, string>>();

                formValues.Add(new KeyValuePair<string, string>("storedprocedurename", commandName));
                formValues.Add(new KeyValuePair<string, string>("apikey", UserToken));
                foreach (var parameter in parameters)
                    if (parameter.Value != null)
                        formValues.Add(new KeyValuePair<string, string>(parameter.ParameterName, parameter.Value.ToString()));


                var response = httpClient.PostAsync(compiledUrl, new FormUrlEncodedContent(formValues));
                response.Wait();
                if (response.IsCompleted)
                {
                    managedResult.DatabaseConnectionSuccessful = true;
                    var dtString = response.Result.Content.ReadAsStringAsync();

                    dtString.Wait();
                    var dt = JsonConvert.DeserializeObject<WebApiResult>(dtString.Result);
                    managedResult.Result = dt.Result;
                    managedResult.ExceptionText = dt.ExceptionText;
                    if (response.Result.IsSuccessStatusCode)
                        managedResult.DatabaseCallSuccessful = true;
                }
                else
                {
                    managedResult.DatabaseConnectionSuccessful = false;
                    managedResult.DatabaseCallSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                managedResult.DatabaseCallSuccessful = false;
                if (managedResult.ExceptionText == null)
                    managedResult.ExceptionText = ex.ToString();
                //string args = string.Join(";", parameters.Select(o => o.ParameterName + ":" + (o.Value ?? "").ToString()));
                //string message = string.Format("Error attempting to execute sproc {0} on database {1} in mode {2} with these arguments: {3}", commandName, db, PLR.Mode, args);
                //PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            finally
            {
                if (!managedResult.DatabaseConnectionSuccessful)
                {
                    onConnectionError?.Invoke();
                }
            }
            return managedResult;
        }


        public WebDataResult<List<T>> ExecuteList<T>(string commandName, DB db, params SqlParameter[] parameters)
        {
            var dt = ExecuteDataTable(commandName, db, parameters);
            var result = new WebDataResult<List<T>>(dt);
            if (!result.DatabaseCallSuccessful || !result.DatabaseConnectionSuccessful)
                return result;

            result.ExceptionText = dt.ExceptionText;
            if (dt.Result != null)
            {
                var table = dt.Result.Tables[0];
                var type = typeof(T);
                var constructor = type.GetConstructor(Type.EmptyTypes); //default constructor
                List<T> results = new List<T>();
                bool isSimpleCast = false;
                if (constructor == null) //no default constructor
                    isSimpleCast = true;
                else if (type == typeof(object)) //they just want a base object type, return it
                    isSimpleCast = true;
                var columnNames = table.Columns.Cast<DataColumn>().Select(o => o.ColumnName);
                List<PropertyInfo> columnProperties = new List<PropertyInfo>();
                if (!isSimpleCast)
                    columnProperties = columnNames.Select(o => typeof(T).GetProperty(o)).ToList();

                foreach (DataRow row in table.Rows)
                {
                    var items = row.ItemArray;
                    if (isSimpleCast)
                        results.Add((T)Convert.ChangeType(items[0], typeof(T)));
                    else
                    {
                        T obj = (T)constructor.Invoke(new object[] { });
                        for (int i = 0; i < items.Length; i++)
                        {
                            var val = items[i];
                            if (val is DBNull)
                                val = null;
                            var prop = columnProperties[i];
                            prop?.SetValue(obj, ConvertCustom(val, prop.PropertyType));
                        }
                        results.Add(obj);
                    }
                }
                result.DatabaseCallSuccessful = true;
                result.Result = results;
            }
            return result;
        }

        private void Authenticate()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls; //enable HTTPS
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            var wic = wi.Impersonate();
            try
            {
                using (var client = new WebClient { UseDefaultCredentials = true })
                    UserToken = client.DownloadString(AuthenticateUrl + "/Authenticate");
            }
            finally
            {
                wic.Undo();
            }
        }

        /// <summary>
        /// Convert specified value to a given type.  Special string parsing includes:
        /// Money with dollar signs converted to decimal ("$5.23" -> 5.23m)
        /// Y/N converted to bool ("y" -> true)
        /// 1/0 converted to bool ("0" -> false)
        /// String dates parsed to dates ("1/1/2015" -> new Date(1, 1, 2015))
        /// </summary>
        private static object ConvertCustom(object value, Type type)
        {
            if (value is DBNull)
                if (type == typeof(DateTime))
                    throw new Exception("Can't convert DBNull to non-nullable DateTime");
                else
                    return null;
            if (value is int && type.IsEnum)
                return Enum.ToObject(type, value);
            if (Nullable.GetUnderlyingType(type) != null)//working with a nullable)
            {
                if (value is string)
                    if (string.IsNullOrEmpty((string)value))
                        return null;
                type = Nullable.GetUnderlyingType(type);
            }
            if (type == typeof(bool))
                if (value is string)
                {
                    //returns 0/1 and Y/N as bools
                    string sval = (string)value;
                    sval = sval.ToLower();
                    if (sval == "y" || sval == "yes" || sval == "1") return true;
                    if (sval == "n" || sval == "no" || sval == "0") return false;
                }
            if (type == typeof(DateTime))
                if (value is string)
                {
                    DateTime parse = DateTime.Now;
                    if (DateTime.TryParse((string)value, out parse))
                        return parse;
                }
            if (type == typeof(decimal))
                if (value is string)
                {
                    string sval = (string)value;
                    sval = sval.Replace("$", "");
                    decimal parse = 0;
                    if (decimal.TryParse(sval, out parse))
                        return parse;
                }

            return Convert.ChangeType(value, type);
        }
    }
}
