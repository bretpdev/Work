using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Test
{
    [TestClass]
    public class Regimin
    {
        [TestMethod]
        public void TestDLOPortfolio()
        {
            LNSLSSNFED.Portfolios portfolio = new LNSLSSNFED.Portfolios();
            Dictionary<string, string> dlos = new Dictionary<string, string> { { "DLSTFD", "DLSTFD" }, { "DLUNST", "DLUNST" }, { "DLPLGB", "DLPLDB" }, { "DLPLUS", "DLPLUS" }, { "TEACH", "TEACH" } };
            bool contains = true;

            foreach( KeyValuePair<string, string> dlo in dlos)
            {
                if(!portfolio.DLO.ContainsKey(dlo.Key))
                {
                    contains = false;
                    break;
                }
            }

            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void TestLNCPortfolio()
        {
            LNSLSSNFED.Portfolios portfolio = new LNSLSSNFED.Portfolios();
            Dictionary<string, string> lncs = new Dictionary<string, string> { { "DLPCNS", "DLPCNS" }, { "DLSCNS", "DLSCNS" }, { "DLUSPL", "DLUSPL" }, { "DLSSPL", "DLSSPL" }, { "DLUCNS", "DLUCNS" } };
            bool contains = true;

            foreach (KeyValuePair<string, string> lnc in lncs)
            {
                if (!portfolio.LNC.ContainsKey(lnc.Key))
                {
                    contains = false;
                    break;
                }
            }

            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void TestDirectoryStructure()
        {
            string pslf = "Q:\\CS Loan Servicing\\PSLF Transfer";
            string split = "Q:\\CS Loan Servicing\\Split Loan Transfer";
            int exists = 0;
            DirectoryInfo dirSplit;
            DirectoryInfo dirPslf;

            dirPslf = new DirectoryInfo(pslf);

            if (dirPslf.Exists)
                exists++;
            
            dirSplit = new DirectoryInfo(split);

            if (dirSplit.Exists)
                exists++;

            Assert.IsTrue(exists == 2);
        }


    }
}
