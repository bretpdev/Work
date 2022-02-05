using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Q;

namespace OPSACTCMNT
{
	public class OpsActivityComments : BatchScriptBase
	{
		private readonly bool _tracing;
		private readonly string _traceFile;

		public OpsActivityComments(ReflectionInterface ri)
			: base(ri, "OPSACTCMNT")
		{
			_traceFile = Efs.TempFolder + ScriptID + "_traceLog.txt";
			_tracing = File.Exists(_traceFile);
		}

		public override void Main()
		{
			Trace("Script has started");

			StartupMessage("This script will add activity comments for OPS payments to COMPASS. Click OK to continue, or Cancel to quit.");

			Trace(string.Format("Startup message has run, also checking to see if this was called by Master Batch {0}", CalledByMasterBatchScript()));
			bool isInRecovery = !string.IsNullOrEmpty(Recovery.RecoveryValue);

			//Get the file name
			DirectoryInfo diSource = new DirectoryInfo(FtpFolder);
			foreach (FileInfo file in diSource.GetFiles("webpayut.*"))
			{
				Trace(string.Format("Looking in dir: {0} using file {1}", FtpFolder, file.FullName));
				IEnumerable<PaymentData> data = LoadPaymentData(file.FullName);

				foreach (PaymentData pData in data)
				{
					string comment = string.Format("Payment amount = {0:C} with an effective date of {1}. The confirmation number is {2}.", pData.PaymentAmount, pData.PaymentEffectiveDate, pData.ConfirmationNumber);
					string arc = "OPSWP";
					if (pData.PaymentType == "T" || pData.PaymentType == "E") { arc = "OPSTP"; }
					if (isInRecovery)
					{
						if (Recovery.RecoveryValue == pData.FileLine && Recovery.RecoveryValue == file.FullName)
						{
							isInRecovery = false;
						}
					}
					else
					{
						ATD22AllLoans(pData.SSN, arc, comment, false);
						Recovery.RecoveryValue = string.Format("{0},{1}", pData.FileLine, file.FullName);
					}
				}

				File.Delete(file.FullName);
				Trace("File has been deleted report will be generated");
				PrintReport(data, file.FullName);
				Trace("Report has been generated");
			}
			Trace("End of script");
			ProcessingComplete();
		}

		/// <summary>
		/// Adds the payment amounts and creates a word document
		/// </summary>
		/// <param name="data"></param>
		private void PrintReport(IEnumerable<PaymentData> data, string exactFileName)
		{

			//Add up the two payment types
			double wPay = (from d in data
						   where d.PaymentType == "W"
						   select d.PaymentAmount).Sum();
			int wNumber = (from d in data
						   where d.PaymentType == "W"
						   select d.PaymentType).Count();
			double ePay = (from d in data
						   where d.PaymentType == "T" || d.PaymentType == "E"
						   select d.PaymentAmount).Sum();
			int eNumber = (from d in data
						   where d.PaymentType == "T" || d.PaymentType == "E"
						   select d.PaymentType).Count();

			//Open a blank Word document.
			using (WordFacade turd = WordFacade.CreateDocument())
			{

				turd.Visible = false;

				//Strip out the date from the file name
				exactFileName = exactFileName.Remove(0, exactFileName.Length - 8);
				exactFileName = exactFileName.Insert(2, "/");
				exactFileName = exactFileName.Insert(5, "/");

				//Fill out the document
				turd.FontSize = 15;
				turd.WriteLine("OPS Usage " + exactFileName);
				turd.WriteLine("");
				turd.WriteLine("");
				turd.WriteLine("");
				turd.FontSize = 10;
				turd.WriteLine("Number of borrowers making web payments = " + wNumber.ToString());
				turd.WriteLine("");
				turd.WriteLine("Total Amount of web payments = " + string.Format("{0:C}", wPay));
				turd.WriteLine("");
				turd.WriteLine("");
				turd.WriteLine("Number of borrowers making telephone payments = " + eNumber.ToString());
				turd.WriteLine("");
				turd.WriteLine("Total Amount of Telephone Payments = " + string.Format("{0:C}", ePay));

				//save the document
				string fileName = Efs.GetPath("EOJ_OPSACTCMNT") + "OPS_Usage_" + exactFileName.Replace("/", "_") + ".doc";
				turd.SaveAs(fileName);

			}
		}

		/// <summary>
		/// Creates a PaymentData object from file
		/// </summary>
		/// <returns></returns>
		private IEnumerable<PaymentData> LoadPaymentData(string exactFileName)
		{
			const int FIELD_COUNT = 9;
			List<PaymentData> paymentData = new List<PaymentData>();
			using (StreamReader sr = new StreamReader(exactFileName))
			{

				while (!sr.EndOfStream)
				{
					string readLine = sr.ReadLine();
					PaymentData data = new PaymentData();
					data.FileLine = readLine;
					List<string> line = data.FileLine.SplitAgnosticOfQuotes(",");
					if (line.Count != FIELD_COUNT)
					{
						string message = string.Format("Expected {0} fields but found {1} in the following line:{2}", FIELD_COUNT, line.Count, Environment.NewLine);
						message += readLine;
						throw new Exception(message);
					}
					data.PaymentType = line[0].Trim();
					data.PaymentEffectiveDate = line[1].Trim();
					data.BorrowerName = line[2].Trim();
					data.SSN = line[3].Trim();
					data.PaymentAmount = Convert.ToDouble(line[4].Trim()) / 100;
					data.AccountType = line[5].Trim();
					data.AccountName = line[6].Trim();
					data.DatePaymentSubmitted = line[7].Trim();
					data.ConfirmationNumber = line[8].Trim();
					paymentData.Add(data);
				}
			}
			return paymentData;
		}
		private void Trace(string message)
		{
			if (!_tracing) { return; }
			File.AppendAllText(_traceFile, string.Format("{0:MM/dd/yyyy-hh-mm-ss}\t{1}\n", DateTime.Now, message));
		}

	}
}
