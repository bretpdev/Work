using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace FINALREV
{
    public class OneLinkProcess
    {
        public ReflectionInterface RI { get; set; }
        public Steps Step { get; set; }
        public BorrowerRecord Borrower { get; set; }
        public DataAccess DA { get; set; }

        public OneLinkProcess(ReflectionInterface ri, BorrowerRecord bor)
        {
            RI = ri;
            Borrower = bor;
            Step = new Steps();
            DA = new DataAccess(RI.LogRun);
        }

        public void Process()
        {
            RI.FastPath($"LP8QI{Borrower.Demos.Ssn}");
            if (Borrower.SkipType == "A")
                Address();
            else if (Borrower.SkipType == "P")
                Phone();
            else if (Borrower.SkipType == "B")
                Both();

            if (!Step.ACURINT2)
                AddQueue("ACURINT2", FinalReview.RecoveryStep.ACURINT2);
        }

        #region Address

        /// <summary>
        /// Search LP8QI for school contact and credit report
        /// </summary>
        private void Address()
        {
            SearchAddressLp8q();
            CheckAddressLP50();

            if (!Step.KSCHLLTR || (!Step.BRWRCALa || !Step.BRWRCALs) || !Step.ACURINT2)
                Borrower.OLTaskNeeded = true;

            if (Borrower.Step < FinalReview.RecoveryStep.REF_REV)
            {
                References r = new References(RI, Borrower, DA);
                r.ReviewReferences();
            }
        }

        /// <summary>
        /// Searches LP8Q to look for SCHLLTTR or SCHLCALL queues
        /// </summary>
        private void SearchAddressLp8q()
        {
            int row = 9;
            while (RI.AltMessageCode != "46004")
            {
                if (RI.GetText(row, 12, 8).IsIn("SCHLLTTR", "SCHLCALL"))
                {
                    Step.KSCHLLTR = true;
                    break; //If found, no need to continue searching
                }
                row += 2;
                if (row > 19 || RI.CheckForText(row, 7, " "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                }
            }
        }

        /// <summary>
        /// earch LP50 for borrower call, DMV, DA, and Internet
        /// </summary>
        private void CheckAddressLP50()
        {
            int arcCount = 0;
            RI.FastPath($"LP50I{Borrower.Demos.Ssn};;X");
            while (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY") && RI.AltMessageCode != "46004")
            {
                if (RI.GetText(7, 15, 8).ToDate() < Borrower.StartDate)
                    break;
                string arc = RI.GetText(8, 2, 5);
                if (arc.IsIn("KABAB", "KUBAB"))
                {
                    arcCount++;
                    if (arcCount > 1)
                        Step.BRWRCALa = true;
                }
                else if (arc == "KSBAB")
                    Step.BRWRCALs = true;
                else if (arc == "KUBSS")
                    Step.ACURINT2 = true;
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        #endregion

        #region Phone

        private void Phone()
        {
            CheckPhoneTd2a();
            CheckPhoneLp50();

            if (!Step.KSCHLLTR || !Step.ACURINT2)
                Borrower.OLTaskNeeded = true;

            if (Borrower.Step < FinalReview.RecoveryStep.REF_REV)
            {
                References r = new References(RI, Borrower, DA);
                r.ReviewReferences();
            }
        }

        private void CheckPhoneTd2a()
        {
            RI.FastPath($"TX3Z/ITD2A{Borrower.Demos.Ssn}");
            RI.PutText(4, 66, "X", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TDX2C")
                RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
            while (RI.ScreenCode == "TDX2D" && RI.MessageCode != "90007")
            {
                if (RI.GetText(13, 31, 8).ToDate() < Borrower.StartDate)
                    break;
                if (RI.CheckForText(13, 2, "KLSCH"))
                    Step.KSCHLLTR = true;
                NextPage();
            }
        }

        private void CheckPhoneLp50()
        {
            RI.FastPath($"LP50IP{Borrower.Demos.Ssn};;X");
            while (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY") && RI.AltMessageCode != "46004")
            {
                if (RI.GetText(7, 15, 8).ToDate() < Borrower.StartDate)
                    break;
                if (RI.CheckForText(8, 2, "KUBSS"))
                    Step.ACURINT2 = true;
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        #endregion

        #region Both

        /// <summary>
        /// Searches LP8Q for school contact and credit report
        /// </summary>
        private void Both()
        {
            SearchBothLp8q();
            CheckBothLp50();

            if (!Step.KSCHLLTR || !Step.ACURINT2)
                Borrower.OLTaskNeeded = true;

            if (Borrower.Step < FinalReview.RecoveryStep.REF_REV)
            {
                References r = new References(RI, Borrower, DA);
                r.ReviewReferences();
            }
        }

        /// <summary>
        /// Searches LP8Q to look for SCHLLTTR or SCHLCALL queues
        /// </summary>
        private void SearchBothLp8q()
        {
            int row = 9;
            while (RI.AltMessageCode != "46004")
            {
                if (RI.CheckForText(row, 12, "SCHLLTTR", "SCHLCALL"))
                {
                    Step.KSCHLLTR = true;
                    break; //One was found, no need to continue searching.
                }
                row += 2;
                if (row > 19 || RI.CheckForText(row, 7, " "))
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                }
            }
        }

        /// <summary>
        /// Search LP50 for DMV, DA and internet
        /// </summary>
        private void CheckBothLp50()
        {
            RI.FastPath($"LP50I{Borrower.Demos.Ssn};;X");
            while (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY") && RI.AltMessageCode != "46004")
            {
                if (RI.GetText(7, 15, 8).ToDate() < Borrower.StartDate)
                    break;
                if (RI.CheckForText(8, 2, "KUBSS"))
                {
                    Step.ACURINT2 = true;
                    break;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        #endregion

        public void NextPage()
        {
            if (RI.MessageCode == "01033")
            {
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.ScreenCode == "TDX2C")
                    RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
            }
            else
                RI.Hit(ReflectionInterface.Key.F8);
        }

        public void AddQueue(string queue, FinalReview.RecoveryStep step)
        {
            if (!RI.AddQueueTaskInLP9O(Borrower.Demos.Ssn, queue))
            {
                string message = $"There was an error adding queue: {queue} to account: {Borrower.Demos.AccountNumber} in LP90.";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
            }
            Borrower.UpdateStep(DA, step);
        }
    }
}