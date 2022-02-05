using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace FINALREV
{
    public class CompassProcess
    {
        public ReflectionInterface RI { get; set; }
        public Steps Step { get; set; }
        public BorrowerRecord Borrower { get; set; }
        public DataAccess DA { get; set; }
        public SchoolLetter Letter { get; set; }

        public CompassProcess(ReflectionInterface ri, BorrowerRecord bor)
        {
            RI = ri;
            Borrower = bor;
            Step = new Steps();
            DA = new DataAccess(RI.LogRun);
            Letter = new SchoolLetter(RI, DA, bor);
        }

        public void Process()
        {
            if (Borrower.SkipType == "A")
                Address();
            else if (Borrower.SkipType == "P")
                Phone();
            else if (Borrower.SkipType == "B")
                Both();
        }

        #region Address

        /// <summary>
        /// Processes skip type Address
        /// </summary>
        private void Address()
        {
            Letter = new SchoolLetter(RI, DA, Borrower);
            List<string> schoolCode = GetSchoolCode();
            CheckAddressLp50();
            AddAddressQueues(schoolCode);
        }

        /// <summary>
        /// Search LP50 for endorser call, borrower call, DMV, credit report, DA and internet
        /// </summary>
        private void CheckAddressLp50()
        {
            RI.FastPath($"LP50I{Borrower.Demos.Ssn};;X");
            int endCount = 0;
            int arcCount = 0;
            while (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY") && RI.AltMessageCode != "46004")
            {
                if (RI.GetText(7, 15, 8).ToDate() < Borrower.StartDate)
                    break;
                string arc = RI.GetText(8, 2, 5);
                if (arc.IsIn("KAEAB", "KUEAB") && Borrower.IsEndorser)
                {
                    endCount++;
                    if (endCount > 1)
                        Step.ENDORSEu = true;
                }
                else if (arc == "KSEAB")
                {
                    if (Borrower.IsEndorser)
                        Step.ENDORSEs = true;
                }
                else if (arc.IsIn("KABAB", "KUBAB"))
                {
                    if (!Borrower.IsEndorser)
                    {
                        arcCount++;
                        if (arcCount > 1)
                            Step.BRWRCALa = true;
                    }
                }
                else if (arc == "KSBAB")
                {
                    if (!Borrower.IsEndorser)
                        Step.BRWRCALs = true;
                }
                else if (arc == "KUBSS")
                    Step.ACURINT2 = true;

                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        /// <summary>
        /// Add queues for skip type address
        /// </summary>
        /// <param name="schoolCodes"></param>
        private void AddAddressQueues(List<string> schoolCode)
        {
            if (!Step.KORGLNDR && Borrower.Step < FinalReview.RecoveryStep.KORGLNDR)
                AddQueue("KORGLNDR", FinalReview.RecoveryStep.KORGLNDR);
            if (!Step.KSCHLLTR && Borrower.Step < FinalReview.RecoveryStep.KSCHLLTR)
            {
                if (!Letter.SchoolLetterCreated(schoolCode))
                    Step.KSCHLLTR = true;
                Borrower.UpdateStep(DA, FinalReview.RecoveryStep.KSCHLLTR);
            }
            if (Borrower.IsEndorser && (!Step.ENDORSEu || !Step.ENDORSEs) && Borrower.Step < FinalReview.RecoveryStep.KENDORSR)
                AddQueue("KENDORSR", FinalReview.RecoveryStep.KENDORSR);
            if ((!Step.BRWRCALa || !Step.BRWRCALs) && Borrower.Step < FinalReview.RecoveryStep.BRWRCALS)
                AddQueue("BRWRCALS", FinalReview.RecoveryStep.BRWRCALS);
            if (!Step.ACURINT2 && Borrower.Step < FinalReview.RecoveryStep.ACURINT2)
                AddQueue("ACURINT2", FinalReview.RecoveryStep.ACURINT2);

            if ((!Step.KORGLNDR || !Step.KSCHLLTR) || (Borrower.IsEndorser && !Step.ENDORSEu) || ((!Step.BRWRCALa || !Step.BRWRCALs)) || !Step.ACURINT2)
                Borrower.TaskAdded = true;

            if (Borrower.Step < FinalReview.RecoveryStep.REF_REV)
            {
                References r = new References(RI, Borrower, DA);
                r.ReviewReferences();
            }
        }

        #endregion

        #region Phone

        /// <summary>
        /// Processes skip type Phone
        /// </summary>
        private void Phone()
        {
            List<string> schoolCodes = CheckPhoneTd2a();
            CheckPhoneLP50();
            AddPhoneQueues(schoolCodes);
        }

        /// <summary>
        /// Checks TD2A for KLSCH arc
        /// </summary>
        private List<string> CheckPhoneTd2a()
        {
            List<string> schoolCodes = new List<string>();
            RI.FastPath($"TX3Z/ITD2A{Borrower.Demos.Ssn}");
            RI.PutText(4, 66, "X", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TDX2C")
                RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
            string comment = "";
            while (RI.ScreenCode == "TDX2D" && RI.MessageCode != "90007")
            {
                if (RI.GetText(13, 31, 8).ToDate() < Borrower.StartDate)
                    break;
                string arc = RI.GetText(13, 2, 5);
                if (arc == "KLSCH")
                    Step.KSCHLLTR = true;
                else if (arc == "KLSLT" && schoolCodes.Count == 0)
                {
                    for (int i = 17; i < 22; i++)
                        comment += RI.GetText(i, 2, 79);
                    List<string> sc = comment.Replace(",", " ").Replace(".", " ").Split(' ').Where(p => p.IsNumeric() && p.Length == 8).ToList();
                    schoolCodes.AddRange(sc);
                }
                NextPage();
            }
            return schoolCodes;
        }

        /// <summary>
        /// Checkps LP50 for ACURINT2 queue
        /// </summary>
        private void CheckPhoneLP50()
        {
            RI.FastPath($"LP50I{Borrower.Demos.Ssn};;X");
            while (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY") && RI.AltMessageCode != "46004")
            {
                if (RI.GetText(7, 15, 8).ToDate() < Borrower.StartDate)
                    break;
                if (RI.CheckForText(8, 2, "KUBSS"))
                    Step.ACURINT2 = true;
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        /// <summary>
        /// Add queues for skip type phone.
        /// </summary>
        /// <param name="schoolCodes"></param>
        private void AddPhoneQueues(List<string> schoolCodes)
        {
            if (!Step.KSCHLLTR && Borrower.Step < FinalReview.RecoveryStep.KSCHLLTR)
            {
                if (!Letter.SchoolLetterCreated(schoolCodes))
                    Step.KSCHLLTR = true;
                Borrower.UpdateStep(DA, FinalReview.RecoveryStep.KSCHLLTR);
            }

            if (!Step.ACURINT2 && Borrower.Step < FinalReview.RecoveryStep.ACURINT2)
                AddQueue("ACURINT2", FinalReview.RecoveryStep.ACURINT2);

            if (!Step.KSCHLLTR || !Step.ACURINT2)
                Borrower.TaskAdded = true;

            if (Borrower.Step < FinalReview.RecoveryStep.REF_REV)
            {
                References r = new References(RI, Borrower, DA);
                r.ReviewReferences();
            }
        }

        #endregion

        #region Both

        private void Both()
        {
            List<string> schoolCodes = GetSchoolCode();
            CheckBothLp50();
            AddBothQueues(schoolCodes);
        }

        /// <summary>
        /// search LP50 for DMV, credit report, DA, and Internet
        /// </summary>
        private void CheckBothLp50()
        {
            RI.FastPath($"LP50I{Borrower.Demos.Ssn};;X");
            while (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY") && RI.AltMessageCode != "46004")
            {
                if (RI.GetText(7, 15, 8).ToDate() < Borrower.StartDate)
                    break;
                if (RI.CheckForText(8, 2, "KUBSS"))
                    Step.ACURINT2 = true;
                RI.Hit(ReflectionInterface.Key.F8);
            }
        }

        /// <summary>
        /// add queue tasks for missing activities
        /// </summary>
        /// <param name="schoolCodes"></param>
        private void AddBothQueues(List<string> schoolCodes)
        {
            string queue;
            bool queueAdded;
            if (!Step.KORGLNDR && Borrower.Step < FinalReview.RecoveryStep.KORGLNDR)
            {
                queue = "KORGLNDR";
                queueAdded = RI.AddQueueTaskInLP9O(Borrower.Demos.Ssn, queue);
                Borrower.UpdateStep(DA, FinalReview.RecoveryStep.KORGLNDR);
            }
            if (!Step.KSCHLLTR && Borrower.Step < FinalReview.RecoveryStep.KSCHLLTR)
            {
                if (!Letter.SchoolLetterCreated(schoolCodes))
                    Step.KSCHLLTR = true;
                Borrower.UpdateStep(DA, FinalReview.RecoveryStep.KSCHLLTR);
            }
            if (!Step.ACURINT2 && Borrower.Step < FinalReview.RecoveryStep.ACURINT2)
            {
                queue = "ACURINT2";
                queueAdded = RI.AddQueueTaskInLP9O(Borrower.Demos.Ssn, queue);
                Borrower.UpdateStep(DA, FinalReview.RecoveryStep.ACURINT2);
            }

            if (!Step.KORGLNDR || !Step.KSCHLLTR || !Step.ACURINT2)
                Borrower.TaskAdded = true;

            if (Borrower.Step < FinalReview.RecoveryStep.REF_REV)
            {
                References r = new References(RI, Borrower, DA);
                r.ReviewReferences();
            }
        }

        #endregion

        /// <summary>
        /// Search TD2A for original lender and school contact
        /// </summary>
        private List<string> GetSchoolCode()
        {
            List<string> schoolCodes = new List<string>();
            RI.FastPath($"TX3Z/ITD2A{Borrower.Demos.Ssn}");
            RI.PutText(4, 66, "X", ReflectionInterface.Key.Enter);
            if (RI.ScreenCode == "TDX2C")
                RI.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
            while (RI.ScreenCode == "TDX2D" && RI.MessageCode != "90007")
            {
                if (RI.GetText(13, 31, 8).ToDate() < Borrower.StartDate)
                    break;
                string arc = RI.GetText(13, 2, 5);
                string comment = "";
                if (arc == "S7LA9")
                    Step.KORGLNDR = true;
                else if (arc.IsIn("KLSLT", "KLSCH"))
                {
                    if (RI.CheckForText(13, 2, "KLSCH"))
                        Step.KSCHLLTR = true;
                    else if (RI.CheckForText(13, 2, "KLSLT") && schoolCodes.Count == 0)
                    {
                        for (int i = 17; i < 22; i++)
                            comment += RI.GetText(i, 2, 79);
                        List<string> sc = comment.Replace(",", " ").Replace(".", " ").Split(' ').Where(p => p.IsNumeric() && p.Length == 8).ToList();
                        schoolCodes.AddRange(sc);
                    }
                }
                NextPage();
            }
            return schoolCodes;
        }

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
                WriteLine("message");
            }
            Borrower.UpdateStep(DA, step);
        }
    }
}