using System.Collections.Generic;

namespace IDRUSERPRO
{
    public class ApplicationState
    {
        public BorrowerExistingLoans Loans { get; set; }
        public BorrowerInfo BorData { get; set; }
        public bool NewApp { get; set; }
        public bool UserEnterInvalidData { get; set; }
        public bool MisroutedApp { get; set; }
        public bool NoBalance { get; set; }
        public bool DoneProcessing { get; set; }
        public bool Cancel { get; set; }
        public IdentifiedApplication ExistingApp { get; set; }
        public int AppId { get; set; }
        public bool FirstTimeApp { get; set; }
        public ApplicationData AppInfo { get; set; }
        public string Comment { get; set; }

        public ApplicationState()
        {
            ExistingApp = new IdentifiedApplication();
        }

        /// <summary>
        /// Sets properties
        /// </summary>
        public static ApplicationState UpdateCurrentState(List<Ts26Loans> loans, BorrowerInfo bData, bool newApp, bool userEnterInvalidData, bool misRoutedApp, bool noBalance, bool doneProcessing, bool cancel, IdentifiedApplication existingApp)
        {
            return new ApplicationState()
            {
                Loans = new BorrowerExistingLoans(loans),
                BorData = bData,
                NewApp = newApp,
                MisroutedApp = misRoutedApp,
                NoBalance = noBalance,
                UserEnterInvalidData = userEnterInvalidData,
                DoneProcessing = doneProcessing,
                Cancel = cancel,
                ExistingApp = existingApp
            };
        }
    }
}