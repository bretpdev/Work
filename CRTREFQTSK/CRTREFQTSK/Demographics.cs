using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRTREFQTSK
{
    class Demographics
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mid { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string State { get; set; }
        public string AreaCode { get; set; }
        public string Exchange { get; set; }
        public string LocalNum { get; set; }
        public string Relationship { get; set; }

        public Demographics(string fileLine)
        {
            string[] parsedData = fileLine.Split(',');
            SSN = parsedData[0];
            FirstName = parsedData[1];
            LastName = parsedData[2];
            Mid = parsedData[3];
            ZipCode = parsedData[5];
            City = parsedData[6];
            Street1 = parsedData[7];
            Street2 = parsedData[8];
            State = parsedData[9];
            AreaCode = parsedData[11];
            Exchange = parsedData[12];
            LocalNum = parsedData[13];
            Relationship = parsedData[14];
        }

        public string BuildComment()
        {
            return string.Format("{0},{1},{2},Other,{3},{4},{5},{6},{7},{8},{9},{10},{11}", FirstName, Mid, LastName,
                   Relationship, Street1, City, State, ZipCode, AreaCode, Exchange, LocalNum, Street2);
        }
    }
}
