using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace NSLDSCONSO
{
    public class CompassHelper : IDisposable
    {
        ProcessLogRun plr;
        public ReflectionInterface RI { get; internal set; }
        BatchProcessingHelper login;
        public CompassHelper(ProcessLogRun plr)
        {
            this.plr = plr;
            this.RI = new ReflectionInterface();
        }

        public void Dispose()
        {
            this.RI.CloseSession();
            if (login != null)
                BatchProcessingHelper.CloseConnection(login);
        }

        public bool Login()
        {
            login = BatchProcessingLoginHelper.Login(plr, RI, this.GetType().Namespace, "BatchCornerstone");
            return login != null;
        }

        public bool RequestBorrower(Borrower borrower)
        {
            RI.FastPath("tx3z/ATL40" + borrower.Ssn);
            RI.PutText(9, 36, "AAC");
            if (RI.CheckForText(15, 36, "_", "M"))
                RI.PutText(15, 36, borrower.DateOfBirth.ToString("MM/dd/yyyy"));
            if (RI.CheckForText(11, 36, "_") || RI.CheckForText(13, 36, "_"))
            {
                var parsedName = new NameParser(borrower.Name);
                RI.PutText(11, 36, parsedName.LastName);
                RI.PutText(13, 36, parsedName.FirstName);
            }
            RI.Hit(ReflectionInterface.Key.Enter);
            return RI.CheckForText(22, 2, "01004", "01018");
        }
    }
}
