using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;
using System.Xml;

namespace ECORRINERR
{
    public class ECORRINERR
    {
        public ProcessLogRun LogDataUheaa { get; set; }
        public ProcessLogRun LogDataCornerstone { get; set; }
        public string ScriptId { get { return "ECORRINERR"; } }
        static readonly object locker = new object();
        private DataAccess DA { get; set; }

        public ECORRINERR(DataAccessHelper.Mode mode)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = mode;
            LogDataUheaa = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            LogDataCornerstone = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true);
            DA = new DataAccess(LogDataUheaa, LogDataCornerstone);
        }

        /// <summary>
        /// Starting point of the application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "Ecorr Error XML Reader", false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            return new ECORRINERR(DataAccessHelper.CurrentMode).Process(args);
        }

        /// <summary>
        /// Start up process logger and locate files to process
        /// </summary>
        /// <returns></returns>
        private int Process(string[] args)
        {
            List<string> filesFed = new List<string>();
            List<string> filesUheaa = new List<string>();
            string fedSource = EnterpriseFileSystem.GetPath("ECORRINFED");
            string uheaaSource = EnterpriseFileSystem.GetPath("ECORRINUHEAA");
            foreach (string f in Directory.EnumerateFiles(fedSource).Where(x => x.Contains("KU_OUT_INPUT_ERR") && !x.EndsWith(".u0000")))
            {
                filesFed.Add(f);
            }

            foreach (string f in Directory.EnumerateFiles(uheaaSource).Where(x => x.Contains("UT_OUT_INPUT_ERR") && !x.EndsWith(".u0000")))
            {
                filesUheaa.Add(f);
            }

            int fedResult = 0;
            int uheaaResult = 0;

            List<Task> threads = new List<Task>();
            threads.Add(Task.Factory.StartNew(() => fedResult = ProcessFiles(filesFed, DataAccessHelper.Region.CornerStone, args)));
            threads.Add(Task.Factory.StartNew(() => uheaaResult = ProcessFiles(filesUheaa, DataAccessHelper.Region.Uheaa, args)));

            Task.WhenAll(threads).Wait();
            LogDataUheaa.LogEnd();
            LogDataCornerstone.LogEnd();
            return fedResult + uheaaResult;
        }

        /// <summary>
        /// Read XML from files and insert into database
        /// </summary>
        /// <param name="files">List of files to process</param>
        private int ProcessFiles(List<string> files, DataAccessHelper.Region region, string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            SqlConnection connEcorr;
            string directory;
            List<string> directoryList;
            string subDirectory = DateTime.Now.Year.ToString() + " - " + DateTime.Now.Month.ToString();

            foreach (string filename in files)
            {
                xmlDoc.Load(filename);

                if (region == DataAccessHelper.Region.CornerStone)
                    connEcorr = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.ECorrFed, DataAccessHelper.CurrentMode));
                else 
                    connEcorr = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.EcorrUheaa, DataAccessHelper.CurrentMode));


                List<ErrorData> records = ParseFile(xmlDoc, filename);
                connEcorr.Open();
                foreach(ErrorData record in records)
                {
                    DA.InsertErrorInformation(record, connEcorr);
                }
                connEcorr.Close();

                if (region == DataAccessHelper.Region.CornerStone)
                {
                    directory = EnterpriseFileSystem.GetPath("ECORRINFED");
                    directoryList = Directory.GetDirectories(directory).ToList();
                    if (!directoryList.Contains(subDirectory))
                        Directory.CreateDirectory(directory + subDirectory);

                    File.Move(filename, directory + subDirectory + "\\" + filename.Substring(filename.LastIndexOf("\\")));
                }
                else
                {
                    directory = EnterpriseFileSystem.GetPath("ECORRINUHEAA");
                    directoryList = Directory.GetDirectories(directory).ToList();
                    if (!directoryList.Contains(subDirectory))
                        Directory.CreateDirectory(directory + subDirectory);

                    File.Move(filename, directory + subDirectory + "\\" + filename.Substring(filename.LastIndexOf("\\")));
                }
            }
            return 0;
        }

        /// <summary>
        /// Read the nodes in the files and create ErrorData records
        /// </summary>
        /// <param name="xmlDoc">xml records from file</param>
        /// <param name="filename">file being processed</param>
        /// <returns></returns>
        private static List<ErrorData> ParseFile(XmlDocument xmlDoc, string filename)
        {
            XmlNodeList failedDocs = xmlDoc.GetElementsByTagName("path");
            XmlNodeList nameProperties = xmlDoc.GetElementsByTagName("name");
            XmlNodeList propertyValues = xmlDoc.GetElementsByTagName("value");
            XmlNodeList errorText = xmlDoc.GetElementsByTagName("ErrorText");
            List<KeyValuePair<string, string>> properties = new List<KeyValuePair<string, string>>();
            List<ErrorData> records = new List<ErrorData>();
            ErrorData newRecord = new ErrorData();
            foreach (XmlNode node in failedDocs)
            {
                for (int i = 0; i < nameProperties.Count; i++)
                {
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(nameProperties[i].InnerText, propertyValues[i].InnerText);
                    properties.Add(pair);
                }

                newRecord = CreateErrorDataRecord(properties, newRecord, node, filename, errorText);

                records.Add(newRecord);
            }
            return records;
        }

        private static ErrorData CreateErrorDataRecord(List<KeyValuePair<string, string>> properties, ErrorData newRecord, XmlNode node, string filename, XmlNodeList errorText)
        {
            newRecord.path = node.InnerText;
            newRecord.SSN = SetField(properties, "SSN");
            newRecord.DOC_DATE = SetField(properties, "DOC_DATE");
            newRecord.DOC_ID = SetField(properties, "DOC_ID");
            newRecord.ADDR_ACCT_NUM = SetField(properties, "ADDR_ACCT_NUM");
            newRecord.LETTER_ID = SetField(properties, "LETTER_ID");
            newRecord.REQUEST_USER = SetField(properties, "REQUEST_USER");
            newRecord.VIEWABLE = SetField(properties, "VIEWABLE");
            newRecord.CORR_METHOD = SetField(properties, "CORR_METHOD");
            newRecord.REPORT_DESC = SetField(properties, "REPORT_DESC");
            newRecord.LOAD_TIME = SetField(properties, "LOAD_TIME");
            newRecord.REPORT_NAME = SetField(properties, "REPORT_NAME");
            newRecord.ADDRESSEE_EMAIL = SetField(properties, "ADDRESSEE_EMAIL");
            newRecord.VIEWED = SetField(properties, "VIEWED");
            newRecord.CREATE_DATE = SetField(properties, "CREATE_DATE");
            newRecord.MAINFRAME_REGION = SetField(properties, "MAINFRAME_REGION");
            newRecord.DCN = SetField(properties, "DCN");
            newRecord.SUBJECT_LINE = SetField(properties, "SUBJECT_LINE");
            newRecord.DOC_SOURCE = SetField(properties, "DOC_SOURCE");
            newRecord.DOC_COMMENT = SetField(properties, "DOC_COMMENT");
            newRecord.WORKFLOW = SetField(properties, "WORKFLOW");
            newRecord.DOC_DELETE = SetField(properties, "DOC_DELETE");
            newRecord.Region = SetField(properties, "Region");
            newRecord.ErrorText = errorText[0].InnerText;
            newRecord.ErrorFileName = filename;
            return newRecord;
        }

        private static string SetField(List<KeyValuePair<string, string>> properties, string propertyName)
        {
            return properties.Where(x => x.Key == propertyName).Select(y => y.Value).FirstOrDefault();
        }
    }
}