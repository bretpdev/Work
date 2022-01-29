using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EmployeeHistory
{
    class XmlParser
    {
        public string LastSynchronizationToken = "";
        public string[] ParseUserIds(string userIdXml)
        {
            List<string> results = new List<string>();
            var doc = XDocument.Parse(userIdXml);
            foreach (var user in doc.Root.Element("Users").Elements("User"))
            {
                results.Add(user.Element("LoginID").Value);
                LastSynchronizationToken = user.Element("SynchronizationToken").Value;
            }
            return results.ToArray();
        }
    }
}
