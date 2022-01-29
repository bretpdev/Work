using IDRUSERPRO.Object_Classes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace IDRUSERPRO
{
    public partial class ApplicationEntry : Form
    {
        public RecoveryLog Recovery { get; set; }
        List<int> InvalidTabs { get; set; }
        private DataAccess DA { get; set; }
        private ProcessLogRun LogRun { get; set; }
        List<PayStubs> BorEmployers { get; set; }
        List<PayStubs> SpouseEmployers { get; set; }
        public ApplicationState AppState { get; set; }
        public bool FoundExistingApp { get; set; }
        public ApplicationData UserInputedData { get; set; }
        public bool IsPending { get; set; }
        public bool Approved { get; set; }
        public bool WaitingForNSLDS { get; set; }
        public SpouseData Spouse { get; set; } = new SpouseData();
        public int RepaymentTypeId { get; set; }
        public OtherLoanCoordinator LoansWithOtherServicers { get; set; }
        public bool FamilySizeHold { get; set; }
        public int SubStatusId { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public bool NewApp { get; set; }
        private string Ssn { get; set; }
        public ReflectionInterface RI { get; set; }
        public string UserId { get; set; }
        public bool? LoansInSameRegion { get; set; }
        public bool SpouseRequired { get; set; }
        public List<string> ValidStateCodes { get; set; }
        public List<LoanPrograms> LoanPrograms { get; set; }
        int? existingFamilySize = null;
        ErrorAttacher attacher = new ErrorAttacher();
        ControlHelper ch = new ControlHelper();
        const string TwoDecimals = "0.00";

        public ApplicationEntry(ApplicationState appState, DataAccess da, ProcessLogRun logRun, ReflectionInterface ri, string userId, RecoveryLog recovery)
        {
            InitializeComponent();
            Recovery = recovery;
            DA = da;
            LogRun = logRun;
            AppState = appState;
            NewApp = appState.NewApp;
            InvalidTabs = new List<int>();
            ApplicationInformationControl.AccountNumber = appState.BorData.AccountNumber;
            Ssn = appState.BorData.Ssn;
            ValidStateCodes = DA.GetStateCodes();
            LoanPrograms = DA.GetLoanPrograms();
            FoundExistingApp = true;
            BorEmployers = LoadAdoiPaystubs(AppState.AppId, false);
            SpouseEmployers = LoadAdoiPaystubs(AppState.AppId, true);
            SetupTabs();
            if (appState.NewApp)
                LoadLoansAtOtherServicer();
            else
                FoundExistingApp = LoadExistingApp();
            existingFamilySize = DA.GetPreviousAppFamilySize(Ssn, AppState.AppId);

            SetComboboxWidth();
            RI = ri;
            UserId = RI.UserId;
            ApplicationInformationControl.Focus();
            ForceConstantValidation();
            LoadingIndicatorsTask = new Task(LoadIndicators);
            LoadingIndicatorsTask.Start();
        }

        private HashSet<Control> ControlsThatHaveHadFocus = new HashSet<Control>();
        private void ForceConstantValidation()
        {
            foreach (var control in ch.GetControlHierarchy(this))
            {
                control.Leave += (o, ea) =>
                {
                    ControlsThatHaveHadFocus.Add(control);
                    RunValidation();
                };

                control.TextChanged += (o, ea) => RunValidation();

                var combo = control as ComboBox;
                if (combo != null)
                    combo.SelectedIndexChanged += (o, ea) => RunValidation();

                var checkbox = control as CheckBox;
                if (checkbox != null)
                    checkbox.CheckedChanged += (o, ea) => RunValidation();

                var tabControl = control as TabControl;
                if (tabControl != null)
                    tabControl.TabIndexChanged += (o, ea) => RunValidation();
            }
        }

        /// <summary>
        /// Call all the methods to load the data to each tab
        /// </summary>
        private void SetupTabs()
        {
            ApplicationInformationControl.LoadControl(AppState.MisroutedApp, AppState.BorData.Ssn, DA.UserIsSupervisor(), AppState.NewApp, DA);
            PlanSetup();
            HouseholdProcess();
            IncomeProcess();
            LoanProcess();
            DefermentProcess();
            StatusProcess();
        }

        private bool LoadExistingApp()
        {
            ApplicationData appData = DA.GetExistingAppData(AppState.AppId);

            Spouse = DA.GetExistingSpouse(appData.SpouseId) ?? new SpouseData();
            AppState.Loans = new BorrowerExistingLoans(DA.GetExistingLoans(appData.ApplicationId));
            if (appData == null)
            {
                MessageBox.Show("The borrower selected does not have an existing Application.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }


            LoadTabs(appData);

            List<string> statuses = DA.GetExistingAppStatus(appData.ApplicationId);
            if (statuses.Last().SplitAndRemoveQuotes(",").Count > 1)
            {
                var subStatusText = statuses.Last().SplitAndRemoveQuotes(",")[1];
                var item = SubStatus.SingleOrDefault(o => o.SubStatus == subStatusText);
                AppSubstatusCbo.SelectedItem = item;
            }

            return true;
        }

        private void LoadTabs(ApplicationData appData)
        {
            LoadApplication(appData);
            LoadPlan(appData);
            LoadLoansAtOtherServicer(appData);
            LoadHousehold(appData);
            LoadIncome(appData);
            LoadLoan(appData);
            LoadDeferment(appData);
            LoadStatus(appData);
            BorrowerIncomeInformation.SetVisibility();
            SpouseIncomeInformation.SetVisibility();
        }

        #region Application
        /// <summary>
        /// Loads the application tab data for existing data
        /// </summary>
        private void LoadApplication(ApplicationData appData)
        {
            ApplicationInformationControl.ApplicationReceivedDate = appData.ReceivedDate;
            if (appData.EApplicationId.IsPopulated() && appData.EApplicationId.Trim().Length == 9)
                ApplicationInformationControl.CodIdMaxLength = 9;
            ApplicationInformationControl.CodId = appData.EApplicationId;
            ApplicationInformationControl.ApplicationId = appData.ApplicationId;
            ApplicationInformationControl.AwardId = appData.AwardId;
            ApplicationInformationControl.ApplicationSourceId = appData.ApplicationSourceId;
        }

        /// <summary>
        /// Gets all the data provided in the Application tab and adds it to the UserInputData object that will be sent to the database.
        /// </summary>
        private void UpdateApplication()
        {
            UserInputedData.ApplicationSourceId = ApplicationInformationControl.ApplicationSourceId;
            if (UserInputedData.ApplicationSourceId == null)
                UserInputedData.ApplicationSourceId = ApplicationInformationControl.CodId.IsPopulated() ? 2 : 1; // 1 = Paper, 2 = Electronic
            UserInputedData.EApplicationId = ApplicationInformationControl.CodId?.Trim();
            UserInputedData.ReceivedDate = ApplicationInformationControl.ApplicationReceivedDate;
            UserInputedData.Active = !ApplicationInformationControl.Inactive;
            UserInputedData.AwardId = ApplicationInformationControl.AwardId;
        }
        #endregion

        #region Plan
        // The PlanTypes will be used to determine whicy plans were selected by the borrower
        List<RepaymentPlanReason> PlanReasons;
        /// <summary>
        /// Adds data to any controls on the Plan tab.
        /// </summary>
        private void PlanSetup()
        {
            List<string> dates = new List<string>();
            for (int i = 1; i < 29; i++)
                dates.Add(i.ToString());
            dates.Insert(0, "");
            PlanDueDate.DataSource = dates;

            using (ch.TemporarilyDisableEvent(RequestReasonCbo, RequestReasonCbo_SelectedIndexChanged))
            {
                PlanReasons = DA.GetRepaymentPlanReasons();
                PlanReasons.Insert(0, new RepaymentPlanReason() { RepaymentPlanReasonId = 0, RepaymentPlanReasonDescription = "" });
                RequestReasonCbo.ValueMember = "RepaymentPlanReasonId";
                RequestReasonCbo.DisplayMember = "RepaymentPlanReasonDescription";
                RequestReasonCbo.DataSource = PlanReasons;
            }
        }

        /// <summary>
        /// Loads the data in the AppData object for existing apps
        /// </summary>
        private void LoadPlan(ApplicationData appData)
        {
            using (ch.TemporarilyDisableEvent(RequestReasonCbo, RequestReasonCbo_SelectedIndexChanged))
            {
                RequestReasonCbo.SelectedValue = appData.RepaymentPlanReason ?? 0;
                PlanDueDate.Text = appData.DueDateRequested?.Trim() ?? "";

                if (TypeRequested != null)
                    StatusRequestedPlanLbl.Text = TypeRequested.Where(p => p.RepaymentPlanTypeRequestedId == appData.RepaymentPlanTypeRequested).FirstOrDefault()?.RepaymentPlanTypeRequestedDescription;
            }
        }

        /// <summary>
        /// Gets all the data provided in the Plan tab and adds it to the UserInputData object that will be sent to the database.
        /// </summary>
        private void UpdatePlan()
        {
            UserInputedData.RepaymentPlanReason = RequestReasonCbo.SelectedValue.ToString().ToIntNullable();
            UserInputedData.DueDateRequested = PlanDueDate.Text;
            UserInputedData.JointRepaymentPlan = null;
            UserInputedData.RepaymentPlanTypeRequested = null;
            UserInputedData.RepaymentPlanDateEntered = DateTime.Now.Date;
        }
        #endregion

        #region Household
        /// <summary>
        /// Adds data to any controls in the Household tab
        /// </summary>
        private void HouseholdProcess()
        {
            using (ch.TemporarilyDisableEvent(MaritalStatusCbo, MaritalStatusCbo_SelectedIndexChanged))
            using (ch.TemporarilyDisableEvent(FilingStatusCbo, MaritalFilingSpouse_DataChanged, "SelectedIndexChanged"))
            {
                List<MaritalStatus> mStatus = DA.GetMaritalStatus();
                mStatus.Insert(0, new MaritalStatus());
                MaritalStatusCbo.DisplayMember = "Status";
                MaritalStatusCbo.ValueMember = "MaritalStatusId";
                MaritalStatusCbo.DataSource = mStatus;

                FilingStatusCbo.DisplayMember = "FilingStatusDescription";
                FilingStatusCbo.ValueMember = "FilingStatusCode";

                List<FilingStatus> statuses = DA.GetFilingStatuses();
                statuses.Insert(0, new FilingStatus());
                FilingStatusCbo.DataSource = statuses;
            }
        }

        /// <summary>
        /// Loads the existing app data to the controls
        /// </summary>
        private void LoadHousehold(ApplicationData appData)
        {
            using (ch.TemporarilyDisableEvent(Children, MaritalFilingSpouse_DataChanged, "TextChanged"))
            using (ch.TemporarilyDisableEvent(Dependents, MaritalFilingSpouse_DataChanged, "TextChanged"))
            using (ch.TemporarilyDisableEvent(FilingStatusCbo, MaritalFilingSpouse_DataChanged, "SelectedIndexChanged"))
            using (ch.TemporarilyDisableEvent(MaritalStatusCbo, MaritalStatusCbo_SelectedIndexChanged))
            {
                Children.Text = appData.NumberOfChildren?.ToString() ?? "0";
                Dependents.Text = appData.NumberOfDependents?.ToString() ?? "0";
                MaritalStatusCbo.SelectedValue = appData.MaritalStatusId ?? 0;
                FilingStatusCbo.SelectedValue = appData.FilingStatusId ?? 0;

                if (LoansWithOtherServicers.SpouseLoans.Any())
                    SpouseExternalLoansBox.SelectedValue = true;

                SpouseFirstName.Text = Spouse.FirstName;
                SpouseMiddleName.Text = Spouse.MiddleName;
                SpouseLastName.Text = Spouse.LastName;
                SpouseDOB.Text = Spouse.BirthDate?.ToString("MM/dd/yyyy");
                SpouseSsn.Text = Spouse.Ssn;
            }
        }


        private void MaritalStatusCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaritalStatusCbo.SelectedIndex == 0)
            {
                FilingStatusCbo.SelectedIndex = -1;
                FilingStatusCbo.Enabled = false;
            }
            else
            {
                FilingStatusCbo.Enabled = true;
            }
            SetHouseholdVisibility();
            SetUserTriggeredHouseholdVisibility();
            BorrowerIncomeInformation.SetVisibility();
        }


        private void SpouseExternalLoansBox_SelectedValueChanged(object sender, EventArgs e)
        {
            SetHouseholdVisibility();
            SetUserTriggeredHouseholdVisibility();
        }

        private void MaritalFilingSpouse_DataChanged(object sender, EventArgs e)
        {
            SetHouseholdVisibility();
            SetUserTriggeredHouseholdVisibility();
        }

        private void SetHouseholdVisibility()
        {
            bool married = MaritalStatusCbo.Text == "Married";
            bool filingJointly = FilingStatusCbo.Text == "Married Filing Jointly";
            bool filingSeparately = FilingStatusCbo.Text == "Married Filing Separately";
            bool blankSelected =  string.IsNullOrWhiteSpace(ProcessedBox.Text);
            bool agiReflects = BorrowerIncomeInformation.AgiReflectsCurrentIncome == true;

            bool spouseExternalLoansVisible = false;
            if (filingJointly)
                spouseExternalLoansVisible = true;
            else if (filingSeparately&& blankSelected)
                spouseExternalLoansVisible = true;

            SpouseExternalLoansBox.Visible = SpouseExternalLoansLabel.Visible = spouseExternalLoansVisible;
            if (!spouseExternalLoansVisible)
                SpouseExternalLoansBox.SelectedValue = null;
            bool spouseHasExternalLoans = SpouseExternalLoansBox.SelectedValue == true && ch.WouldBeVisible(SpouseExternalLoansBox);


            SpouseRequired = spouseHasExternalLoans;

            //Determine if the spouse information should be visible and required
            SpouseGroup.Visible = SpouseRequired;
            SpouseInfoGroup.Visible = SpouseRequired;

            bool spouseIncomeVisible = false;
            if (filingJointly && !agiReflects)
                spouseIncomeVisible = true;
            else if (filingSeparately && blankSelected)
                spouseIncomeVisible = true;
            var filingStatus = EnumParser.Parse<MaritalStatuses>(MaritalStatusCbo.Text);
            if (filingStatus.IsIn(MaritalStatuses.MarriedSeparated, MaritalStatuses.MarriedNoSpouseInfo))
                spouseIncomeVisible = false;
            SpouseIncomeInformation.Visible = spouseIncomeVisible;

            SpouseSigned.Visible = married;
            if (!SpouseSigned.Visible && SpouseSigned.Checked)
                SpouseSigned.Checked = false;

            if (existingFamilySize.HasValue)
            {
                int? size = LoadLpc()?.FamilySize;
                FamilySizeIncreasedBox.Visible = size - existingFamilySize.Value >= 4;
            }
        }

        private void SetUserTriggeredHouseholdVisibility()
        {
            if (!ch.WouldBeVisible(SpouseIncomeInformation))
            {
                SpouseEmployers.Clear();
                SpouseIncomeInformation.ResetFields();
            }
            if (!ch.WouldBeVisible(SpouseInfoGroup))
            {
                SpouseFirstName.Text = "";
                SpouseMiddleName.Text = "";
                SpouseLastName.Text = "";
                SpouseSsn.Text = "";
                SpouseDOB.Text = "";
                LoansWithOtherServicers.ClearSpouses();
                Spouse = new SpouseData();
            }

        }

        /// <summary>
        /// Adds the text from the spouse controls to the spouse object
        /// </summary>
        private void LoadSpouseData()
        {
            if (UserInputedData != null)
                UserInputedData.SpouseId = Spouse.SpouseId;
            Spouse.FirstName = SpouseFirstName.Text;
            Spouse.MiddleName = SpouseMiddleName.Text;
            Spouse.LastName = SpouseLastName.Text;
            Spouse.Ssn = SpouseSsn.Text;
            Spouse.BirthDate = SpouseDOB.Text.ToDateNullable();
        }

        /// <summary>
        /// Loads all the Household data to the UserInputData object that will be loaded to the database
        /// </summary>
        private void UpdateHousehold()
        {
            UserInputedData.NumberOfChildren = Children.Text.ToIntNullable();
            UserInputedData.NumberOfDependents = Dependents.Text.ToIntNullable();
            UserInputedData.MaritalStatusId = MaritalStatusCbo.SelectedValue.ToString().ToIntNullable();
            UserInputedData.FilingStatusId = FilingStatusCbo.SelectedValue.ToString().ToIntNullable();
            Result.Input.RepaymentPlan = EnumParser.Parse<RepaymentPlans>(ProcessedBox.Text).Value;
            UserInputedData.FamilySize = Result.Input.FamilySize;
            UserInputedData.IncludeSpouseInFamilySize = Result.Input.IncludeSpouseInFamilySize;

            if (SpouseRequired)
                LoadSpouseData();
        }
        #endregion

        #region Income
        List<IncomeSource> IncomeSource;
        private void IncomeProcess()
        {
            IncomeSource = DA.GetIncomeSources();
            var getAppStatusIsApproved = new Func<bool>(() => AppStatusCbo.Text == "Approved");
            var onBorrowerAgiReflectsChanged = new Action(() =>
            {
                SetHouseholdVisibility();
                LoadSubStatus();
            });
            BorrowerIncomeInformation.LoadControl(BorEmployers, ValidStateCodes, IncomeSource, null, GetAdoiCalculator, getAppStatusIsApproved, false, () => SpouseIncomeInformation.SetVisibility(), onBorrowerAgiReflectsChanged);
            SpouseIncomeInformation.LoadControl(SpouseEmployers, ValidStateCodes, IncomeSource, DA.GetFilingStatuses(), GetAdoiCalculator, getAppStatusIsApproved, true, () => BorrowerIncomeInformation.SetVisibility(), null);
        }

        private void LoadIncome(ApplicationData appData)
        {
            BorrowerIncomeInformation.AgiReflectsCurrentIncome = appData.BorrowerAgiReflectsCurrentIncome;
            BorrowerIncomeInformation.TaxesFiled = appData.TaxesFiledFlag;
            BorrowerIncomeInformation.TaxableIncome = appData.BorrowerIncomeTaxable;
            BorrowerIncomeInformation.SupportingDocsRequired = appData.BorrowerSupportingDocumentationRequired;
            BorrowerIncomeInformation.ReceivedDate = appData.BorrowerSupportingDocumentationReceivedDate;
            BorrowerIncomeInformation.AgiFromTaxes = appData.Agi;
            BorrowerIncomeInformation.TaxYear = appData.TaxYear;
            BorrowerIncomeInformation.StateCode = appData.State;


            LoadSpouseIncome(appData);

            BorrowerIncomeInformation.SetVisibility();
        }

        private void LoadSpouseIncome(ApplicationData appData)
        {
            SpouseIncomeInformation.AgiReflectsCurrentIncome = Spouse.AgiReflectsIncome;
            SpouseIncomeInformation.TaxesFiled = Spouse.TaxesFiled;
            SpouseIncomeInformation.TaxableIncome = Spouse.TaxableIncome;
            SpouseIncomeInformation.SupportingDocsRequired = Spouse.SupportingDocsReq;
            SpouseIncomeInformation.ReceivedDate = Spouse.SupportingDocsRecDate;
            SpouseIncomeInformation.AgiFromTaxes = Spouse.SpouseAgi;
            SpouseIncomeInformation.TaxYear = Spouse.TaxYear;
            SpouseIncomeInformation.SpouseFilingStatusId = Spouse.FilingStatusId;
        }

        private void CheckTaxableIncome()
        {
            if (SpouseIncomeInformation.TaxableIncome == false && BorrowerIncomeInformation.TaxableIncome == false && AppStatusCbo.Text == "Approved")
                AppSubstatusCbo.Text = "New Income Driven Application Approved on Self-Certification";
        }


        /// <summary>
        /// Gets all the data provided in the Household tab and adds it to the UserInputData object that will be sent to the database.
        /// </summary>
        private void UpdateIncome()
        {
            var calc = GetAdoiCalculator();
            UserInputedData.State = BorrowerIncomeInformation.StateCode;
            decimal totalIncome = calc.TotalAltIncome;
            totalIncome += BorrowerIncomeInformation.AgiFromTaxes ?? 0;
            totalIncome += SpouseIncomeInformation.AgiFromTaxes ?? 0;
            UserInputedData.TotalIncome = totalIncome;
            UserInputedData.ManuallySubmittedIncome = calc.BorrowerAltIncome;
            UserInputedData.TaxesFiledFlag = BorrowerIncomeInformation.TaxesFiled; // This needs to be set before the tax year and agi. If there are no taxes past 2 years, these fields will be blank.
            UserInputedData.TaxYear = BorrowerIncomeInformation.TaxYear;
            UserInputedData.Agi = BorrowerIncomeInformation.AgiFromTaxes;
            UserInputedData.IncomeSource = BorrowerIncomeInformation.IncomeSource;

            Spouse.SupportingDocsRecDate = SpouseIncomeInformation.ReceivedDate;
            Spouse.SupportingDocsReq = SpouseIncomeInformation.SupportingDocsRequired;
            Spouse.AltIncome = calc.SpouseAltIncome;
            Spouse.TaxesFiled = SpouseIncomeInformation.TaxesFiled;
            Spouse.FilingStatusId = SpouseIncomeInformation.SpouseFilingStatusId;
            Spouse.SpouseAgi = SpouseIncomeInformation.AgiFromTaxes;
            Spouse.StateCode = SpouseIncomeInformation.StateCode;
            Spouse.TaxYear = SpouseIncomeInformation.TaxYear;
            Spouse.TaxesFiled = SpouseIncomeInformation.TaxesFiled;
            var maritalStatus = EnumParser.Parse<MaritalStatuses>(MaritalStatusCbo.Text);
            Spouse.SeparatedFromSpouse = null;
            Spouse.AccessSpouseIncome = null;
            if (maritalStatus == MaritalStatuses.MarriedSeparated)
            {
                Spouse = new SpouseData(); //reset all fields
                Spouse.SeparatedFromSpouse = true;
            }
            else if (maritalStatus == MaritalStatuses.MarriedNoSpouseInfo)
            {
                Spouse = new SpouseData(); //reset all fields
                Spouse.AccessSpouseIncome = false;
            }
            else if (maritalStatus == MaritalStatuses.Married)
            {
                Spouse.SeparatedFromSpouse = false;
                Spouse.AccessSpouseIncome = true;
            }
        }
        #endregion

        #region ADOI
        protected AdoiCalculator GetAdoiCalculator()
        {
            return new AdoiCalculator(GetBorrowerAdoi(), GetSpouseAdoi());
        }

        protected AdoiIncomeInput GetBorrowerAdoi()
        {
            return BorrowerIncomeInformation.GetAdoiIncomeInput();
        }

        protected AdoiIncomeInput GetSpouseAdoi()
        {
            if (ch.WouldBeVisible(SpouseIncomeInformation))
                return SpouseIncomeInformation.GetAdoiIncomeInput();
            return null;
        }

        private List<PayStubs> LoadAdoiPaystubs(int application_id, bool isSpouse)
        {
            var paystubs = DA.GetApplicationPaystubs(application_id, isSpouse);
            return paystubs;
        }

        private void UpdateAdoi()
        {
            var borr = GetBorrowerAdoi();
            UserInputedData.BorrowerIncomeSource = borr.IncomeSource;
            UserInputedData.BorrowerAgiReflectsCurrentIncome = borr.AgiReflectsCurrentIncome;
            UserInputedData.BorrowerSupportingDocumentationRequired = borr.SupportingDocsRequired;
            UserInputedData.BorrowerSupportingDocumentationReceivedDate = borr.ReceivedDate;
            UserInputedData.BorrowerIncomeTaxable = borr.TaxableIncome;
            UserInputedData.BorrowerStubs = BorEmployers;

            var spouse = GetSpouseAdoi();
            if (spouse != null)
            {
                Spouse.IncomeSourceId = spouse.IncomeSource;
                Spouse.AgiReflectsIncome = spouse.AgiReflectsCurrentIncome;
                Spouse.SupportingDocsReq = spouse.SupportingDocsRequired;
                Spouse.SupportingDocsRecDate = spouse.ReceivedDate;
                Spouse.TaxableIncome = spouse.TaxableIncome;
                UserInputedData.SpouseStubs = SpouseEmployers;
            }
        }
        #endregion

        #region Loans
        List<BorrowerEligibility> Eligibility;
        private void LoanProcess()
        {
            Eligibility = DA.GetBorrowerEligibility();
            Eligibility.Insert(0, new BorrowerEligibility());
            BorEligibilityCbo.DataSource = Eligibility;
            BorEligibilityCbo.ValueMember = "EligibilityId";
            BorEligibilityCbo.DisplayMember = "EligibilityDescription";

            LoansWithOtherServicers = new OtherLoanCoordinator();
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
            {
                BorGradeLevelCbo.Visible = false;
                BorGradeLevelLbl.Visible = false;
            }
        }

        private void LoadLoan(ApplicationData appData)
        {
            if (appData.GradeLevel.IsPopulated())
                BorGradeLevelCbo.Text = appData.GradeLevel == "U" ? "Undergraduate" : "Graduate";

            BorEligibilityCbo.SelectedValue = appData.BorrowerEligibilityId ?? 0;
            ExternalLoansBox.SelectedValue = appData.LoansAtOtherServicers;
        }

        private void LoadLoansAtOtherServicer(ApplicationData appData = null)
        {
            LoadBorrowerOtherLoans(appData);

            if (Spouse?.Ssn != null)
            {
                var loans = DA.GetOtherLoans(appData?.ApplicationId ?? AppState.AppId, true, Spouse.Ssn);
                foreach (OtherLoans loan in loans)
                {
                    loan.ApplicationId = appData?.ApplicationId ?? 0;
                }
                LoansWithOtherServicers.SetSpouseLoans(loans);
            }
        }

        private void LoadBorrowerOtherLoans(ApplicationData appData)
        {
            var loans = DA.GetOtherLoans(appData?.ApplicationId ?? this.AppState.AppId, false, Ssn);
            foreach (OtherLoans loan in loans)
            {
                loan.ApplicationId = appData?.ApplicationId ?? 0;
                loan.SetMonthlyPay();
                loan.SetFfelp(LoanPrograms);
            }
            LoansWithOtherServicers.SetBorrowerLoans(loans);
        }

        private void ExternalLoansBox_SelectedValueChanged(object sender, EventArgs e)
        {
            BorDisplayBtn.Visible = ExternalLoansBox.SelectedValue == true;
            if (ExternalLoansBox.SelectedValue != true)
                LoansWithOtherServicers.ClearBorrowers();
            else
                LoadLoansAtOtherServicer(AppState.AppInfo);
        }

        private void BorDisplayBtn_Click(object sender, EventArgs e)
        {
            using (ExternalLoans loans = new ExternalLoans("Borrower", LoansWithOtherServicers.BorrowerLoans.ToList(), LoanPrograms, Ssn, Spouse?.Ssn, false, DA, null))
            {
                if (loans.ShowDialog() == DialogResult.OK)
                    LoansWithOtherServicers.SetBorrowerLoans(loans.OtherLoansBorrower);
                else
                    LoansWithOtherServicers.ClearBorrowers();

                RunValidation();
            }
        }

        private void SpouseDisplayBtn_Click(object sender, EventArgs e)
        {
            if (SpouseSsn.Text.IsPopulated())
            {
                using (ExternalLoans loans = new ExternalLoans("Spouse", LoansWithOtherServicers.SpouseLoans.ToList(), LoanPrograms, Ssn, SpouseSsn.Text, true, DA, LoansInSameRegion))
                {
                    if (loans.IsPending)
                    {
                        AddSpousePendingNSLDS();
                        return;
                    }
                    if (loans.ShowDialog() == DialogResult.OK)
                    {
                        LoansWithOtherServicers.SetSpouseLoans(loans.OtherLoansSpouse);
                        LoansInSameRegion = loans.LoansInSameRegion;
                    }
                    else
                    {
                        LoansWithOtherServicers.ClearSpouses();
                    }
                    RunValidation();
                }
            }
            else
                ValidateHousehold();
        }

        private void AddSpousePendingNSLDS()
        {
            ArcData data = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = AppState.BorData.AccountNumber,
                Arc = "IDRPN",
                Comment = "Waiting for NSLDS Information to be updated.",
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                ProcessOn = DateTime.Now.AddDays(1),
                RecipientId = "",
                ScriptId = "IDRUSERPRO"
            };

            var tryAddArc = new Action(() =>
            { data.AddArc(); });
            var repeaterResult = Repeater.TryRepeatedly(tryAddArc);
            if (!repeaterResult.Successful)
            {
                LogRun.AddNotification($"Unable to add arc {data.Arc} for account {data.AccountNumber} with comment: {data.Comment}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok("The Application was unable to enter the future dated ARC IDRPN for this pending applcation in order to pend it for spousal NSLDS info.  Please make sure it is added.");
            }
            AppState.DoneProcessing = true;
            this.Close();
        }

        private void BorEligibilityCbo_SelectionChangedCommitted(object sender, EventArgs e)
        {
            if (BorEligibilityCbo.Text.IsPopulated())
            {
                using (ConsolidationLoans consol = new ConsolidationLoans(AppState.Loans.FilteredLoans))
                {
                    this.Hide();
                    consol.ShowDialog();
                    this.Show();
                }
            }
        }

        private void BorEligibilityCbo_Leave(object sender, EventArgs e)
        {
            //BorEligibilityCbo_SelectionChangedCommitted(sender, e);

        }

        /// <summary>
        /// Gets all the data provided in the Loans tab and adds it to the UserInputData object that will be sent to the database.
        /// </summary>
        private void UpdateLoans()
        {
            UserInputedData.BorrowerEligibilityId = Eligibility.Where(p => p.EligibilityDescription == BorEligibilityCbo.Text).FirstOrDefault()?.EligibilityId;
            if (!BorGradeLevelCbo.Text.IsNullOrEmpty())
                UserInputedData.GradeLevel = BorGradeLevelCbo.Text.Substring(0, 1);
            UserInputedData.LoansAtOtherServicers = ExternalLoansBox.SelectedValue;

            if (LoansWithOtherServicers.SpouseLoans.Any())
                Spouse.LoansInSameRegion = LoansInSameRegion;
        }
        #endregion

        #region Deferment/Forbearance
        private void DefermentProcess()
        {
            List<DefForbData> def = DA.GetDefForbOptions();
            def.Insert(0, new DefForbData());
            DefForbCbo.DataSource = def;
            DefForbCbo.DisplayMember = "DefForbOption";
            DefForbCbo.ValueMember = "DefForbId";
        }

        private void LoadDeferment(ApplicationData appData)
        {
            DefForbCbo.SelectedValue = appData.DefForbId ?? 0;
            RequestedRpfBox.Text = (appData.RPF ?? 0).ToString(TwoDecimals);
        }

        private void RequestRpfTxt_TextChanged(object sender, EventArgs e)
        {
            if (RequestedRpfBox.Text.ToDecimal() > 0)
            {
                AppStatusCbo.Text = "Pending";
                AppStatusCbo.Enabled = false;
            }
            else
            {
                AppStatusCbo.Text = "";
                AppStatusCbo.Enabled = true;
            }
        }

        /// <summary>
        /// Gets all the data provided in the Deferment/Forbearance tab and adds it to the UserInputData object that will be sent to the database.
        /// </summary>
        private void UpdateDeferment()
        {
            UserInputedData.DefForbId = DefForbCbo.SelectedValue.ToString().ToInt();
            UserInputedData.RPF = RequestedRpfBox.Text.ToDecimal();
        }
        #endregion

        #region Status
        List<RepaymentPlanTypeRequested> TypeRequested;
        List<RepaymentPlanStatus> PlanStatus;
        private void StatusProcess()
        {
            List<PlanType> types = DA.GetPlanTypes();
            types.Insert(0, new PlanType());

            LoadAppStatus();

            TypeRequested = DA.GetRepaymentPlayTypeRequested();

            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
            {
                IbrPlan.Parent.Controls.Remove(IbrPlan);
                IbrPlan.Location = PlansContainer.Location;
                IbrPlan.Size = PlansContainer.Size;
                PlansContainer.Parent.Controls.Add(IbrPlan);
                PlansContainer.Parent.Controls.Remove(PlansContainer);
            }
        }

        private void LoadAppStatus(bool removeApproved = false)
        {
            using (ch.TemporarilyDisableEvent(AppStatusCbo, AppStatus_SelectedIndexChanged))
            {
                PlanStatus = DA.GetRepaymentPlanStatuses();
                if (removeApproved)
                    PlanStatus = PlanStatus.Where(o => o.Status != "Approved").ToList();
                PlanStatus.Insert(0, new RepaymentPlanStatus());
                var selection = PlanStatus.SingleOrDefault(o => o.Status == AppStatusCbo.Text);
                AppStatusCbo.DataSource = PlanStatus;
                AppStatusCbo.ValueMember = "StatusId";
                AppStatusCbo.DisplayMember = "Status";
                if (selection != null)
                    AppStatusCbo.SelectedItem = selection;
            }
        }

        public LpcResults Result { get; private set; }
        IndicatorsResult InitialIndicators;
        Task LoadingIndicatorsTask;
        private void CalculatePlans()
        {
            WarehouseDataAccess wda = new WarehouseDataAccess(DA.LDA, DataAccessHelper.CurrentRegion);
            var lpcInput = LoadLpc();
            if (lpcInput == null)
                return;
            if (InitialIndicators == null)
                LoadingIndicatorsTask.Wait();
            var indicators = Result?.EligibilityIndicators ?? InitialIndicators;
            foreach (var indicator in indicators.Indicators)
            {
                var eligibleLoans = AppState.Loans.EligibleLoans;
                if (!indicator.LoanSequence.IsIn(eligibleLoans.Select(o => o.LoanSeq.ToInt()).ToArray()))
                    indicator.FutureEligibilityIndicator = "I";
                else
                {
                    var selectedEligibility = (int)BorEligibilityCbo.SelectedValue;
                    var matchingEligibility = Eligibility.SingleOrDefault(o => o.EligibilityId == selectedEligibility);
                    if (!string.IsNullOrWhiteSpace(matchingEligibility?.EligibilityCode))
                        indicator.FutureEligibilityIndicator = matchingEligibility.EligibilityCode.Trim();
                    else
                        indicator.FutureEligibilityIndicator = indicator.EligibilityIndicator;
                }
            }
            LowestPlanCalculator lpc = new LowestPlanCalculator(RI, wda, lpcInput);
            Result = lpc.Calculate(indicators);
            LoadResults();
        }

        private void LoadIndicators()
        {
            var gatherer = new IndicatorGatherer();
            if (!AppState.MisroutedApp)
                InitialIndicators = gatherer.LoadEligibilityIndicators(RI, new WarehouseDataAccess(DA.LDA, DataAccessHelper.CurrentRegion), AppState.BorData.AccountNumber);
            else
                InitialIndicators = new IndicatorsResult() { Indicators = new List<LoanSequenceEligibility>() };
        }

        private void LoadResults()
        {
            LoadIbr();
            LoadIbr2014();
            var appStatusHasApprovedOption = true;
            using (ch.TemporarilyDisableEvent(ProcessedBox, ProcessedBox_SelectedIndexChanged))
            {
                var processedOptions = new string[] { "", "IBR" }.ToList();
                string alreadySelected = ProcessedBox.Text;
                ProcessedBox.DataSource = processedOptions;
                ProcessedBox.Text = alreadySelected;
            }
            IbrPlan.EnablePlan();
            IbrPlan.SelectPlan();
            if (Result.Ibr.Status != LpcResults.ResultStatus.Successful)
                appStatusHasApprovedOption = false;
            LoadAppStatus(!appStatusHasApprovedOption);
        }

        /// <summary>
        /// Adds all the required data to the LpcInput object to run the calculation
        /// </summary>
        private LpcInput LoadLpc()
        {
            var filingStatus = EnumParser.Parse<FilingStatuses>(FilingStatusCbo.Text);
            var maritalStatus = EnumParser.Parse<MaritalStatuses>(MaritalStatusCbo.Text);
            if (filingStatus == null || maritalStatus == null)
                return null;
            var calc = GetAdoiCalculator();
            var incomePercentageFactors = new List<IncomePercentageFactor>();

            return new LpcInput()
            {
                AccountNumber = ApplicationInformationControl.AccountNumber,
                BorrowerAgiFromTaxes = BorrowerIncomeInformation.AgiFromTaxes ?? 0,
                SpouseAgiFromTaxes = SpouseIncomeInformation.AgiFromTaxes ?? 0,
                ExternalLoans = GetExternalLoans(),
                FilingStatus = filingStatus.Value,
                IncomePercentageFactors = incomePercentageFactors,
                MaritalStatus = maritalStatus.Value,
                NumberOfChildren = Children.Text.ToInt(),
                NumberOfDependents = Dependents.Text.ToInt(),
                PovertyGuideline = DA.GetPovertyGuideline(),
                RepaymentPlan = EnumParser.Parse<RepaymentPlans>(ProcessedBox.Text) ?? RepaymentPlans.IBR,
                StateCode = BorrowerIncomeInformation.StateCode,
                BorrowerAltIncome = calc.BorrowerAltIncome,
                SpouseAltIncome = calc.SpouseAltIncome
            };
        }

        /// <summary>
        /// Adds the borrower and spouse other loans an a List<ExternalLoan>
        /// </summary>
        private IEnumerable<ExternalLoan> GetExternalLoans()
        {
            List<ExternalLoan> loans = new List<ExternalLoan>();
            foreach (var item in LoansWithOtherServicers.AllLoans)
            {
                ExternalLoan loan = new ExternalLoan();
                loan.OutstandingPrincipalBalance = item.CalculatedOutstandingBalance;
                loan.OutstandingInterestBalance = 0;  // We don't collect this information, the user adds the interest to the outstanding balance
                loans.Add(loan);
            }
            return loans;
        }

        private void ClearListBoxes()
        {
            IbrPlan.ClearItems();
            Ibr2014Plan.ClearItems();
        }

        private RepaymentPlan[] RepaymentPlanControls
        {
            get
            {
                return new RepaymentPlan[] { IbrPlan };
            }
        }

        private void RepaymentPlan_PlanSelected(RepaymentPlan sender)
        {
            foreach (var plan in RepaymentPlanControls)
                if (plan != sender)
                    plan.DeselectPlan();
            ProcessedBox.Text = sender.PlanTitle;
            RunValidation();
            SetRepaymentTypeId(sender.PlanTitle);
        }

        private void SetRepaymentTypeId(string planTitle)
        {
            switch (planTitle)
            {
                case "IBR":
                case "IBR 2014":
                    if (RequestReasonCbo.Text.IsIn("Annual Recertification", "Recalculation Request"))
                        RepaymentTypeId = 2;
                    else
                        RepaymentTypeId = 1;
                    break;
            }
        }

        private void LoadIbr()
        {
            IbrPlan.LoadPlan(Result.Ibr, Result.EligibilityIndicators.Indicators);
        }

        private void LoadIbr2014()
        {
            Ibr2014Plan.LoadPlan(Result.NewIbr, Result.EligibilityIndicators.Indicators);
        }

        private void BorrowerSigned_CheckedChanged(object sender, EventArgs e)
        {
            if ((SpouseSigned.Enabled && SpouseSigned.Checked) || (!SpouseSigned.Enabled))
                Validation_Click(sender, e);
        }

        private void SpouseSigned_CheckedChanged(object sender, EventArgs e)
        {
            if (BorrowerSigned.Checked)
                Validation_Click(sender, e);
        }

        private void LoadStatus(ApplicationData appData)
        {
            ProcessedBox.Text = "IBR";
            if (appData.RepaymentPlanTypeRequested.HasValue && TypeRequested != null)
                StatusRequestedPlanLbl.Text = TypeRequested.Where(p => p.RepaymentPlanTypeRequestedId == appData.RepaymentPlanTypeRequested).FirstOrDefault().RepaymentPlanTypeRequestedDescription;
            AppStatusCbo.SelectedValue = appData?.RepaymentPlanStatusId ?? 0;
            LoadSubStatus();

            string typeProcessed = DA.GetExistingAppTypeProcessed(AppState.AppId);
            ProcessedBox.DataSource = RepaymentPlanControls.Select(o => o.PlanTitle).ToList();
            ProcessedBox.Text = typeProcessed;
            SetRepaymentTypeId(typeProcessed);
        }

        List<SubStatuses> SubStatus;
        /// <summary>
        /// Updates the AppSubstatus drop down data according to the selected text in the AppStatus drop down
        /// </summary>
        private void AppStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppSubstatusCbo.Enabled = false;
            AppSubstatusCbo.DataSource = null;
            if (AppStatusCbo.Text.IsPopulated())
            {
                AppSubstatusCbo.Enabled = true;
                Approved = AppStatusCbo.Text == "Approved";
                LoadSubStatus();

                if (AppSubstatusCbo.Items.Count > 0)
                    AppSubstatusCbo.DropDownWidth = GetWidth(AppSubstatusCbo);
                CheckTaxableIncome(); //Set substatus if borrower and spouse taxable income is No
            }
        }

        private void LoadSubStatus()
        {
            int status = AppStatusCbo.SelectedValue.ToString().ToInt();
            if (status > 0)
            {
                int codeId = PlanReasons.Where(p => p.RepaymentPlanReasonDescription == RequestReasonCbo.Text).FirstOrDefault().RepaymentPlanReasonId;
                SubStatus = DA.GetSubstatuses(status, status == 1 ? codeId : 1); // If Approved, use the codeID otherwise use 1
                SubStatus.Insert(0, new SubStatuses());
                const string newPlan = "New Plan Request";
                const string changePlan = "Change Plan Request";
                const string recalcRequest = "Recalculation Request";
                if (RequestReasonCbo.Text.IsIn(newPlan, changePlan))
                    SubStatus = SubStatus.Where(o => o.SubStatus != "Renewal/Recalculation request denied borrower is not currently on an IDR plan.").ToList();
                if (RequestReasonCbo.Text.IsIn(newPlan, changePlan, recalcRequest))
                    SubStatus = SubStatus.Where(o => o.SubStatus != "Recertification Received too Early").ToList();
                if (AppStatusCbo.Text == "Approved" && BorrowerIncomeInformation.IncomeSource?.SourceCode == "ALT")
                    SubStatus = SubStatus.Where(o => o.SubStatus != "New Income Driven Application Approved on Tax Documentation").ToList();
                var selection = AppSubstatusCbo.Text;
                AppSubstatusCbo.ValueMember = "SubStatusId";
                AppSubstatusCbo.DisplayMember = "SubStatus";
                AppSubstatusCbo.DataSource = SubStatus;
                AppSubstatusCbo.Text = selection;
            }
        }

        /// <summary>
        /// Uses the logic in the DetermineRepaymentPlanProcessed to see if the borrower requested the selected plan
        /// </summary>
        private bool GetBorrowerRequested()
        {
            return true;
        }

        /// <summary>
        /// Gets all the data provided in the Status tab and adds it to the UserInputData object that will be sent to the database.
        /// </summary>
        private void UpdateStatus()
        {
            UserInputedData.RepaymentTypeProcessedNotSame = false;
            UserInputedData.RepaymentPlanStatusId = PlanStatus.Where(p => p.Status == AppStatusCbo.Text).FirstOrDefault().StatusId;
            UserInputedData.DisclosureDate = Approved ? DA.GetDisclosureDate(ApplicationInformationControl.ApplicationId) : null;
            UserInputedData.RequestedByBorrower = GetBorrowerRequested();
            SetRepaymentTypeId(ProcessedBox.Text);
            SubStatusId = SubStatus.Where(p => p.SubStatus == AppSubstatusCbo.Text).FirstOrDefault().SubStatusId;
        }
        #endregion

        #region TabControls
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            ChangeTabTransparent(e); //Change back to normal color
            if (e.Index == tabs.SelectedTab.TabIndex.ToString().ToInt()) //Change the selected tab color to gray
                ChangeTabGray(e);
            if (InvalidTabs.Contains(e.Index))
                ChangeTabRed(e);
            AddHeaders(e);
        }

        /// <summary>
        /// Since the DrawMode is set to OwnerDrawFixed, the header names need to be written out.
        /// </summary>
        private void AddHeaders(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);
            var color = Color.Black;
            if (e.Index == tabs.SelectedTab.TabIndex.ToString().ToInt())
                color = Color.White;
            g.DrawString(tabs.TabPages[e.Index].Text, new Font(tabs.Font, FontStyle.Bold), new SolidBrush(color), headerRect, sf);
        }

        /// <summary>
        /// Updates the current tab color to red
        /// </summary>
        private void ChangeTabRed(DrawItemEventArgs e)
        {
            if (e.Index.IsIn(InvalidTabs.ToArray()))
            {
                Graphics g = e.Graphics;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);
                SolidBrush sb = new SolidBrush(Color.LightPink);
                g.FillRectangle(sb, e.Bounds);
            }
        }

        private void ChangeTabGray(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);
            SolidBrush sb = new SolidBrush(Color.Gray);
            g.FillRectangle(sb, e.Bounds);
        }

        /// <summary>
        /// Changes the tab color to transparent
        /// </summary>
        private void ChangeTabTransparent(DrawItemEventArgs e)
        {
            if (e.Index.IsIn(InvalidTabs.ToArray()))
            {
                Graphics g = e.Graphics;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);
                SolidBrush sb = new SolidBrush(Color.Transparent);
                g.FillRectangle(sb, e.Bounds);
            }
        }

        /// <summary>
        /// Sets up hotkeys so the user doesn't have to use the mouse
        /// </summary>
        private void ApplicationEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode.ToString().ToLower() == "v") // Alt + v runs validation
                Validation_Click(sender, new EventArgs());

            if ((e.Control && e.KeyCode == Keys.Tab) && (tabs.SelectedIndex < tabs.TabCount)) // CTRL + TAB - Next Step
                NextStep_Click(sender, new EventArgs());
            if ((e.Alt && e.Shift && e.KeyCode == Keys.Tab) && (tabs.SelectedIndex > 0)) // CTRL + SHIFT + TAB - Previous Step
                tabs.SelectedIndex = tabs.SelectedIndex - 1;
        }

        /// <summary>
        /// Sets the focus to the first control in the tab when a new tab is selected
        /// </summary>
        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tabs.SelectedIndex; i++)
            {
                ControlsThatHaveHadFocus.Add(tabs.TabPages[i]);
                foreach (var control in ch.GetControlHierarchy(tabs.TabPages[i]))
                    ControlsThatHaveHadFocus.Add(control);
            }

            RunValidation();
            NextStep.Enabled = true;
            switch (tabs.SelectedIndex)
            {
                case 0:
                    ApplicationInformationControl.Focus();
                    break;
                case 1:
                    RequestReasonCbo.Focus();
                    this.ActiveControl = RequestReasonCbo;
                    break;
                case 2:
                    Children.Focus();
                    break;
                case 3:
                    BorrowerIncomeInformation.Focus();
                    break;
                case 4:
                    BorGradeLevelCbo.Focus();
                    BorEligibilityCbo.Focus();
                    break;
                case 5:
                    DefForbCbo.Focus();
                    break;
                case 6:
                    ClearListBoxes();
                    CalculatePlans();
                    AppStatusCbo.Focus();
                    NextStep.Enabled = false;
                    break;
            }

        }

        /// <summary>
        /// Searches each tab of the tab control to find any combobox to reset the dropdownwidth
        /// </summary>
        private void SetComboboxWidth()
        {
            foreach (TabPage page in tabs.TabPages)
            {
                foreach (Control ctrl in ch.GetControlHierarchy(page))
                {
                    if (ctrl is ComboBox && ((ComboBox)ctrl).Items.Count > 0)
                        ((ComboBox)ctrl).DropDownWidth = GetWidth((ComboBox)ctrl);
                }
            }
        }

        /// <summary>
        /// Gets the length of the longest string in the drop down and sets the width to that length
        /// </summary>
        /// <returns></returns>
        private int GetWidth(ComboBox box)
        {
            int maxWidth = 0;

            foreach (Object o in box.Items)
            {
                string toCheck = "";
                PropertyInfo pInfo;
                Type objectType = o.GetType();
                if (box.DisplayMember.CompareTo("") == 0)
                    toCheck = o.ToString();
                else
                {
                    pInfo = objectType.GetProperty(box.DisplayMember);
                    object value = pInfo.GetValue(o, null);
                    if (value != null && value.ToString().IsPopulated())
                        toCheck = pInfo.GetValue(o).ToString();
                }
                if (TextRenderer.MeasureText(toCheck, box.Font).Width > maxWidth)
                    maxWidth = TextRenderer.MeasureText(toCheck, box.Font).Width;
            }
            return maxWidth == 0 ? box.DropDownWidth : maxWidth;
        }
        #endregion

        private void NextStep_Click(object sender, EventArgs e)
        {
            InvalidTabs.Remove(tabs.SelectedIndex);
            switch (tabs.SelectedIndex)
            {
                case 0:
                    ValidateApplication();
                    break;
                case 1:
                    ValidatePlan();
                    break;
                case 2:
                    ValidateHousehold();
                    break;
                case 3:
                    ValidateIncome();
                    break;
                case 4:
                    ValidateLoans();
                    break;
                case 6:
                    ValidateStatus();
                    break;
            }
            tabs.SelectedIndex = tabs.SelectedIndex + 1;
            this.Refresh();
        }

        /// <summary>
        /// Runs the validation for all tabs and unlocks the Complete button when everything passes
        /// </summary>
        private void Validation_Click(object sender, EventArgs e)
        {
            foreach (var control in ch.GetControlHierarchy(this))
                ControlsThatHaveHadFocus.Add(control);
            RunValidation(true);
        }

        private void Complete_Click(object sender, EventArgs e)
        {
            UserInputedData = new ApplicationData();
            if (!GetAnniversaryDate())
                return;
            UpdateApplication();
            UpdatePlan();
            UpdateHousehold();
            UpdateIncome();
            UpdateLoans();
            UpdateDeferment();
            UpdateStatus();
            UpdateAdoi();

            IsPending = AppStatusCbo.Text == "Pending";
            if (!AppState.MisroutedApp)
            {
                var updatedAwardId = GetOldestAwardId(AppState.Loans.EligibleLoans.Where(q => !q.LoanType.IsIn("DLPLUS", "PLUS", "DLPLGB", "PLUSGB")).ToList());
                if (updatedAwardId.IsNullOrEmpty())
                    updatedAwardId = GetOldestAwardId(AppState.Loans.EligibleLoans);
                if (updatedAwardId.IsNullOrEmpty())
                    updatedAwardId = GetOldestAwardId(AppState.Loans.FilteredLoans);
                UserInputedData.AwardId = updatedAwardId ?? UserInputedData.AwardId;
            }

            var maritalStatus = EnumParser.Parse<MaritalStatuses>(MaritalStatusCbo.Text);
            if (SpouseRequired || ch.WouldBeVisible(SpouseIncomeInformation) || maritalStatus != MaritalStatuses.Single)
            {
                DA.InsertOrUpdateSpouseData(Spouse);
                UserInputedData.SpouseId = Spouse.SpouseId;
            }
            else if (UserInputedData.SpouseId.HasValue)
            {
                DA.DeleteSpouse(UserInputedData.SpouseId.Value);
                UserInputedData.SpouseId = null;
            }


            if (NewApp)
            {
                int? appId = AddNewApp();
                if (appId.HasValue)
                {
                    ApplicationInformationControl.ApplicationId = appId;
                    UserInputedData.ApplicationId = appId;
                    Dialog.Info.Ok("Application Submitted.");
                    this.DialogResult = DialogResult.OK;
                }
                else
                    Dialog.Error.Ok("Unable to insert Application Data successfully.  Please reference PL#" + LogRun.ProcessLogId);
            }
            else
            {
                UserInputedData.ApplicationId = ApplicationInformationControl.ApplicationId;
                if (!UpdateApp())
                    Dialog.Error.Ok("Unable to update Application Data successfully.  Please reference PL#" + LogRun.ProcessLogId);
                else
                {
                    Dialog.Info.Ok("Application Updated.");
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private bool GetAnniversaryDate()
        {
            if (AppSubstatusCbo.Text == "Other - Application Pending Anniversary Date")
            {
                while (AnniversaryDate == null)
                {
                    using (AnnivesaryDate ann = new AnnivesaryDate())
                    {
                        if (ann.ShowDialog() != DialogResult.OK)
                        {
                            if (Dialog.Info.YesNo("Are you sure you want to cancel? You will not be able to move on until you provide the anniversary date."))
                                return false;
                        }
                        else
                            AnniversaryDate = ann.AnniversaryDate;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Updates the application data in the database when changed.
        /// </summary>
        private bool UpdateApp()
        {
            if (!DA.UpdateApp(UserInputedData, UserId))
                return false;

            DA.InsertOtherLoans(LoansWithOtherServicers.SpouseLoans, LoansWithOtherServicers.BorrowerLoans, (int)UserInputedData.ApplicationId);

            RepaymentPlanType repaymentPlanType = DA.GetRepaymentPlanTypeId(UserInputedData.ApplicationId);

            if (RepaymentTypeId != repaymentPlanType.RepaymentTypeId)
                DA.UpdatePlanSelected(repaymentPlanType.RepaymentPlanTypeId, RepaymentTypeId);

            string lastStatus = DA.GetExistingAppStatus(AppState.AppId).Last();
            if (lastStatus.SplitAndRemoveQuotes(",")[0] != AppStatusCbo.Text || lastStatus.SplitAndRemoveQuotes(",")[1] != AppSubstatusCbo.Text)
            {
                DA.InsertStatusHistory(new
                {
                    RepaymentPlanTypeId = repaymentPlanType.RepaymentPlanTypeId,
                    RepaymentPlanStatusMappingId = SubStatusId,
                    CreatedBy = Environment.UserName
                });
            }

            DA.InsertApplicationStatusHistory(UserInputedData.ApplicationId.Value, UserInputedData.Active);

            if (Recovery.RecoveryValue.IsNullOrEmpty())
                Recovery.RecoveryValue = string.Format("{0},Database Updated", UserInputedData.ApplicationId);
            return true;
        }

        /// <summary>
        /// Adds the application data into the session.
        /// </summary>
        private int? AddNewApp()
        {
            int borrowerId = DA.CheckAndInsertBorrowerData(AppState.BorData);
            int? appId = DA.InsertApplicationData(UserInputedData, UserId);
            if (!appId.HasValue)
                return null;

            UserInputedData.ApplicationId = appId;
            AppState.AppId = appId.Value;

            foreach (OtherLoans item in LoansWithOtherServicers.AllLoans)
                item.ApplicationId = appId.Value;



            DA.InsertOtherLoans(LoansWithOtherServicers.SpouseLoans, LoansWithOtherServicers.BorrowerLoans, appId.Value);

            var loansToUpdate = AppState.Loans.AllLoans.ToList();
            if (!loansToUpdate.Any())
            {
                var dummyLoan = new Ts26Loans() { AwardId = UserInputedData.AwardId, AppId = appId.Value, BorrowerId = borrowerId };
                var oldestLoan = AppState.Loans.AllLoans.OrderBy(o => o.DisbDate.ToDate()).FirstOrDefault();
                loansToUpdate.Add(oldestLoan ?? dummyLoan);
            }
            //Add the app id and borrower id generated from the database
            foreach (Ts26Loans item in loansToUpdate)
            {
                item.AppId = appId.Value;
                item.BorrowerId = borrowerId;
            }
            DA.InsertLoanData(loansToUpdate);

            if (RepaymentTypeId > 0)
            {
                int repaymentPlanTypeId = DA.InsertPlanSelected(new { AppId = appId, RepaymentTypeId = RepaymentTypeId });
                DA.InsertStatusHistory(new
                {
                    RepaymentPlanTypeId = repaymentPlanTypeId,
                    RepaymentPlanStatusMappingId = SubStatusId,
                    CreatedBy = Environment.UserName
                });
            }

            DA.InsertApplicationStatusHistory(appId.Value, UserInputedData.Active);
            Recovery.RecoveryValue = string.Format("{0},Database Updated", UserInputedData.ApplicationId);
            return appId;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public string GetOldestAwardId(List<Ts26Loans> loans)
        {
            return loans.FirstOrDefault()?.AwardId;
        }

        #region Validation
        private void RunValidation(bool runAllValidation = false)
        {
            attacher.ClearAllErrors();
            InvalidTabs.Clear();
            int currentTab = 0;
            int validatedTabs = 0;
            var validate = new Action<Action>((validationAction) =>
            {
                var tab = tabs.TabPages[currentTab];
                if (ControlsThatHaveHadFocus.Contains(tab) || runAllValidation)
                {
                    int errorCount = attacher.ErrorCount;
                    validationAction();
                    if (errorCount != attacher.ErrorCount)
                        InvalidTabs.Add(currentTab);
                    validatedTabs++;
                }
                currentTab++;
            });
            validate(ValidateApplication);
            validate(ValidatePlan);
            validate(ValidateHousehold);
            validate(ValidateIncome);
            validate(ValidateLoans);
            validate(ValidateDefForb);
            validate(ValidateStatus);

            bool invalid = attacher.ErrorCount > 0;
            if (validatedTabs < tabs.TabPages.Count)
                invalid = true;  //not all pages were validated
            if (!invalid)
                Complete.Enabled = true;
            else
                Complete.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// Runs the validation for all the fields in the Application tab
        /// </summary>
        public void ValidateApplication()
        {
            ApplicationInformationControl.PerformValidation(SetError, AppState.MisroutedApp, AppState.BorData.Ssn);
        }

        /// <summary>
        /// Validates all the required fields are filled in for the Plan tab
        /// </summary>
        private void ValidatePlan()
        {
            int errorCount = attacher.ErrorCount;
            if (RequestReasonCbo.SelectedIndex <= 0)
                SetError("Requested Reason is required", RequestReasonCbo, RequestReasonLbl);
        }
        /// <summary>
        /// Validates the household data
        /// </summary>
        private void ValidateHousehold()
        {
            if (Children.Text.ToIntNullable() == null)
                SetError("Please enter a valid number.", Children, ChildrenLabel);
            if (Dependents.Text.ToIntNullable() == null)
                SetError("Please enter a valid number.", Dependents, DependentsLabel);

            if (MaritalStatusCbo.SelectedIndex <= 0)
                SetError("Please select a Marital Status", MaritalStatusCbo, MaritalStatusLbl);

            if (FilingStatusCbo.SelectedIndex <= 0)
                SetError("Please select a Filing Status", FilingStatusCbo, FilingStatusLbl);

            if (ch.WouldBeVisible(SpouseExternalLoansBox))
                if (SpouseExternalLoansBox.SelectedValue == null)
                    SetError("Does Spouse have External Loans?", SpouseExternalLoansBox, SpouseExternalLoansLabel);

            if (ch.WouldBeVisible(FamilySizeIncreasedBox))
            {
                string status = AppStatusCbo.Text;
                var invalidSize = FamilySizeIncreasedBox.SelectedIndex < 0;
                if (FamilySizeIncreasedBox.SelectedIndex == 0 && !status.IsIn("Pending", "Denied"))
                    invalidSize = true;
                if (invalidSize) //unconfirmed selection
                {
                    string message = "Family size must be confirmed in order to approve the application.";
                    SetError(message, FamilySizeIncreasedBox);
                    SetError(message, Children, ChildrenLabel);
                    SetError(message, Dependents, DependentsLabel);
                }
            }


            if (SpouseRequired)
            {
                SimpleRequired("First Name", SpouseFirstName, SpouseFirstNameLabel);
                SimpleRequired("Last Name", SpouseLastName, SpouseLastNameLabel);
                if (SpouseSsn.TextLength < 9)
                    SetError("SSN must be 9 digits.", SpouseSsn, SpouseSsnLabel);
                using (var group = ch.Group(SetError, SpouseDOB, SpouseDobLabel))
                {
                    group.SetErrorIf("Date of Birth is required.", dob => dob.Text.ToDateNullable() == null);
                    group.SetErrorIf("Date of Birth must be in the past.", dob => dob.Text.ToDateNullable() > DateTime.Now);
                }
            }
        }

        private void ValidateIncome()
        {
            var borrowerAndSpouseSigned = BorrowerSigned.Checked;
            if (ch.WouldBeVisible(SpouseSigned))
                borrowerAndSpouseSigned &= SpouseSigned.Checked;
            BorrowerIncomeInformation.PerformValidation(SetError, AppSubstatusCbo.Text, borrowerAndSpouseSigned);
            if (ch.WouldBeVisible(SpouseIncomeInformation))
                SpouseIncomeInformation.PerformValidation(SetError, AppSubstatusCbo.Text, borrowerAndSpouseSigned);
        }

        private void ValidateLoans()
        {
            if (BorEligibilityCbo.SelectedIndex <= 0)
                SetError("Please select a Borrower Eligibility.", BorEligibilityCbo, BorEligibilityLbl);

            if (ExternalLoansBox.SelectedValue == null)
                SetError("Please select if Borrower has External Loans", ExternalLoansBox, ExternalLoansLabel);

            using (var group = ch.Group(SetError, BorDisplayBtn))
            {
                group.SetErrorIf("External Loans required", o => !LoansWithOtherServicers.BorrowerLoans.Any() && ExternalLoansBox.SelectedValue == true);
            }

            if (ch.WouldBeVisible(SpouseInfoGroup))
            {
                using (var group = ch.Group(SetError, SpouseDisplayBtn))
                {
                    group.SetErrorIf("External Loans required", o => !LoansWithOtherServicers.SpouseLoans.Any());
                }
            }
        }

        private void ValidateDefForb()
        {
            SimpleRequired("Is the borrower on a Deferment or Forbearance?", DefForbCbo, DefForbLbl);
            if (RequestedRpfBox.Text.ToDecimalNullable() > 99999)
                SetError("Please enter an amount no more than 5 digits prior to the decimal.", RequestedRpfBox, RequestedRpfLabel);
            else if (RequestedRpfBox.Text.ToDecimalNullable() == null)
                SetError("Please enter a valid amount", RequestedRpfBox, RequestedRpfLabel);
            else
            {
                string afterDecimal = RequestedRpfBox.Text.Split('.').Skip(1).SingleOrDefault();
                if (afterDecimal != null && afterDecimal.Length > 2)
                    SetError("Please enter no more than two digits after the decimal place", RequestedRpfBox, RequestedRpfLabel);
            }
        }


        /// <summary>
        /// Validates all the required fields in the Status tab are populated
        /// </summary>
        private void ValidateStatus()
        {
            SimpleRequired("App Status", AppStatusCbo, AppStatusLbl);
            SimpleRequired("App Substatus", AppSubstatusCbo, AppSubstatusLbl);
            string[] exceptions = new string[] { "Waiting for Documentation", "Denial - Application is Missing Signature(s)", "Denial - Other", "Multiple Applications Received/Other Application Processed" };
            string signMessage = string.Format("must Sign unless the application is 'Pending - {0}', 'Denied - {1}', or 'Denied - {2}', or 'Denied - {3}'", exceptions);
            if (!BorrowerSigned.Checked)
            {
                if (!AppSubstatusCbo.Text.IsIn(exceptions))
                    SetError("Borrower " + signMessage, BorrowerSigned);
            }


            if (ch.WouldBeVisible(SpouseSigned) && !SpouseSigned.Checked)
            {
                if (!AppSubstatusCbo.Text.IsIn(exceptions))
                    SetError("Spouse ", SpouseSigned);
            }

            SimpleRequired("Processed", ProcessedBox, StatusProcessedLbl);

        }

        private void SimpleRequired(string fieldName, Control control, Label associatedLabel = null)
        {
            if (control.Text.IsNullOrEmpty())
                SetError(fieldName + " required", control, associatedLabel);
        }
        /// <summary>
        /// Wrapper around the attacher.SetError method to only attach if the user previously had focus
        /// </summary>
        private void SetError(string message, Control control, Label associatedLabel = null)
        {
            if (ControlsThatHaveHadFocus.Contains(control))
                attacher.SetError(message, control, associatedLabel);
            else
                attacher.AdditionalErrorCount++;
        }
        #endregion

        private void RequestReasonCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubStatus();
        }

        private void ProcessedBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var planTitle = ProcessedBox.Text;
            foreach (var plan in RepaymentPlanControls)
            {
                if (plan.PlanTitle == planTitle)
                    plan.SelectPlan();
                else
                    plan.DeselectPlan();
            }
            SetHouseholdVisibility();
        }

        private void RequestedRpfBox_Enter(object sender, EventArgs e)
        {
            RequestedRpfBox.Select(0, RequestedRpfBox.Text.Length);
        }

        private void AppSubstatusCbo_Enter(object sender, EventArgs e)
        {
            AppSubstatusCbo.DroppedDown = true;
        }

        private void AppSubstatusCbo_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void AppSubstatusCbo_MouseUp(object sender, MouseEventArgs e)
        {
            AppSubstatusCbo.DroppedDown = true;
        }

        private void SpouseSsn_TextChanged(object sender, EventArgs e)
        {
            if (SpouseSsn.Text.Length == 9)
            {
                LoadSpouseData();
                LoadLoansAtOtherServicer(AppState.AppInfo);
            }
        }

        private void BorEligibilityCbo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (BorEligibilityCbo.DroppedDown && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                SendKeys.Send("{ENTER}");
            }
        }
    }
}