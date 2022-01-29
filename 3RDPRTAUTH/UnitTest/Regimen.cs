using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Xunit;

namespace UnitTest
{
    public class Regimen
    {
        [Fact]
        public void ThrdPrty()
        {
            string testFile = @"X:\PADD\General\THRDPRTY.doc";
            Assert.True( File.Exists(testFile));
        }



        [Fact]
        public void TestKeyLine()
        {
            string eax = "";
            eax = DocumentProcessing.ACSKeyLine("0123456789", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
            if(eax == "#PMRETHGUAL0822L1#")
                Assert.True(1 == 1);
            else
                Assert.True(1 == 2);

        }
    }
}
