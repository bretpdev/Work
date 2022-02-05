using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AesApiTester
{
    public partial class MainForm : Form
    {
        Properties.Settings settings = Properties.Settings.Default;
        string CertLocation
        {
            get
            {
                var result = Assembly.GetExecutingAssembly().Location;
                result = Path.GetDirectoryName(result);
                result = Path.Combine(result, "cer-webservices.qa.aessuccess.org.crt");
                return result;
            }
        }
        public MainForm()
        {
            InitializeComponent();

            UrlBox.Text = settings.Url;
            PayloadBox.Text = settings.Payload;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback += (o, a, b, c) => true;
        }

        private void SendRequestButton_Click(object sender, EventArgs e)
        {
            ResponseBox.Text = AesRequest(settings.Url);
        }

        private string AesRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.ClientCertificates.Add(new System.Security.Cryptography.X509Certificates.X509Certificate(CertLocation));
            using (var response = webRequest.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void UrlBox_TextChanged(object sender, EventArgs e)
        {
            settings.Url = UrlBox.Text;
            settings.Save();
        }

        private void PayloadBox_TextChanged(object sender, EventArgs e)
        {
            settings.Payload = PayloadBox.Text;
            settings.Save();
        }
    }
}
