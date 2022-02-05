using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Collections;
using System.Threading;


namespace NelnetReportExporter
{
    class Program
    {
        const string recoverFile = @"recover.txt";
        const string errorFile = @"error_list.txt";
        
        //const string reportName = "Borrower_History";
        //const string controlFile = @"\borrower_history";
        //const string saveFile = @"Borrower_History-";
        //const string reportURL = "http://lpp-2543/SSRS/Pages/ReportViewer.aspx?%2fTesting%2fNelnet+Borrower+History";

        //const string reportName = "Loan_History";
        //const string controlFile = @"loan_history";
        //const string saveFile = @"Loan_History-";
        //const string reportURL = "http://lpp-2543/SSRS/Pages/ReportViewer.aspx?%2fTesting%2fNelnet+Loan+History";

        const string reportName = "Co-Maker_Borrower_History";
        const string filesPath = @"C:\Users\ebarnes\Desktop\Co-Maker Borrower History Reports\";
        const string controlFile = @"co-maker_borrower_history";
        const string saveFile = @"co-maker_borrower_history-";
        const string reportURL = "http://lpp-2543/SSRS/Pages/ReportViewer.aspx?%2fTesting%2fCo-maker+Borrower+History";
        
        static int borrower_count = 0;
        static ArrayList Borrowers = new ArrayList();

        static void Main(string[] args)
        {
            //string sql = "SELECT DISTINCT BH.br_ssn FROM dbo.ITGLSQLDF_Borr_Hist BH WHERE BH.br_ssn < '527000000' ORDER BY BH.br_ssn DESC";
            //string sql = "SELECT DISTINCT CMBH.[Br SSN] [br_ssn] FROM dbo.Co_Maker_Borr_Hist CMBH WHERE CMBH.[Br SSN] IN ('117725682','114583653','096709859','075720892','067487124','065680496','043761598','025645338','001769669'    )"; //< " + Recover() + " ORDER BY CMBH.[Br SSN] DESC";
            string sql = "SELECT DISTINCT CMBH.[Br SSN] [br_ssn] FROM dbo.Co_Maker_Borr_Hist CMBH WHERE CMBH.[Br SSN] < " + Recover() + " ORDER BY CMBH.[Br SSN] DESC";
            //string sql = "SELECT DISTINCT CMBH.[Br SSN] [br_ssn] FROM dbo.Co_Maker_Borr_Hist CMBH WHERE CMBH.[Br SSN] < '043761598' ORDER BY CMBH.[Br SSN] DESC";
            
            //cmd.CommandText = "SELECT DISTINCT BH.br_ssn FROM dbo.ITGLSQLDF_Borr_Hist BH WHERE BH.br_ssn < " + Recover() + " ORDER BY BH.br_ssn DESC";

            GetBorrowers(sql);   
            GenerateReports();
            FindMissingReports();
            CreateControlFile(reportName);
        }

