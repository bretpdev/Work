using System.Collections.Generic;
using System.IO;
using Q;

namespace R301QUEUE
{
	class TransactionReport
	{
		private readonly bool TEST_MODE;
		private readonly string TRANSACTION_FILE;

		public TransactionReport(bool testMode)
		{
			TEST_MODE = testMode;
			TRANSACTION_FILE = DataAccess.PersonalDataDirectory + "R301TransactionLevelData.txt";
		}

		public void AddRecord(SystemBorrowerDemographics demos, string amount)
		{
			string keyline = Common.ACSKeyLine(demos.SSN, Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal);
			bool newFile = !File.Exists(TRANSACTION_FILE);
			using (StreamWriter fileWriter = new StreamWriter(TRANSACTION_FILE, true))
			{
				if (newFile) { fileWriter.WriteCommaDelimitedLine("AN", "FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "Keyline", "RefundAmt", "State_Ind", "COST_CENTER_CODE"); }
				fileWriter.WriteCommaDelimitedLine(demos.AccountNumber, demos.FName, demos.LName, demos.Addr1, demos.Addr2, demos.City, demos.State, demos.Zip, demos.Country, keyline, amount, demos.State, "MA4119");
			}
		}//AddRecord()

		public void Delete()
		{
			File.Delete(TRANSACTION_FILE);
		}//Delete()

		public void Print()
		{
			string refundData = RollUpDataFile();
			if (File.Exists(refundData))
			{
				string barcodedFile = DocumentHandling.AddBarcodeAndStaticCurrentDateForUserProcessing(TEST_MODE, refundData, "AN", "RFDPIFB", true, 0, true, DocumentHandling.Barcode2DLetterRecipient.lrBorrower).NewFileName;
				DocumentHandling.PrintDocs(TEST_MODE, "RFDPIFB", barcodedFile);
				File.Delete(barcodedFile);
				File.Delete(refundData);
			}
		}//Print()

		//Takes an R301 data file at a transaction level and creates a new data file
		//grouped at the account level, with the transaction amounts being summed.
		//TODO: If the file is not too big, this can be greatly simplified by using a DataTable.
		private string RollUpDataFile()
		{
			string accountDataFile = DataAccess.PersonalDataDirectory + "R301AccountLevelData.txt";

			if (File.Exists(TRANSACTION_FILE))
			{
				//Create a new account-level data file with just a header row to start out.
				using (StreamWriter accountWriter = new StreamWriter(accountDataFile))
				{
					accountWriter.WriteCommaDelimitedLine("AN", "FirstName", "LastName", "Address1", "Address2", "City", "State", "Zip", "Country", "Keyline", "RefundAmt", "State_Ind", "COST_CENTER_CODE");
				}

				using (StreamReader transactionReader = new StreamReader(TRANSACTION_FILE))
				{
					List<string> processedAccountNumbers = new List<string>();
					//Get column headers out of the way.
					transactionReader.ReadLine();
					while (!transactionReader.EndOfStream)
					{
						List<string> transactionFields = transactionReader.ReadLine().SplitAgnosticOfQuotes(",");
						string accountNumber = transactionFields[0];
						string firstName = transactionFields[1];
						string lastName = transactionFields[2];
						string address1 = transactionFields[3];
						string address2 = transactionFields[4];
						string city = transactionFields[5];
						string state = transactionFields[6];
						string zip = transactionFields[7];
						string country = transactionFields[8];
						string keyline = transactionFields[9];
						string amount = transactionFields[10];
						//Index 11 is just the state again.
						string costCenter = transactionFields[12];

						//For accounts that haven't been summed yet, go through the file and get a sum of the amounts.
						if (!processedAccountNumbers.Contains(accountNumber))
						{
							processedAccountNumbers.Add(accountNumber);
							double accountTotal = 0;
							using (StreamReader searchReader = new StreamReader(TRANSACTION_FILE))
							{
								while (!searchReader.EndOfStream)
								{
									List<string> searchFields = searchReader.ReadLine().SplitAgnosticOfQuotes(",");
									string searchAccountNumber = searchFields[0];
									double searchAmount = double.Parse(searchFields[10]);
									if (searchAccountNumber == accountNumber) { accountTotal += searchAmount; }
								}
							}//using
							using (StreamWriter accountWriter = new StreamWriter(accountDataFile))
							{
								accountWriter.WriteCommaDelimitedLine(accountNumber, firstName, lastName, address1, address2, city, state, zip, country, keyline, accountTotal.ToString("$###,##0.00"), state, costCenter);
							}
						}//if
					}//while
				}//using
			}//if

			return accountDataFile;
		}//RollUpDataFile()
	}//class
}//namespace
