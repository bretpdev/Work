using System;
using System.IO;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Xunit;
using System.Collections.Generic;
using SCRADEFALT;

namespace SCRADEFALT_Tests
{
    
    public class FileHandling
    {
        [Theory]
        [InlineData("This is my header row \r\n\"I Am a test ssn\", \" I am a test comment\"")]
        public void FileReadSuccess(string fileData)
        {
            string path = @"T:\UTLWD43.Testfile.testdate";
            using (FileStream fs = File.Create(path))
            {
                Byte[] data = new UTF8Encoding(true).GetBytes(fileData);
                fs.Write(data, 0, data.Length);
            }
            SCRADEFALT.SCRADEFALT Item = new SCRADEFALT.SCRADEFALT();
            List<ScraData> Records = Item.ReadFile(path);
            Assert.True(Records.Count != 0); //Read in a record
            File.Delete(path);
        }
    }
}
