using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmailTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string email = Environment.UserName + "@utahsbr.edu";
            string templateFile = Assembly.GetExecutingAssembly().Location;
            for (int i = 0; i < 5; i++)
                templateFile = Path.GetDirectoryName(templateFile);
            templateFile = Path.Combine(templateFile, "uheaa_template.htm");
            Uheaa.Common.EmailHelper.SendMail
            (
                testMode: true,
                to: email,
                from: email,
                subject: "Test Email Template",
                body: File.ReadAllText(templateFile),
                cc: "",
                attachmentFilePath: "",
                importance: Uheaa.Common.EmailHelper.EmailImportance.Normal,
                sendAsHtml: true
            );
        }
    }
}
