using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO.Compression;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using System.Threading;

namespace XmlGeneratorECorr
{
    class XmlGenerator
    {
        private XmlTextWriter Writer { get; set; }
        private static object queueLock = new object();
		private static ProcessLogData LogData;
        public static SqlConnection EcorrFedCon { get; set; }

        /// <summary>
        /// Generates an XML file for all unprocessed records.
        /// </summary>
        public static void GenerateXml(ProcessLogData logData, bool onlyRunBilling)
        {
			LogData = logData;
            EcorrFedCon = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.ECorrFed, DataAccessHelper.CurrentMode));
            EcorrFedCon.Open();
            using (XmlProgressBar xml = new XmlProgressBar(onlyRunBilling))
                xml.ShowDialog();

            EcorrFedCon.Close();
        }

        public void WriteXmlDoc(DocumentProperties document, string dir, string zipFileName)
        {
            string xmlFile = string.Format("INDX_{0}_{1}.xml", Guid.NewGuid(), DataAccessHelper.CurrentRegion);
            Writer = new XmlTextWriter(Path.Combine(dir, xmlFile), Encoding.UTF8);
            Writer.Formatting = Formatting.Indented;

            Writer.WriteStartDocument(); // <?xml version="1.0" encoding="utf-8"?>
            Writer.WriteStartElement("InputRequest");
            WriteAttributesAndValues("InputRequest");
            Writer.WriteStartElement("RoutingInformation");
            WriteRoutingInformation("RoutingInformation");
            Writer.WriteStartElement("Environment");
            Writer.WriteString(DataAccessHelper.TestMode ? DataAccessHelper.CurrentMode.ToString().ToUpper() : "PROD");
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteStartElement("Document");
            WriteAttributesAndValues("Document");
            Writer.WriteStartElement("DocumentStorageInformation");
            Writer.WriteStartElement("ObjectStore");
            Writer.WriteString("CORNERSTONE");
            Writer.WriteEndElement();
            Writer.WriteStartElement("DocumentLibrary");
            Writer.WriteString("CORNERSTONEeCorr");
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteStartElement("DocumentLocation");
            Writer.WriteStartElement("path");
            Writer.WriteString(string.Format(document.PATH, Path.GetFileNameWithoutExtension(zipFileName)));
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            WriteDetailsForEcorrXmlFile(document);
            Writer.WriteEndElement();

            string file = Path.GetFileName(document.PATH);
			try
			{
                string toFile = Path.Combine(dir, file);
				File.Copy(Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), file), toFile, true);
                while (!File.Exists(toFile))
                    Thread.Sleep(1000);
				Writer.WriteEndDocument();
				Writer.Close();
			}
			catch (FileNotFoundException ex)
			{
				ProcessLogger.AddNotification(LogData.ProcessLogId, string.Format("Unable to move file {0}.  File does not exist", Path.Combine(EnterpriseFileSystem.GetPath("ECORRLocation"), file).ToString()), NotificationType.NoFile, NotificationSeverityType.Critical, LogData.ExecutingAssembly, ex);
			}   
        }

        /// <summary>
        /// Writes the document properties for the XML file.
        /// </summary>
        /// <param name="docData">Object with all of the document properties</param>
        private void WriteDetailsForEcorrXmlFile(DocumentProperties docData)
        {
            Writer.WriteStartElement("DocumentProperties");
            foreach (PropertyInfo item in docData.GetType().GetProperties().Skip(2)) //Skipping the first 2 properties as one is used before this loop and the other is an ID column.
            {
                var value = docData.GetType().GetProperty(item.Name).GetValue(docData, null);
                if (value != null)
                {
                    Writer.WriteStartElement("DocumentProperty");
                    Writer.WriteStartElement("name");
                    Writer.WriteString(item.Name);
                    Writer.WriteEndElement();
                    Writer.WriteStartElement("value");
                    if (docData.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime) || docData.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime?))
                    {
                        DateTime? temp = (DateTime)docData.GetType().GetProperty(item.Name).GetValue(docData, null);
                        if (temp.HasValue)
                        {
                            string tempDate = temp.Value.ToString("MM/dd/yyyy");
                            Writer.WriteString(tempDate.ToDate().AddHours(2).ToString("MM/dd/yyyy hh:mm:ss") + " AM");
                        }
                    }
                    else
                        Writer.WriteString(docData.GetType().GetProperty(item.Name).GetValue(docData, null).ToString());

                    Writer.WriteEndElement();
                    Writer.WriteEndElement();
                }
            }

            Writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the attributes for the XML file
        /// </summary>
        /// <param name="tag"></param>
        private void WriteAttributesAndValues(string tag)
        {
            lock (queueLock)
            {
                List<EcorrXmlAttributeValues> attrValues = DataAccessHelper.ExecuteList<EcorrXmlAttributeValues>("GetAttributesAndValues", EcorrFedCon , new SqlParameter("Tag", tag));
                foreach (EcorrXmlAttributeValues item in attrValues)
                {
                    
                    Writer.WriteAttributeString(item.Attribute, item.Value);
                }
            }
        }

        private void WriteRoutingInformation(string tag)
        {
            lock (queueLock)
            {
                List<EcorrXmlAttributeValues> attrValues = DataAccessHelper.ExecuteList<EcorrXmlAttributeValues>("GetAttributesAndValues", DataAccessHelper.Database.ECorrFed, new SqlParameter("Tag", tag));
                foreach (EcorrXmlAttributeValues item in attrValues)
                {
                    //This tag requires a little bit of formatting so that the value is unique and has a value indicating if this is a test.
                    if (item.Attribute == "clientUUID")
                        item.Value = string.Format(item.Value, Guid.NewGuid().ToBase64String().Substring(1), DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? "TEST" : "");

                    Writer.WriteStartElement(item.Attribute);
                    Writer.WriteString(item.Value);
                    Writer.WriteEndElement();
                }
            }
        }

    }
}
