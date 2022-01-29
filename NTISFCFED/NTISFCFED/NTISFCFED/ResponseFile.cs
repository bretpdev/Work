using System.Collections.Generic;
using Uheaa.Common;

namespace NTISFCFED
{
    public class ResponseFile
    {
        public string ResponseFileName { get; set; }
        public string ServicerCode { get; set; }
        public string FulfilSourceFile { get; set; }
        public string DocumentName { get; set; }
        public string FileType { get; set; }
        public string NumberOfPages { get; set; }
        public string AltFormat { get; set; }
        public string DataCdFormat { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ForeignStte { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string InternationalZip { get; set; }
        public string AccountNumber { get; set; }
        public string ReturnName { get; set; }
        public string ReturnAddress1 { get; set; }
        public string ReturnAddress2 { get; set; }
        public string ReturnAddress3 { get; set; }
        public string ReturnCity { get; set; }
        public string ReturnState { get; set; }
        public string ReturnZip { get; set; }
        public string StatusCode { get; set; }
        public string CompletedDate { get; set; }
        public string ShippingCourier { get; set; }
        public string TrackingNumber { get; set; }

        public ResponseFile(string line)
        {
            LoadObject(line.SplitAndRemoveQuotes(","));
        }

        public string ParseStatus()
        {
            switch (StatusCode)
            {
                case "R":
                    return "Received";
                case "I":
                    return "In Progress";
                case "E":
                    return "Error";
                case "F":
                    return "Fulfilled";
                case "S":
                    return "Escalated";
                case "A":
                    return "Abandoned";
                default:
                    return null;
            }
        }

        private void LoadObject(List<string> records)
        {
            ResponseFileName = records[0];
            ServicerCode = records[1];
            FulfilSourceFile = records[2];
            DocumentName = records[3];
            FileType = records[4];
            NumberOfPages = records[5];
            AltFormat = records[6];
            DataCdFormat = records[7];
            FirstName = records[8];
            LastName = records[9];
            Address1 = records[10];
            Address2 = records[11];
            Address3 = records[12];
            Address4 = records[13];
            Address5 = records[14];
            City = records[15];
            State = records[16];
            ForeignStte = records[17];
            Country = records[18];
            ZipCode = records[19] + records[20];
            InternationalZip = records[21];
            AccountNumber = records[22];
            ReturnName = records[23];
            ReturnAddress1 = records[24];
            ReturnAddress2 = records[25];
            ReturnAddress3 = records[26];
            ReturnCity = records[27];
            ReturnState = records[28];
            ReturnZip = records[29] + records[30];
            StatusCode = records[31];
            CompletedDate = records[34];
            ShippingCourier = records[36];
            TrackingNumber = records[37];
        }
    }
}
