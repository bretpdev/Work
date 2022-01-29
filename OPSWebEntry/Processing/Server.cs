using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Uheaa.Common;

namespace OPSWebEntry
{
    /// <summary>
    /// Represents the AES Web Server
    /// </summary>
    public class Server
    {
        private const string TestLoginDomain = "https://partners.uat.uheaa.comservicing.org";
        private const string LiveLoginDomain = "https://partners.uheaa.org";
        private string LoginDomain { get { return ModeHelper.IsTest ? TestLoginDomain : LiveLoginDomain; } }

        private const string TestDomain = "https://account.uat.uheaa.comservicing.org";
        private const string LiveDomain = "https://myaccount.uheaa.org";
        private string Domain { get { return ModeHelper.IsTest ? TestDomain : LiveDomain; } }

        private string LoginUrl { get { return LoginDomain + "/B2BAuth/login.htm"; } }
        private string ServiceUrl { get { return Domain + "/accountAccess/index.cfm?event=common.inhouselogin"; } }
        private string LoadAccountUrl { get { return Domain + "/accountAccess/index.cfm?event=common.inhouseLoginCheck"; } }
        private string InitPaymentUrl { get { return Domain + "/accountAccess/index.cfm?event=payments.makePayment"; } }
        private string MakePaymentUrl { get { return Domain + "/payments/processMakePayment.do"; } }

        private CookieContainer cookies = new CookieContainer();
        public List<SubmitResults> ResultLog = new List<SubmitResults>();


        private const string AES_NEKOTCNYS = "AES_NEKOTCNYS";
        private string aes_nekotcnys = null;
        public Server()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public bool Login(string username, string password)
        {
            var info = new { txtUserId = username, txtPassword = password, Submit = "Sign+In", resourceID = "UTCPTLP" }; //yep, submit is required
            SubmitResults response = Submit(LoginUrl, info);
            string serviceFind = "name=\"serviceID\" value=\"";
            int serviceIdPos = response.Response.IndexOf(serviceFind);
            if (serviceIdPos == -1)
            {
                return false;
            }
            serviceIdPos += serviceFind.Length;
            string serviceId = response.Response.Substring(serviceIdPos, 36);
            Submit(ServiceUrl, new { serviceID = serviceId });
            return true;
        }

        public bool LoadAccount(string ssn, DateTime dob)
        {
            ResultLog.Clear();
            SubmitResults response = Submit(LoadAccountUrl, new { ssn = ssn, dateOfBirth = dob.ToString("MM/dd/yyyy"), submit = "Sign+In" });
            if (response.Response == null || response.Response.Contains("Welcome,")) //welcome message means successful login
                return true;
            return false;

        }

        public bool PostPayment(decimal amount, DateTime date, string routingNumber, string accountNumber, AccountType type, string primaryOwner)
        {
            Submit(InitPaymentUrl);
            //TODO: Some of these fields may be unnecessary.
            Submit(MakePaymentUrl, new
            {
                txtPaymentAmount = amount,
                txtPaymentDate = date.ToString("MM/dd/yyyy"),
                accountInfoViewBean = new
                {
                    txtRoutingNumber = routingNumber,
                    txtAccountNumber = accountNumber,
                    selAccountType = type == AccountType.Checking ? "C" : "S",
                    txtPrimaryOwner = primaryOwner,
                    txtAccountName = "" //we don't want to make a named account
                },
                makePayment = "Make+a+Payment",
                hasDualRegions = "false",
                payAll = "true",
                lastPayment = "",
                lastPaymentDate = "",
                hasSavedAccount = "false"
            });

            var result = Submit(MakePaymentUrl, new
            {
                payAll = "true",
                chkAgree = "true",
                _chkAgree = "on",
                verifySubmitPayment = "Verify+%26+Submit+Payment",
                lastPayment = "",
                lastPaymentDate = "",
                hasSavedAccount = "false",
                txtPaymentAmount = amount,
                txtPaymentDate = date.ToString("MM/dd/yyyy"),
                selSavedBankAccount = "",
                radBankAccount = "",
                rdPaymentGroup = "",
                accountInfoViewBean = new
                {
                    txtRoutingNumber = routingNumber,
                    txtAccountNumber = accountNumber,
                    selAccountType = type == AccountType.Checking ? "C" : "S",
                    txtPrimaryOwner = primaryOwner,
                    txtAccountName = "" //we don't want to make a named account
                },
                hasDualRegions = "false",
            });
            if (result.Response == null || result.Response.Contains("an unexpected error occurred"))
                return false;
            return true;
        }

        private string GetAESToken(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            using (StringReader sr = new StringReader(html))
                doc.Load(sr);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//input[@name='AES_NEKOTCNYS']");
            if (node == null) return null;
            return node.Attributes["value"].Value;
        }

        private SubmitResults Submit(string url, object formData, string method = "POST")
        {
            List<string> pairs = new List<string>();
            if (formData != null)
                pairs = ConvertToKeyValuePairs(formData);
            if (!string.IsNullOrEmpty(aes_nekotcnys))
                pairs.Add(AES_NEKOTCNYS + "=" + aes_nekotcnys);

            return Submit(url, string.Join("&", pairs.ToArray()), method);
        }

        private List<string> ConvertToKeyValuePairs(object formData, string prepend = null)
        {
            List<string> pairs = new List<string>();
            foreach (PropertyInfo pi in formData.GetType().GetProperties())
                if (pi.PropertyType.IsAnonymous())
                    pairs.AddRange(ConvertToKeyValuePairs(pi.GetValue(formData, null), pi.Name));
                else
                    pairs.Add((prepend != null ? prepend + "." : "") + pi.Name + "=" + pi.GetValue(formData, null));
            return pairs;
        }

        private SubmitResults Submit(string url)
        {
            return Submit(url, null, "GET");
        }

        string refererer = string.Empty;
        private SubmitResults Submit(string url, string formData, string method = "POST")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.CookieContainer = cookies;
            request.KeepAlive = true;
            var sp = request.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);
            request.Accept = "text/html, application/xhtml+xml, */*";
            if (!string.IsNullOrEmpty(refererer))
                request.Referer = refererer;
            request.Headers["Accept-Language"] = "en-US";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ProtocolVersion = new Version("1.1");
            request.ContentLength = (formData ?? "").Length;
            if (method == "POST")
            {
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers["Cache-Control"] = "no-cache";

                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    sw.Write(formData);
            }
            refererer = url;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string html = sr.ReadToEnd();
                    string token = GetAESToken(html);
                    if (!string.IsNullOrEmpty(token))
                        aes_nekotcnys = token;
                    SubmitResults results = new SubmitResults(html, response.ResponseUri, url, formData);
                    ResultLog.Add(results);
                    return results;
                }
            }
            catch (WebException ex)
            {
                SubmitResults results = new SubmitResults(null, null, url, formData, ex);
                ResultLog.Add(results);
                return results;
            }
        }
        public struct SubmitResults
        {
            public string Response { get; set; }
            public Uri ResponseUri { get; set; }
            public string RequestUrl { get; set; }
            public string FormData { get; set; }
            public Exception Exception { get; set; }
            public SubmitResults(string response, Uri responseUri, string requestUrl, string formData, Exception ex = null)
                : this()
            {
                Response = response;
                ResponseUri = responseUri;
                FormData = formData;
                RequestUrl = requestUrl;
                Exception = ex;
            }
        }
        public enum AccountType
        {
            Checking,
            Saving
        }
    }
}
