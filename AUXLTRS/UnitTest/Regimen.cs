using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Xunit;

namespace UnitTest
{
    public class Regimen
    {
        // Make sure we don't lose our test data.
        [Fact]
        public void CustomA()
        {
            string testFile = @"X:\PADD\Aux Services\CUSTOMA.doc";
            Assert.True( File.Exists(testFile));
        }

        [Fact]
        public void SsnCflt()
        {
            string testFile = @"X:\PADD\Aux Services\SSNCFLT.doc";
            Assert.True( File.Exists(testFile));
        }

        [Fact]
        public void NotSat()
        {
            string testFile = @"X:\PADD\AWG\NOTSAT.doc";
            Assert.True( File.Exists(testFile));
        }

        [Fact]
        public void ThrdPrty()
        {
            string testFile = @"X:\PADD\General\THRDPRTY.doc";
            Assert.True( File.Exists(testFile));
        }

        [Fact]
        public void Ssn()
        {
            string testFile = @"X:\PADD\DBQ\SSN1.doc";
            Assert.True( File.Exists(testFile));
        }

    }
}
