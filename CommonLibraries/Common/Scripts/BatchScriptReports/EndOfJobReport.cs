using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace Uheaa.Common.Scripts
{
	public class EndOfJobReport
	{
		private readonly string ReportName;
		private readonly string DataFile;
		private readonly string PublicationDirectory;

		private Dictionary<string, Count> EojCounts;
		public Dictionary<string, Count> Counts
		{
			get
			{
				return EojCounts;
			}
		}

        public Count this[string index]
        {
            get
            {
                return Counts[index];
            }
            set
            {
                Counts[index] = value;
            }
        }

        public EndOfJobReport(string reportName, string fileSystemKey, IEnumerable<string> headers, string userId = "")
		{
			EojCounts = new Dictionary<string, Count>();

            DataFile = userId.IsNullOrEmpty() ? string.Format("{0}EOJ_{1}.txt", EnterpriseFileSystem.TempFolder, reportName) : string.Format("{0}EOJ_{1}_{2}.txt", EnterpriseFileSystem.TempFolder, reportName ,userId);
            PublicationDirectory = EnterpriseFileSystem.GetPath(fileSystemKey);
			ReportName = reportName;

			if (File.Exists(DataFile))
			{
				List<string> recoveryItems = new List<string>();
				using (StreamReader sr = new StreamR(DataFile))
				{
					while (!sr.EndOfStream)
					{
						recoveryItems.Add(sr.ReadLine());
					}
				}


				int i = 0;
				foreach (string item in recoveryItems[0].SplitAndRemoveQuotes(",", false))
				{
                    int recoveryValue = int.Parse(recoveryItems[1].SplitAndRemoveQuotes(",", false)[i]);
					Count recoveryCount = new Count(recoveryValue);
					i++;
					recoveryCount.NewValue += new Count.ValueChanged(UpdateDataFile);
					EojCounts.Add(item, recoveryCount);
				}

			}
			else
			{
				foreach (string header in headers)
				{
					Count newCount = new Count(0);
					newCount.NewValue += new Count.ValueChanged(UpdateDataFile);
					EojCounts.Add(header, newCount);
				}
			}

		}

		public void UpdateDataFile(Count c)
		{
			using (StreamWriter sw = new StreamW(DataFile, false))
			{
				sw.WriteLine(string.Join(",", EojCounts.Keys.ToArray()));
				sw.WriteLine(string.Join(",", EojCounts.Values.Select(p => p.ToString()).ToArray()));
			}
		}

        public void PublishProcessLogger(ProcessLogData logData)
        {
            foreach (KeyValuePair<string, Count> item in Counts)
            {
                var parameters = new
                {
                    ProcessLogId = logData.ProcessLogId,
                    ResultHeader = item.Key,
                    ResultsValue = item.Value.Value
                };

                DataAccessHelper.Execute("LogEndOfJobResults", DataAccessHelper.Database.ProcessLogs, parameters.SqlParameters());
            }
        }

        public void Delete()
        {
            if (!File.Exists(DataFile))
                return;
            else
                FS.Delete(DataFile);
        }

		/// <summary>
		/// Produces an HTML file based on the counts, and saves it to the directory indicated by the filesystem key specified in the constructor.
		/// </summary>
		public void Publish()
		{
			if (!File.Exists(DataFile)) { return; }

			List<string> countTable = new List<string>();
			countTable.Add("Item,Count");

			foreach (KeyValuePair<string, Count> item in Counts)
			{
				countTable.Add(string.Format("{0},{1}", item.Key, item.Value.ToString()));
			}

			int reportNumber = 1;

			string htmlFile = string.Format("{0}{1} End of Job Report {2:MM-dd-yyyy HH.mm}.html", PublicationDirectory, ReportName, DateTime.Now);

			//In case this method gets called more than once per application, use a new file name each time.
			//Slap a counter on the end if needed (i.e., Publish() gets called more than once in a given minute).
			while (File.Exists(htmlFile))
			{
				string oldExtension = reportNumber == 1 ? ".html" : string.Format(".{0}.html", reportNumber);
				reportNumber++;
				string newExtension = string.Format(".{0}.html", reportNumber);
				htmlFile = htmlFile.Replace(oldExtension, newExtension);
			}

			using (StreamWriter sw = new StreamW(htmlFile))
			{
				//Start the HTML file and write out the report headers.
				sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<style type='text/css'>");
                sw.WriteLine("body{font-family:Arial,Helvetica,sans-serif;}table{border-collapse:collapse;}td,th{padding:2px 10px;}tr.oddrow{background-color:#EEE;}h1{font-size:20px;}h2{font-size:16px;}");
                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
				sw.WriteLine("<h1>" + ReportName + "</h1>");
				sw.WriteLine("<h2>End of Job Report</h2>");
				sw.WriteLine("<h2>" + htmlFile + "</h2>");
				//Get HTML from the DataTable and write it out.
				foreach (string item in countTable.ToHtmlLines("    "))
				{
					sw.WriteLine(item);
				}
				//Close out the HTML.
				sw.WriteLine("</body>");
				sw.WriteLine("</html>");
			}

			//Clean up the data file once it's been published, and re-initialize the counts.
			FS.Delete(DataFile);
			List<string> headers = new List<string>();

			foreach (string header in EojCounts.Keys)
			{
				headers.Add(header);
			}

			EojCounts = new Dictionary<string, Count>();
			foreach (string header in headers)
			{
				Count newCount = new Count(0);
				newCount.NewValue += new Count.ValueChanged(UpdateDataFile);
				EojCounts.Add(header, newCount);
			}
		}
	}
}
