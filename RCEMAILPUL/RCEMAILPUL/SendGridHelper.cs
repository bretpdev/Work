using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace RCEMAILPUL
{
    class SendGridHelper
    {
        public string ApiKey { get; set; }
        public SendGridHelper()
        {
            ApiKey = EnterpriseFileSystem.GetPath("SendGridKey");
        }
        public List<SendGridMessage> GetSendGridHistory()
        {
            var dateAfter = DateTime.Now.AddHours(-36); //look at all messages in the last 36 hours
            string url = "/messages?limit=2147483647&query=last_event_time" + Uri.EscapeUriString($"> TIMESTAMP\"{dateAfter:yyyy-MM-dd}T00:00:00Z\"");

            var messages = GetApiCall<MessageHolder>(url).messages;
            foreach (var message in messages)
                message.last_event_time = message.last_event_time.ToLocalTime();
            return messages;
        }

        public List<string> GetGlobalUnsubscribes()
        {
            string url = "/suppression/unsubscribes";
            var emails = GetApiCall<UnsubscribedEmail[]>(url);
            return emails.Select(o => o.email.ToLower()).ToList();
        }

        private T GetApiCall<T>(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
                throw new Exception("relativeUrl must start with /");
            var dateAfter = DateTime.Now.AddHours(-36); //look at all messages in the last 36 hours
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("authorization", "Bearer " + ApiKey);
            string url = "https://api.sendgrid.com/v3" + relativeUrl;

            var response = WaitFor(client.GetAsync(url));
            var content = WaitFor(response.Content.ReadAsStringAsync());
            var json = JsonConvert.DeserializeObject<T>(content);
            return json;
        }

        private T WaitFor<T>(Task<T> t)
        {
            t.Wait();
            return t.Result;
        }
    }
}