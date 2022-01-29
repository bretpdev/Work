using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ISAPIALIVE
{
    public class ProcessAPI
    {
        public  string Url = "https://webuheaaapi.uheaa.org/Borrower/GetSsn";
        public const string UrlParameters = "";
        public List<string> Reasons = new List<string>() { "OK" };

        public ProcessLogRun LogRun { get; set; }

        public int Process(string accountNumber)
        {
            LogRun = new ProcessLogRun("ISAPIALIVE", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            Url = DataAccessHelper.TestMode ? "https://webuheaaapidev.uheaa.org/Borrower/GetSsn" : "https://webuheaaapi.uheaa.org/Borrower/GetSsn";
            int returnType = CallAPI(accountNumber);
            LogRun.LogEnd();
            return returnType;
        }

        private int CallAPI(string accountNumber)
        {
            HttpClient client = new HttpClient();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string key = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev ? "C398B9E3-2161-4337-864A-47E86948A9CF" : "FF5F96B1-124D-48DD-B72F-DA49C3400864";
                
                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("accountnumber", accountNumber));
                formValues.Add(new KeyValuePair<string, string>("apikey", key));
                formValues.Add(new KeyValuePair<string, string>("region", DataAccessHelper.CurrentRegion.ToString().ToLower()));

                var response = client.PostAsync(Url, new FormUrlEncodedContent(formValues)).Result;
                string res = response.Content.ReadAsStringAsync().Result;

                if (!Reasons.Contains(response.StatusCode.ToString()))
                    throw new Exception("There was a return reason that was not recognized.");

                Console.WriteLine($"The server is alive and returned status code: {response.StatusCode}");
                client.Dispose();
                
                return 0;
            }
            catch (Exception ex)
            {
                string message = "There was an error calling the GetSsn method from the API";
                Console.WriteLine(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                client.Dispose();
                return 1;
            }
        }
    }
}