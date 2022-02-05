using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace HRBRIDGE
{
    abstract class ProcessorBase
    {
        protected ProcessLogRun LogRun { get; set; }
        protected DataAccess DA { get; set; }

        public ProcessorBase(ProcessLogRun logRun)
        {
            //Set up logging
            LogRun = logRun;
            DA = new DataAccess(logRun);
        }

        protected string ToISO8601DateString(DateTime? date)
        {
            return date?.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public async Task<T> GetAsync<T>(HttpClient client, string path)
        {
            T value = default(T);
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                string stringValue = await response.Content.ReadAsStringAsync();
                var jsonSerializerSetting = new JsonSerializerSettings();
                jsonSerializerSetting.MissingMemberHandling = MissingMemberHandling.Ignore;
                jsonSerializerSetting.NullValueHandling = NullValueHandling.Ignore;
                value = JsonConvert.DeserializeObject<T>(stringValue, jsonSerializerSetting);
            }
            else
            {
                LogRun.AddNotification($"Response from {client.BaseAddress.ToString()}/{path} server not success. Status code: {response.StatusCode} Reason: {response.ReasonPhrase}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return value;
        }

        public async Task<bool> PostAsync<T>(HttpClient client, string path, T value)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(path, content);

            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                LogRun.AddNotification($"Response from {client.BaseAddress.ToString()}/{path} server not success. Status code: {response.StatusCode} Reason: {response.ReasonPhrase}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }

        //Returns updated object
        public async Task<bool> PutAsync<T>(HttpClient client, string path, T value)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            var stringContent = await content.ReadAsStringAsync();
            HttpResponseMessage response = await client.PutAsync(path, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
                //string resultString = await response.Content.ReadAsStringAsync();
                //Type type = value is UserPutUpdateRecord || value is UserPostUpdateRecord ? typeof(UserDirectoryRecord) : value.GetType();
                //T resultValue = JsonConvert.DeserializeObject<type.BaseType>(resultString);
                //return resultValue;
            }
            else
            {
                LogRun.AddNotification($"Response from {client.BaseAddress.ToString()}/{path} server not success. Status code: {response.StatusCode} Reason: {response.ReasonPhrase}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(HttpClient client, string path)
        {
            HttpResponseMessage response = await client.DeleteAsync(path);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                LogRun.AddNotification($"Response from {client.BaseAddress.ToString()}/{path} server not success. Status code: {response.StatusCode} Reason: {response.ReasonPhrase}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
        }

    }
}