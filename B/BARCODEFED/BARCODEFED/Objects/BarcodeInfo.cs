using System;
using Uheaa.Common;

namespace BARCODEFED
{
	public class BarcodeInfo
	{
		public DateTime CreateDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string LetterId { get; set; }
		public BarcodeScanner.PersonType PersonType { get; set; }
		public string RecipientId { get; set; }

		public static BarcodeInfo Parse(string barcodeInput)
		{
			//Barcodes include the following fields:
			//Recipient ID (0-9, 10-digit account number for borrowers, or 9-digit reference ID and a space for references)
			//Letter ID (10-19, padded with spaces)
			//Create date (20-27, "MMddyyyy" for letters we create; 20-37, "MMMM dd, yyyy" for letters AES creates)
			//Person type (28-29 for letters we create; 38-39 for letters AES creates)
			const int RECIPIENT_ID_START = 0;
			const int RECIPIENT_ID_LENGTH = 10;
			const int LETTER_ID_START = RECIPIENT_ID_LENGTH;
			const int LETTER_ID_LENGTH = 10;
			const int CREATE_DATE_START = RECIPIENT_ID_LENGTH + LETTER_ID_LENGTH;
			int createDateLength = (barcodeInput.Length > 30 ? 18 : 8);
			int personTypeStart = RECIPIENT_ID_LENGTH + LETTER_ID_LENGTH + createDateLength;
			const int PERSON_TYPE_LENGTH = 2;

			//Split the barcode input into its constituent parts.
			BarcodeInfo info = new BarcodeInfo();
			try
			{
				info.RecipientId = barcodeInput.Substring(RECIPIENT_ID_START, RECIPIENT_ID_LENGTH).Trim();
				info.LetterId = barcodeInput.Substring(LETTER_ID_START, LETTER_ID_LENGTH).Trim();
				string createDate = barcodeInput.Substring(CREATE_DATE_START, createDateLength).Trim();
				if (createDateLength == 8) { createDate = createDate.ToDateFormat(); }
				info.CreateDate = DateTime.Parse(createDate);
				//Let PersonType default to borrower for cases where it's not in the barcode.
				info.PersonType = BarcodeScanner.PersonType.NONE;
				if (barcodeInput.Length > personTypeStart)
				{
					string typeString = barcodeInput.Substring(personTypeStart, PERSON_TYPE_LENGTH);
					switch (typeString)
					{
						case "P1":
							info.PersonType = BarcodeScanner.PersonType.Borrower;
							break;
						case "P3":
							info.PersonType = BarcodeScanner.PersonType.Endorser;
							break;
						default:
							//The spec says any code that isn't P1 or P3 is a reference code.
							info.PersonType = BarcodeScanner.PersonType.Reference;
							break;
					}
				}
			}
			catch (FormatException ex)
			{
				string message = $"The letter create date ({barcodeInput.Substring(20)}) is not in a recognized format.";
				throw new FormatException(message, ex);
			}
			catch (Exception ex)
			{
				string message = $"The barcode input ({barcodeInput}) is incomplete or not recognizable.";
				throw new Exception(message, ex);
			}
			return info;
        }
    }
}