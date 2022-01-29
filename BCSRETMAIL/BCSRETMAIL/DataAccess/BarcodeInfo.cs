using System;
using Uheaa.Common;

namespace BCSRETMAIL
{
    public class BarcodeInfo
	{
        public DateTime CreateDate { get; set; }
		public string LetterId { get; set; }
		public string RecipientId { get; set; }
        public DateTime ReceivedDate { get; set; }
		public SystemType System { get; set; }
		public enum SystemType { Both, Compass, Onelink }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
        public string Comment { get; set; }

        public static BarcodeInfo Parse(string barcodeInput)
		{
			//Barcodes include the following fields:
			//Recipient ID (0-9, 10-digit account number for borrowers, or 9-digit reference ID and a space for references)
			//Letter ID (10-19, padded with spaces)
			//Create date (20-27, "MMddyyyy" for letters we create; 20-37, "MMMM dd, yyyy" for letters AES creates)

			if (barcodeInput.Contains("=="))
            {
                string accountNumber = LegacyCryptography.Decrypt(barcodeInput.Substring(0, barcodeInput.LastIndexOf("=") + 1), LegacyCryptography.Keys.NoradOPS);
                barcodeInput = $"{accountNumber}{barcodeInput.Substring(barcodeInput.LastIndexOf("=") + 1)}";
            }

			int createDateLength = (barcodeInput.Length > 30 ? 18 : 8);
			//Split the barcode input into its constituent parts.
			BarcodeInfo info = new BarcodeInfo();
			try
			{
                info.RecipientId = barcodeInput.Substring(0, 10).Trim();
                if (info.RecipientId.Length != 10)//Accounting for references
                    barcodeInput = barcodeInput.Insert(0, " ");

                if ((info.RecipientId.ToUpper().StartsWith("RF@") || info.RecipientId.ToUpper().StartsWith("P")) && createDateLength == 8)
                {
                    info.LetterId = barcodeInput.Substring(10, barcodeInput.Length - 18).Trim(); //In some cases there are extra spaces so we need to get all the characters between the create date and recipient id as the letter id
					info.CreateDate = barcodeInput.Substring(barcodeInput.Length - 8, createDateLength).Trim().ToDate();
                }
                else
                {
                    info.LetterId = barcodeInput.Substring(10, 10).Trim();
					info.CreateDate = barcodeInput.Substring(20, createDateLength).Trim().ToDate();
                }
            }
			catch (FormatException ex)
			{
				string message = $"The letter create date ({barcodeInput.Substring(20)}) is not in a recognized format.";
				throw new FormatException(message, ex);
			}
			catch (Exception ex)
			{
				string message = $"The barcode input ({barcodeInput.Trim()}) is incomplete or not recognizable.";
				throw new Exception(message, ex);
            }
			return info;
		}
	}
}