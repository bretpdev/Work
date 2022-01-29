using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using Uheaa.Common;

namespace OPSWebEntry
{
    public static class OPSManager
    {
        public static event SimpleHandler StatusChanged;
        public delegate void SimpleHandler();

        static OPSManager()
        {
            StatusChanged += () => { };
            worker = GenerateWorker();
        }

        private static OPSStates status = OPSStates.NotRunning;
        public static OPSStates Status
        {
            get
            {
                return status;
            }
            internal set
            {
                status = value;
                StatusChanged();
            }
        }

        public static void Toggle()
        {
            if (Status == OPSStates.NotRunning)
                Start();
            else
                Stop();
        }

        public static void Start()
        {
            Status = OPSStates.Waiting;
            StartProcessing();
            ActivityLog.LogNotification("START");
        }

        public static void Stop()
        {
            Status = OPSStates.NotRunning;
            AbortProcessing();
            ActivityLog.LogNotification("STOP");
        }

        internal static Properties.Settings Settings { get { return Properties.Settings.Default; } }

        private static BackgroundWorker worker = new BackgroundWorker();
        private static void StartProcessing()
        {
            worker.RunWorkerAsync();
        }

        private static void AbortProcessing()
        {
            worker.CancelAsync();
            worker = GenerateWorker();
        }

        private static BackgroundWorker GenerateWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += (o, ea) =>
            {
                if (ea.Error != null)
                    MessageBox.Show(ea.Error.ToString());
            };
            worker.DoWork += (o, ea) =>
            {
                Server server = new Server();
                while (!worker.CancellationPending)
                {
                    List<OPSPayment> payments = OPSPayment.GetPendingPayments();
                    ProcessPayments(server, payments, worker);
                    if (Status == OPSStates.Processing) Status = OPSStates.Waiting;
                    Sleep.For(20).Seconds();
                }
            };
            return worker;
        }

        public static void ProcessPayments(Server server, List<OPSPayment> payments, BackgroundWorker worker = null)
        {
            if (payments.Count > 0)
            {
                ActivityLog.LogNotification(payments.Count + " pending payments found.");
                Status = OPSStates.Processing;
                ActivityLog.LogNotification("Attempting Login...");
                bool login = server.Login(Settings.Username, Settings.Password);
                if (!login)
                {
                    ActivityLog.LogError("Invalid login credentials.");
                    Stop();
                    return;
                }
                ActivityLog.LogNotification("Login Successful.");
                foreach (OPSPayment payment in payments)
                {
                    if (worker != null && worker.CancellationPending)
                        return;
                    bool success = true;
                    server = new Server();
                    server.Login(Settings.Username, Settings.Password);

                    bool load = server.LoadAccount(payment.SSN, payment.DOB);
                    success = success && load;
                    if (load)
                    {
                        Server.AccountType checksave = new string[] { "S", "Savings", "Saving" }.Contains(payment.AccountType) ? Server.AccountType.Saving : Server.AccountType.Checking;
                        bool posted = server.PostPayment(payment.Amount, payment.EffectiveDate, payment.ABA, payment.BankAccountNumber, checksave, payment.AccountHolderName);
                        success = success && posted;
                    }
                    if (success)
                    {
                        ActivityLog.LogNotification(string.Format("Successfully Processed: Row #{0}, SSN {1}, Name {2}", payment.ID, payment.SSN, payment.Name));
                        payment.MarkAsProcessed();
                        ErrorHelper.SuccessProcessingPayment(payment, server.ResultLog);
                    }
                    else
                    {
                        payment.MarkAsSkipped();
                        ErrorHelper.ErrorProcessingPayment(payment, server.ResultLog);
                    }
                }
            }
        }
    }

    public enum OPSStates
    {
        [Description("Not Running")]
        NotRunning,
        [Description("Awaiting New Payments")]
        Waiting,
        [Description("Processing Payments")]
        Processing,
    }
}