        private static void GenerateReports()
        {
            
            string br_ssn = null;
            int count = 0;

            try
            {

                Parallel.ForEach(Borrowers.Cast<object>(), new ParallelOptions { MaxDegreeOfParallelism = 10 }, borrower => 
                {
                    br_ssn = borrower.ToString();
                    Interlocked.Increment(ref count);

                    string Command = "Render";
                    string Format = "PDF";
                    string savePath = filesPath + saveFile + br_ssn + ".pdf";
                    string URL = reportURL + "&rs:Command=" + Command + "&rs:Format=" + Format + "&br_ssn=" + borrower.ToString();

                    Console.WriteLine("Processing borrower {0} on thread {1}\t\t({2} of {3})", borrower.ToString(), Thread.CurrentThread.ManagedThreadId, count, borrower_count);
                    HttpWebRequest Req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                    Req.UseDefaultCredentials = true;
                    Req.Timeout = int.MaxValue;
                    Req.Method = "GET";

                    WebResponse objResponse = Req.GetResponse();
                    FileStream fs = new System.IO.FileStream(savePath, System.IO.FileMode.Create);
                    Stream stream = objResponse.GetResponseStream();

                    byte[] buf = new byte[1024];
                    int len = stream.Read(buf, 0, 1024);

                    while (len > 0)
                    {
                        fs.Write(buf, 0, len);
                        len = stream.Read(buf, 0, 1024);
                    }
                    stream.Dispose();
                    fs.Dispose();
                    Console.WriteLine("Report created for borrower {0} on thread {1}\t({2} of {3})", borrower.ToString(), Thread.CurrentThread.ManagedThreadId, count, borrower_count);
                } );

                Console.WriteLine("Processing complete at " + br_ssn);
                Console.ReadLine();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filesPath + recoverFile, false))
                {
                    //file.WriteLine(br_ssn);
                    file.WriteLine("Processing complete at " + br_ssn);
                }
            
            }
            catch (Exception ex)
            {
                using (StreamWriter file = new System.IO.StreamWriter(filesPath + errorFile, true))
                {
                    file.WriteLine(br_ssn);
                }
                using (StreamWriter file = new System.IO.StreamWriter(filesPath + recoverFile, false))
                {
                    file.WriteLine(br_ssn);
                }
                Console.WriteLine("Error:  " + ex.InnerException + "\r\nMessage:  " + ex.Message);
                Console.WriteLine("Moving on...");
                Main(null);
            }
        }
        

        static void GetBorrowers(string sql)
        {

            Console.WriteLine("Loading borrowers...");

            SqlConnection opsdev = new SqlConnection();
            opsdev.ConnectionString = "Persist Security Info=False;Integrated Security=SSPI;database=NelNetImport;server=opsdev;Connect Timeout=30";
            opsdev.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql; 
            cmd.Connection = opsdev;

            SqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Borrowers.Add(reader["br_ssn"].ToString());
            }

            borrower_count = Borrowers.Count;
            Borrowers.Sort();
            Borrowers.Reverse();
        }

        static string Recover()
        {
            string br_ssn = "999999999";
            int result;
            try
            {
                if (File.Exists(filesPath + recoverFile))
                {
                    using (StreamReader file = new System.IO.StreamReader(filesPath + recoverFile))
                    {
                        br_ssn = file.ReadLine();
                    }
                }

                if(Int32.TryParse(br_ssn, out result) == false)
                    throw new Exception("Recover file SSN is not valid.");

             }
            catch (Exception ex)
            {
                Console.WriteLine("Error:  " + ex.InnerException + "\r\nMessage:  " + ex.Message);
                Console.WriteLine("Press ENTER to continue.");
                Console.ReadLine();
            }

            Console.WriteLine("Recovering at borrower:  " + br_ssn);
            return br_ssn;
        }

        static void FindMissingReports()
        {
            ArrayList reported = new ArrayList();
            ArrayList borrowers = new ArrayList(Borrowers);
            string[] filePaths = Directory.EnumerateFiles(filesPath, "*.pdf").ToArray();
         
            for (int i = 0; i < filePaths.Count(); i++)
            {
                reported.Add(Path.GetFileName(filePaths[i]).Substring(Path.GetFileName(filePaths[i]).Length - 13, 9));   
            }

            reported.Sort();
            reported.Reverse();
   
            foreach(string report in reported)
            {
                borrowers.Remove(report);
                Console.WriteLine("{0} borrowers not found", borrowers.Count);
            }

            foreach (string borrower in borrowers)
            {
                Console.WriteLine("'" + borrower + "',");
            }

            Console.WriteLine("Press enter to continue");
            Console.Read();

        }
        
        static void CreateControlFile(string reportName)
        {
                string date = "";
                string time = "";
                string dateTime = "";
                string doc_id = "CSCSH";
                int count = 0;

                StreamWriter file = null;

                foreach (string ssn in Borrowers)
                {

                    if (count % 50 == 0)
                    {
                        if (file != null)
                            file.Dispose();

                        file = GetNewControlFile(count / 50 + 1);

                        date = DateTime.Now.ToString("MM/dd/yyyy");
                        time = DateTime.Now.ToString("T");
                        dateTime = DateTime.Now.ToString("G");
                        
                        // Header line
                        file.WriteLine(@"~^Folder~{0}, Doc 1^Type~UTCR_TYPE^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{2}^Attribute~BATCH_NUM~STR~Nelnet^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{1}^Attribute~SCAN_TIME~STR~{3}^Attribute~DESCRIPTION~STR~{0}", dateTime, date, doc_id, time);
                    }

                    // Individual file line
                    file.WriteLine(@"DesktopDoc~\\imgprodkofax\ascent$\UTCROther_imp\{3}-{1}.pdf~{0}, Doc 1^Attribute~SSN~STR~{1}^Attribute~DOC_DATE~STR~{2}", dateTime, ssn, date, reportName);
                    count++;
                }
                file.Dispose();
            }
    

        static StreamWriter GetNewControlFile(int fileNumber)
        {
            StreamWriter file = new StreamWriter(filesPath + controlFile + "_" + fileNumber + ".ctl", false);

            return file;
        }
    }
}
