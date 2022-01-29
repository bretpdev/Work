using MDIntermediary;
using MDIntermediary.PromiseToPay;
using Microsoft.SqlServer.Server;
using Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{
    public partial class UheaaHomePage : GenericScriptAndServicesEnabled, IHomePage
    {
        private HomePageSessionInteractionCoordinator SessionInteractor { get; set; }
        private const string ADDED_CANCEL_TEXT = "Cancel ";
        public UheaaHomePage()
        {
            InitializeComponent();
        }

        public UheaaHomePage(UheaaBorrower borrower) : base("UHEAA Customer Services", borrower,"",DataAccessHelper.Region.Uheaa)
        {
            InitializeComponent();

            ChangeTabControlColors(tabControl);
            ChangeTabControlColors(tabControlLegal);
            ChangeTabControlColors(tabControlLoanDetail);
            ChangeTabControlColors(tabControlPayment);
            ChangeTabControlColors(tabControlDefermentAndForbearance);

            //Borrower = borrower;
            //initialize session interactor
            SessionInteractor = new HomePageSessionInteractionCoordinator(Borrower, DataAccessHelper.Region.Uheaa);

            //do session scraping populations and checks, this call uses the SessionInteractor initialized above
            InitializeTextBoxes(borrower);
            InitializeListPanelControls(borrower);

            //bind borrower to applicable controls
            borrowerBindingSource.DataSource = Borrower;
            //populate list of dates 20-day letters were sent
            if(Borrower.DatesOf20DayLettersSent.Count > 0)
            {
                listBoxDates20DayLettersSent.Items.AddRange(Borrower.DatesOf20DayLettersSent.ToArray());
            }
            if(Borrower.LoanStatusListForLegal.Count > 0)
            {
                listBoxLegalStatuses.Items.AddRange(borrower.LoanStatusListForLegal.ToArray());
            }
            InitializeComplaintTracking(Borrower.AccountNumber);

        }

        public Button SaveAndContinue { get => buttonSave; }
        public Button UpdateDemographicsButton { get => buttonUpdateDemographics; }
        public Button ReturnToMainMenuButtons { get => buttonMainMenu; }
        public bool IsVisible { get => Visible; }
        public bool HasValidData { get; set; }

        public void CloseAllForms()
        {
            CloseAllActivityHistoryForms();
        }

        public new void Hide()
        {
            base.Hide();
        }

        public void ShowCustom()
        {
            //bind demographics
            csExtendedHomePageDemographics.DemographicData = Borrower.UpdatedDemographics;

            //Disabling call categorization control to swap to noble for a time, leaving in so that a quick switch back can be made
            if (callCategorizationControl1.Enabled)
            {
                //call categorization
                if (Borrower.AcpResponses.Selection.ActivityCodeSelection == ActivityCode.TelephoneContact)
                {
                    //if inbound call then enable call cat control
                    callCategorizationControl1.Enabled = true;
                }
                else
                {
                    //if outbound call disable call cat control
                    callCategorizationControl1.Enabled = false;
                }
                //initialize call categorization control
                CallsHelper helper = new CallsHelper(DataAccessHelper.TestMode, Borrower.AcpResponses.Selection.CallType == CallType.IncomingCall);
                callCategorizationControl1.LoadHelper(helper);
            }

            //populate alerts
            listViewAlerts.Items.Clear();
            List<Alert> alerts = Borrower.GetAlertList();

            foreach(Alert alert in alerts)
            {
                var item = new ListViewItem(alert.Text);
                listViewAlerts.Items.Add(item);
            }     

            base.Show();
        }


        public new DialogResult ShowDialog()
        {
            //bind demographics
            csExtendedHomePageDemographics.DemographicData = Borrower.UpdatedDemographics;

            //Disabling call categorization control to swap to noble for a time, leaving in so that a quick switch back can be made
            if (callCategorizationControl1.Enabled)
            {
                //call categorization
                if (Borrower.AcpResponses.Selection.ActivityCodeSelection == ActivityCode.TelephoneContact)
                {
                    //if inbound call then enable call cat control
                    callCategorizationControl1.Enabled = true;
                }
                else
                {
                    //if outbound call disable call cat control
                    callCategorizationControl1.Enabled = false;
                }
            }

            //populate alerts
            listViewAlerts.Items.Clear();
            List<Alert> alerts = Borrower.GetAlertList();

            foreach (Alert alert in alerts)
            {
                var item = new ListViewItem(alert.Text);
                listViewAlerts.Items.Add(item);
            }

            return base.ShowDialog();
        }

        private void ChangeTabControlColors(TabControl tabControlToChange)
        {
            foreach (TabPage tab in tabControlToChange.TabPages)
            {
                tab.BackColor = BackColor;
                tab.ForeColor = ForeColor;
            }
        }

        private void InitializeComplaintTracking(string accountNumber)
        {
            //NOTE: Demographics binding occurs in the show dialog  because the demographics form can be continually
            //revisited and adter it is revisited we only call show dialog so the data binding
            //the changes show up on the homepage if the demos are changed during a revisit
            //NOTE: Call Categorization enabling and disabling occurs in show dialog becuase if the 
            //conatct code is changed when demos is revisited by the user the call categoriation control may need to be re-enabled or disable
            //NOTE: The alerts are poulated in show dialog because some of the alerts depend on information provided by the user on the demographics screen
            string location = Path.Combine(EnterpriseFileSystem.GetPath("CodeBase", DataAccessHelper.Region.Uheaa), "CMPLNTRACK");
            if (Directory.Exists(location))
            {
                groupBoxComplaints.Show();
                buttonManageComplaints.Click += (o, ea) =>
                {
                    try
                    {
                        string localLocation = Path.Combine(EnterpriseFileSystem.TempFolder, "MD\\CMPLNTRACK");
                        if (!Directory.Exists(localLocation))
                        {
                            FS.CreateDirectory(localLocation);
                        }
                        foreach (string file in Directory.GetFiles(location))
                        {
                            FS.Copy(file, Path.Combine(localLocation, Path.GetFileName(file)), true);
                        }
                        string mode = DataAccessHelper.TestMode ? "dev" : "live";
                        string args = $"mode:{mode} region:uheaa accountnumber:{accountNumber}";
                        Proc.Start("MDCMPLNTRACK", args);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Unable to open Complaint Tracker, please close existing copies.");
                    }
                };
            }
        }

        private void InitializeTextBoxes(UheaaBorrower borrower)
        {
            //do session scraping populations and checks
            List<PaymentInfo> paymentInfo = SessionInteractor.GetPaymentInformationFromSession();
            textBoxCurrentAmountDue.Text = string.Format("{0:C}", paymentInfo.Select(p => p.CurrentAmountDue).Sum());
            textBoxPaymentsInSuspense.Text = string.Format("{0:C}", borrower.PaymentsInSuspense);
            textBoxAmountPastDue.Text = string.Format("{0:C}", paymentInfo.Select(p => p.AmountPastDue).Sum());
            textBoxTotalAmountDue.Text = string.Format("{0:C}", paymentInfo.Select(p => p.TotalAmountDue).Sum());
            decimal lateFees = paymentInfo.Select(p => p.TotalAmountPlusLateFees - p.TotalAmountDue).Sum();
            textBoxLateFees.Text = string.Format("{0:C}", lateFees);
            textBoxTotalAmountPlusLateFees.Text = string.Format("{0:C}", paymentInfo.Select(p => p.TotalAmountPlusLateFees).Sum());
            textBoxPayOffDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            textBoxPayOffAmount.Text = string.Format("{0:C}", SessionInteractor.GetPayoffInformation(DateTime.Today));

            string payoff = SessionInteractor.GetPayoffInformation(DateTime.Today);
            if (payoff != "NA") //NA is returned when a payoff amount can't be gathered from the system
            {
                textBoxInterest.Text = string.Format("{0:C}", payoff.ToDecimal() - Borrower.Principal - lateFees);
            }
            if (textBoxPayOffAmount.Text == "NA") //NA is returned when a payoff amount can be gathered from the system.
            {
                //lock payoff functionality if it can't be gathered from the system.
                textBoxPayOffDate.Clear();
                textBoxPayOffDate.ReadOnly = true;
                textBoxPayOffAmount.Clear();
            }
        }

        private void InitializeListPanelControls(UheaaBorrower borrower)
        {
            foreach (ServicingLoanDetail loan in Borrower.CompassLoans)
            {
                flowLayoutPanelCompassLoanDetail.Controls.Add(new ServicingLoanDisplay(loan));
                flowLayoutPanelBorrowerBenefits.Controls.Add(new BorrowerBenefitsDisplay(loan));
            }

            //populate loan program list
            listBoxLoanProgram.Items.AddRange(Borrower.LoanProgramsDistinctList.ToArray());
            //populate loan status list
            listBoxStatuses.Items.AddRange(Borrower.LoanStatusDistinctList.ToArray());
            //populate RPS Types
            listBoxRPSTypes.Items.AddRange(Borrower.RPSTypes.ToArray());
            //populate bill types
            listBoxBillTypes.Items.AddRange(Borrower.BillTypes.ToArray());
            //populate Bill information
            foreach (Bill item in Borrower.Bills)
            {
                var listViewItem = new ListViewItem(new string[] { item.Due, item.Billed, item.Satisfied, item.DateSatisfied });
                listViewBillInfo.Items.Add(listViewItem);
            }

            //populate financial transaction history
            PopulateFinancialTransactionHistoryData(Borrower.FinancialTransactionHistory);
            //populate system list for activity history combo box
            toolStripComboBoxSystem.Items.AddRange(Enum.GetNames(typeof(ActivityComments.AESSystem)));
            toolStripComboBoxSystem.SelectedItem = "Compass";

            //collect the last five comments
            activityCommentsLastFive.PopulateComments(ActivityCommentGatherer.DaysOrNumberOf.NumberOf, 5, false, Borrower.SSN, ActivityComments.AESSystem.Compass, DataAccessHelper.Region.Uheaa);
            //bind reference and authorized third party data
            foreach (Reference reference in Borrower.References)
            {
                ReferenceControl control = new ReferenceControl(reference);
                flowLayoutPanelReferenceData.Controls.Add(control);
            }

            //bind deferment and forbearance data
            foreach (DefermentForbearance df in Borrower.DefermentsAndForbearences)
            {
                flowLayoutPanelDefermentAndForbearance.Controls.Add(new DefermentAndForbearanceDispaly(df));
            }

            //bind ACH information
            if (Borrower.ACHData != null)
            {
                aCHInformationBindingSource.DataSource = Borrower.ACHData;
                //populate ACH loan level data
                PopulateACHLoanLevelData(listViewLoanInfoACH, Borrower.ACHData.LoansOnACH);
                PopulateACHLoanLevelData(listViewLoanInfoNoACH, Borrower.ACHData.LoansNotOnACH);
            }

            //bind the employer demographic information if there is any
            if (borrower.EmployerDemo != null)
            {
                employerDemoDisplay.Demos = borrower.EmployerDemo;
            }
        }

        public string GetMonthsLeftForDefermentOrForbearance(DefermentForbearance df)
        {
            return SessionInteractor.GetDefermentOrFobearanceMonthsLeft(df);
        }

        //rolls payment data up to the transaction level, sorts the transactions and populates rolled up information into the list view
        private void PopulateFinancialTransactionHistoryData(List<FinancialTransaction> paymentData)
        {
            //roll up to transaction level
            List<FinancialTransactionWithLoanLevelDetail> finalTransactionData = new List<FinancialTransactionWithLoanLevelDetail>();
            foreach (FinancialTransaction transaction in paymentData)
            {
                int matchIndex = GetIndexOfExistingTransaction(finalTransactionData, transaction.TransactionType, transaction.EffectiveDate, transaction.PostedDate, transaction.ReversalReason);
                if (matchIndex >= 0)
                {
                    finalTransactionData[matchIndex].AppliedPrincipal += transaction.AppliedPrincipal;
                    finalTransactionData[matchIndex].AppliedInterest += transaction.AppliedInterest;
                    finalTransactionData[matchIndex].AppliedLateFee += transaction.AppliedLateFee;
                    finalTransactionData[matchIndex].TransactionAmount += transaction.TransactionAmount;
                    finalTransactionData[matchIndex].LoanLevelTransactions.Add(transaction);
                }
                else
                {
                    FinancialTransactionWithLoanLevelDetail rolledUpTransaction = new FinancialTransactionWithLoanLevelDetail();
                    rolledUpTransaction.AppliedInterest = transaction.AppliedInterest;
                    rolledUpTransaction.AppliedLateFee = transaction.AppliedLateFee;
                    rolledUpTransaction.AppliedPrincipal = transaction.AppliedPrincipal;
                    rolledUpTransaction.EffectiveDate = transaction.EffectiveDate;
                    rolledUpTransaction.PostedDate = transaction.PostedDate;
                    rolledUpTransaction.ReversalReason = transaction.ReversalReason;
                    rolledUpTransaction.TransactionAmount = transaction.TransactionAmount;
                    rolledUpTransaction.TransactionType = transaction.TransactionType;
                    rolledUpTransaction.LoanLevelTransactions.Add(transaction);
                    finalTransactionData.Add(rolledUpTransaction);
                }
            }
            foreach (FinancialTransactionWithLoanLevelDetail transaction in finalTransactionData)
            {
                flowLayoutPanelPaymentHistory.Controls.Add(new PaymentHistoryDisplay(transaction));
            }
        }

        private int GetIndexOfExistingTransaction(List<FinancialTransactionWithLoanLevelDetail> transactionList, string transactionType, string effectiveDate, string postedDate, string reversalReason)
        {
            FinancialTransactionWithLoanLevelDetail existingTransaction = transactionList.Where(p => p.TransactionType == transactionType && p.EffectiveDate == effectiveDate && p.PostedDate == postedDate && p.ReversalReason == reversalReason).SingleOrDefault();
            if (existingTransaction == null)
            {
                return -1;
            }
            else
            {
                return transactionList.IndexOf(existingTransaction);
            }
        }

        private void PopulateACHLoanLevelData(ListView listViewToPopulate, List<ACHLoanData> data)
        {
            foreach (var result in data)
            {
                listViewToPopulate.Items.Add(new ListViewItem(new string[] { result.LoanSequenceNumber.ToString(), result.FirstDisbursementDate, result.LoanType }));
            }
        }

        public override void EventHandlerForMenuItems(object sender, System.EventArgs e)
        {
            ScriptAndServiceMenuItem senderItem = ((ScriptAndServiceMenuItem)sender);
            if (senderItem.gsData["InternalOrExternal"].ToString() == "External")
            {
                base.EventHandlerForMenuItems(sender, e);
                if (senderItem.gsData["ToBeCalledAtEnd"].ToString().ToUpper() == "TRUE")
                {
                    //mark that they have run part of it and disable it so it will finish running when save and continue is clicked
                    senderItem.Checked = true;
                    senderItem.Enabled = false;
                }
            }
            else if (senderItem.gsData["InternalOrExternal"].ToString() == "Internal")
            {
                if (senderItem.gsData["DisplayName"].ToString() == "30 Days" ||
                    senderItem.gsData["DisplayName"].ToString() == "60 Days" ||
                    senderItem.gsData["DisplayName"].ToString() == "90 Days" ||
                    senderItem.gsData["DisplayName"].ToString() == "All")
                {
                    RunActivityHistory(senderItem.gsData["DataForFunctionCall"].ToString().ToInt(), "Compass", DataAccessHelper.Region.Uheaa);
                }
                else if (senderItem.gsData["SubToBeCalled"].ToString() == "AddLetterARC")
                {
                    //keep track of letter arcs to be added
                    if (senderItem.Checked == true)
                    {
                        senderItem.Checked = false;
                    }
                    else
                    {
                        senderItem.Checked = true;
                    }
                }
                else if (senderItem.gsData["SubToBeCalled"].ToString() == "ReQueueTask")
                {
                    ReQueueProcessor reQueuer = new ReQueueProcessor(Borrower.SSN);
                    reQueuer.Process();
                    Focus();
                }
                else if (senderItem.gsData["SubToBeCalled"].ToString() == "RePrintBill")
                {
                    SessionInteractor.ReprintBill();
                }
            }
            else if (senderItem.gsData["InternalOrExternal"].ToString() == ".NET DLL")
            {
                base.EventHandlerForMenuItems(sender, e);
                if (senderItem.gsData["ToBeCalledAtEnd"].ToString().ToUpper() == "TRUE")
                {
                    //mark that they have run part of it and disable it so it will finish running when save and continue is clicked
                    senderItem.Checked = true;
                    senderItem.Enabled = false;
                }
            }
            else
            {
                base.EventHandlerForMenuItems(sender, e);
            }
        }

        private void textBoxPayOffDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //enter key
            {
                if (textBoxPayOffDate.Text.ToDateNullable().HasValue)
                {
                    DateTime payoffDate = textBoxPayOffDate.Text.ToDate();
                    textBoxPayOffDate.Text = payoffDate.ToString("MM/dd/yyyy");
                    Processing.MakeVisible();
                    textBoxPayOffAmount.Text = string.Format("{0:C}", SessionInteractor.GetPayoffInformation(payoffDate));
                    Processing.MakeInvisible();
                }
                else
                {
                    textBoxPayOffDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    textBoxPayOffDate.Focus();
                }
            }
        }

        #region Main Form Buttons
        private void UheaaHomePage_FormClosing(object sender, FormClosingEventArgs e)
        {
            BrwInfo411Processor.Close411Form();
            CloseAllForms();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            HasValidData = false;

            //do data validation for homepage
            if (panelBankruptcyOrDeceased.Controls.Count == 1)
            {
                //if control is there then be sure that the data in it is valid
                if (!((ILegalOtherControlValidator)panelBankruptcyOrDeceased.Controls[0]).UserInputIsValid())
                {
                    //switch to the legal-other tab
                    tabControl.SelectedTab = tabControl.TabPages["tabPageLegal"];
                    tabControlLegal.SelectedTab = tabControlLegal.TabPages["tabPageOther"];
                    return; //data is not valid
                }
            }

            ////do call categorization
            if (callCategorizationControl1.Enabled)
            {
                //only update data abse and do data validation if control is enabled
                if (!callCategorizationControl1.RecordCall())
                {
                    tabControl.SelectedTab = tabControl.TabPages["tabPageMain"];
                    MessageBox.Show("Please select a call category reason.");
                    return; // call categorization data needed and wasn't provided
                }
            }

            HasValidData = true;
            CloseAllForms();
            BrwInfo411Processor.Close411Form();
        }

        public void SaveAndContinueProcessing()
        {
            if (Borrower.EndProcessingAfterDemosPage)
            {
                ArcData data = new ArcData(DataAccessHelper.Region.Uheaa);
                data.ScriptId = "MD";
                data.AccountNumber = Borrower.AccountNumber;
                data.Arc = "PRECO";
                data.ArcTypeSelected = ArcData.ArcType.Atd22AllLoans;
                data.Comment = textBoxComments.Text;
                data.ProcessOn = DateTime.Now.AddDays(30);
                data.AddArc();
            }

            //Parse/Process user data on the Bankruptcy/Deceased screens
            RetrieveBankruptcyDeceased();

            //complete call through ACP
            SessionInteractor.CompleteProcessingThroughACP(textBoxComments.Text);

            //Add comments that need to be added
            SaveAndContinueAddComments();

            //Call scripts that need to be called as a result of the form
            SaveAndContinueCallScripts();

            return;
        }

        private void RetrieveBankruptcyDeceased()
        {
            AcpBankruptcyInfo bankruptcyData;
            AcpDeceasedInfo deceasedData;
            //figure out bankruptcy/deceased information from ACP process
            if (panelBankruptcyOrDeceased.Controls.Count == 0)
            {
                bankruptcyData = null;
                deceasedData = null;
            }
            else if (panelBankruptcyOrDeceased.Controls[0] is BankruptcyACPQuestionsControl)
            {
                //bankruptcy data provided
                bankruptcyData = ((BankruptcyACPQuestionsControl)panelBankruptcyOrDeceased.Controls[0]).ResponseData;
                deceasedData = null;
            }
            else
            {
                //deceased data provided
                bankruptcyData = null;
                deceasedData = ((DeceasedACPQuestionsControl)panelBankruptcyOrDeceased.Controls[0]).ResponseData;
            }

            Borrower.AcpResponses.Bankruptcy = bankruptcyData;
            Borrower.AcpResponses.Deceased = deceasedData;

            if (Borrower.AcpResponses.Deceased != null)
            {
                HomePageSessionInteractionCoordinator.AddArcForAllLoans(Borrower.SSN, "G113A", "", "MD", false, DataAccessHelper.Region.Uheaa);
            }
        }

        private void SaveAndContinueCallScripts()
        {
            //call all left over scripts that need calling
            foreach (ScriptAndServiceMenuItem scriptOrLetterOption in Scripts.Values)
            {
                if (scriptOrLetterOption.Checked && scriptOrLetterOption.Enabled == false)
                {
                    BaseScriptRequestProcessor scriptProcessor;
                    if (scriptOrLetterOption.gsData["InternalOrExternal"].ToString() == "External")
                    {
                        scriptProcessor = new SessionScriptProcessor(scriptOrLetterOption, Borrower);
                    }
                    else //.NET DLL
                    {
                        scriptProcessor = new DotNetScriptProcessor(scriptOrLetterOption, Borrower);
                    }
                    scriptProcessor.RunScript(Text, 2); //run number is always 2 at this point
                }
            }
        }

        private void SaveAndContinueAddComments()
        {
            //create list of all comments that need to be created
            List<ActivityCommentToBeAdded> comments = new List<ActivityCommentToBeAdded>();
            //add DMP01 ARC if borrower is over 10 days delinquent
            if (Borrower.AcpResponses.Selection.ActivityCodeSelection == ActivityCode.TelephoneContact &&
                Borrower.AcpResponses.Selection.ContactCodeSelection == ContactCode.FromBorrower &&
                Borrower.DaysDelinquent > 10)
            {
                Uheaa.Common.Scripts.ReflectionInterface ri = new Uheaa.Common.Scripts.ReflectionInterface(SessionInteractionComponents.ReflectionSession);

                if (!ri.BorrowerHasQueue(Borrower.SSN, "VB") && !ri.BorrowerHasQueue(Borrower.SSN, "VR") && !ri.BorrowerHasQueue(Borrower.SSN, "S4") &&
                    !ri.BorrowerHasQueue(Borrower.SSN, "SF") && !ri.BorrowerHasArc(Borrower.AccountNumber, "FBAPV", 14) && !ri.BorrowerHasArc(Borrower.AccountNumber, "DFAPV", 14) &&
                    !ri.BorrowerHasArc(Borrower.AccountNumber, "DMP01", 14) && !ri.BorrowerHasArc(Borrower.AccountNumber, "PHNPE", 14) && !ri.BorrowerHasArc(Borrower.AccountNumber, "PHNPL", 14) &&
                    !ri.BorrowerHasArc(Borrower.AccountNumber, "PHNPN", 14))
                {
                    if (textBoxRPFComments.Text.Trim().Length == 0)
                    {
                        comments.Add(new ActivityCommentToBeAdded("DMP01", ""));
                    }
                }
            }
            //add ACH modification request comment if applicable
            if (textBoxACHModificationRequestComments.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("APUPD", textBoxACHModificationRequestComments.Text));
            }
            //adds repayment plan change comments if applicable
            if (textBoxRepaymentPlanChangeComments.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("GRSEL", textBoxRepaymentPlanChangeComments.Text));
            }
            //add RPF Request Comments if applicable
            if (textBoxRPFComments.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("BRRPF", textBoxRPFComments.Text));
            }
            //add Deferment and forbearance update comment if applicable
            if (textBoxForbearanceComments.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("FDUPD", textBoxForbearanceComments.Text));
            }
            //add bridge deferment comments if applicable
            if (textBoxBridgeDefermentComments.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("TBRDG", textBoxBridgeDefermentComments.Text));
            }
            //add comments for due date change if applicable
            if (textBoxDueDateChangeComment.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("DUEDT", textBoxDueDateChangeComment.Text));
            }
            //add IBR Request comment if applicable
            if (textBoxIBRRequestStatus.Text.Length > 0)
            {
                comments.Add(new ActivityCommentToBeAdded("IBR60", ""));
            }

            //letter creation comments
            foreach (ScriptAndServiceMenuItem scriptOrLetterOption in Scripts.Values)
            {
                if (scriptOrLetterOption.gsData["SubToBeCalled"].ToString() == "AddLetterARC" && scriptOrLetterOption.Checked)
                {
                    comments.Add(new ActivityCommentToBeAdded(scriptOrLetterOption.gsData["DataForFunctionCall"].ToString(), ""));
                }
            }

            //add all activity comments
            SessionInteractor.AddComments(comments);
        }

        private void buttonUpdateDemographics_Click(object sender, EventArgs e)
        {
            BrwInfo411Processor.Close411Form();
            CloseAllForms();
        }

        #endregion

        #region Toolbar Functionality

        private void toolStripButton411_Click(object sender, EventArgs e)
        {
            BrwInfo411Processor.Show411Form(true);
        }

        private void toolStripButtonCheckByPhone_Click(object sender, EventArgs e)
        {
            var scriptProcessor = new DotNetScriptProcessor(Scripts["Check By Phone"], Borrower);
            scriptProcessor.RunScript(Text, 1);
        }

        private void toolStripButtonWipeOut_Click(object sender, EventArgs e)
        {
            FeedbackLinker.BugReportAction(this);
        }

        private void toolStripButtonAskDude_Click(object sender, EventArgs e)
        {
            FaqLinker.ShowFaq(this);
        }

        private void toolStripButtonBrightIdea_Click(object sender, EventArgs e)
        {
            FeedbackLinker.FeatureRequestAction(this);
        }

        private void toolStripButtonActivityHistory30_Click(object sender, EventArgs e)
        {
            RunActivityHistory(30, toolStripComboBoxSystem.Text, DataAccessHelper.Region.Uheaa);
        }

        private void toolStripButtonActivityHistory90_Click(object sender, EventArgs e)
        {
            RunActivityHistory(90, toolStripComboBoxSystem.Text, DataAccessHelper.Region.Uheaa);
        }

        private void toolStripButtonActivityHistory180_Click(object sender, EventArgs e)
        {
            RunActivityHistory(180, toolStripComboBoxSystem.Text, DataAccessHelper.Region.Uheaa);
        }

        private void toolStripButtonAllComments_Click(object sender, EventArgs e)
        {
            RunActivityHistory(0, toolStripComboBoxSystem.Text, DataAccessHelper.Region.Uheaa);
        }

        private void toolStripButtonReQueue_Click(object sender, EventArgs e)
        {
            ReQueueProcessor reQueuer = new ReQueueProcessor(Borrower.SSN);
            reQueuer.Process();
            Focus();
        }

        private void toolStripButtonPhysicalThreat_Click(object sender, EventArgs e)
        {
            string arguments = $"--ticketType Threat --region UHEAA --accountNumber {Borrower.AccountNumber} --name \"{Borrower.FullName}\" --state {Borrower.CompassDemographics.State}";
            if(DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void toolStripButtonSecurityIncident_Click(object sender, EventArgs e)
        {
            string arguments = $"--ticketType Incident --region UHEAA --accountNumber {Borrower.AccountNumber} --name \"{Borrower.FullName}\" --state {Borrower.CompassDemographics.State}";
            if (DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void toolStripButtonTraining_Click(object sender, EventArgs e)
        {
            FaqLinker.ShowTraining(this);
        }

        #endregion

        #region Misc Script Calls From Tabs

        private void buttonAddReference_Click(object sender, EventArgs e)
        {
            DotNetScriptProcessor scriptProcessor = new DotNetScriptProcessor(Scripts["Reference Add"], Borrower);
            scriptProcessor.RunScript(Text, 1);
        }

        //private void buttonAddEmployer_Click(object sender, EventArgs e)
        //{
        //    SessionScriptProcessor scriptProcessor = new SessionScriptProcessor(Scripts["Employer Add"], Borrower);
        //    scriptProcessor.RunScript(Text, 1);
        //}

        private void buttonWaiveLateFees_Click(object sender, EventArgs e)
        {
            SessionScriptProcessor scriptProcessor = new SessionScriptProcessor(Scripts["Waive Late Fees"], Borrower);
            scriptProcessor.RunScript(Text, 1);
        }

        private void buttonReprintMostRecentBill_Click(object sender, EventArgs e)
        {
            SessionInteractor.ReprintBill();
        }

        #endregion

        #region Legal Other Tab Button Event Handlers

        private void buttonClaimsBankruptcy_Click(object sender, EventArgs e)
        {
            panelBankruptcyOrDeceased.Controls.Clear();
            panelBankruptcyOrDeceased.Controls.Add(new BankruptcyACPQuestionsControl());
        }

        private void buttonBorrowerDeceased_Click(object sender, EventArgs e)
        {
            panelBankruptcyOrDeceased.Controls.Clear();
            panelBankruptcyOrDeceased.Controls.Add(new DeceasedACPQuestionsControl());
        }

        #endregion

        #region Payment Schedules And Delinquencies

        private void buttonDueDateChange_Click(object sender, EventArgs e)
        {
            if(textBoxDueDateChangeComment.Text.Length == 0)
            {
                //if a comment hasn't been created yet
                var dueDateChangeControl = new DueDateChangeControl();
                dueDateChangeControl.buttonCreateComment.Click += ButtonCreateDueDateChangeCommentClickHandler;
                flowLayoutPanelSchedulesAndDelinquencies.Controls.Clear();
                flowLayoutPanelSchedulesAndDelinquencies.Controls.Add(dueDateChangeControl);
            }
            else
            {
                //if the user has selected to cancel the due date change
                buttonDueDateChange.Text = buttonDueDateChange.Text.Replace(ADDED_CANCEL_TEXT, "");
                textBoxDueDateChangeComment.Text = "";
            }
        }

        private void ButtonCreateDueDateChangeCommentClickHandler(object sender, EventArgs e)
        {
            if(((DueDateChangeControl)flowLayoutPanelSchedulesAndDelinquencies.Controls[0]).monthCalendar.SelectionStart.Day < 29)
            {
                textBoxDueDateChangeComment.Text = $"Borrower requested due date change to {((DueDateChangeControl)flowLayoutPanelSchedulesAndDelinquencies.Controls[0]).monthCalendar.SelectionStart.ToString("MM/dd/yyyy")}.";
                buttonDueDateChange.Text = $"{ADDED_CANCEL_TEXT}{buttonDueDateChange.Text}";
                flowLayoutPanelSchedulesAndDelinquencies.Controls.Clear();
            }
            else
            {
                WhoaDUDE.ShowWhoaDUDE("The due date can't be on the the 29th, 30th or 31st.  Please try again.", "Bad Due Date");
            }
        }

        private void buttonIBREstimator_Click(object sender, EventArgs e)
        {
            if(textBoxIBRRequestStatus.Text.Length == 0)
            {
                //if a comment hasn't been created yet
                IBRCalculatorControl control = new IBRCalculatorControl(Borrower.CompassLoans);
                control.buttonRequestIDRForbearance.Click += ButtonIBRRequestClickHandler;
                flowLayoutPanelSchedulesAndDelinquencies.Controls.Clear();
                flowLayoutPanelSchedulesAndDelinquencies.Controls.Add(control);
            }
            else
            {
                //if the user has selected to cancel IBR request
                buttonIBREstimator.Text = "IDR Estimator";
                textBoxIBRRequestStatus.Text = "";
            }
        }

        private void ButtonIBRRequestClickHandler(object sender, EventArgs e)
        {
            textBoxIBRRequestStatus.Text = "IDR Forbearance Requested";
            buttonIBREstimator.Text = "Cancel IDR Forbearance";
            flowLayoutPanelSchedulesAndDelinquencies.Controls.Clear();
        }

        private void buttonCalculateRepaymentOptions_Click(object sender, EventArgs e)
        {
            RepaymentOptionsControl control = new RepaymentOptionsControl(Borrower, DataAccessHelper.Region.Uheaa);
            flowLayoutPanelSchedulesAndDelinquencies.Controls.Clear();
            flowLayoutPanelSchedulesAndDelinquencies.Controls.Add(control);
        }


        #endregion

        private void tabControlPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(tabControlPayment.SelectedIndex == 4)
            //{
            //    idrCounter.UpdateTabs(Borrower.AccountNumber);
            //}
        }

        private void buttonForbearanceRequest_Click(object sender, EventArgs e)
        {
            //TODO Chagne to VERFORBUH
            VERFORBUH.Program.MDMain(DataAccessHelper.CurrentMode, DataAccessHelper.CurrentRegion, SessionInteractor.RI, Borrower.AccountNumber);
        }

        private void buttonPendingNoCalls_Click(object sender, EventArgs e)
        {
            PendingNoCallsDataAccess data = new PendingNoCallsDataAccess(Borrower.AccountNumber);
            PendingNoCallsForm form = new PendingNoCallsForm(data);
            form.ShowDialog();
        }

        private void buttonPromiseToPay_Click(object sender, EventArgs e)
        {
            PromiseToPayHelper helper = new PromiseToPayHelper(Borrower, DataAccessHelper.Region.Uheaa);
            string daysToExclude = helper.ShowPromiseToPayForm();
            if (daysToExclude != "")
            {
                bool result = HomePageSessionInteractionCoordinator.AddArcForAllLoans(Borrower.SSN, "BRPTP", daysToExclude, "", false, DataAccessHelper.Region.Uheaa);
                if (!result)
                {
                    MessageBox.Show("Promise to pay arc(BRPTP) NOT added.");
                }
                else
                {
                    MessageBox.Show("Promise to pay arc(BRPTP) added.");
                }
            }
        }

        public void ReBindBorrower(Borrower borrower)
        {
            //bind borrower to applicable controls
            Borrower = borrower;
            borrowerBindingSource.DataSource = Borrower;
        }
    }
}
