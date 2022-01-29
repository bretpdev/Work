using MDIntermediary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{
    public partial class DemographicsUI : Form
    {
        public delegate void SelectDemographicsFormFromAnotherThreadDelegate();
        public SelectDemographicsFormFromAnotherThreadDelegate SelectDemographics;

        private Borrower UheaaBorrower { get; set; } = null;
        private Borrower Borrower 
        { 
            get
            {
                if(UheaaBorrower != null)
                {
                    return UheaaBorrower;
                }
                else
                {
                    return null;
                }
               
            }

        }
        private bool CompassAndOnelinkDiffer { get; set; } = false;
        private bool CallsMode { get; set; } = false;
        private bool AddressWarning { get; set; } = false; //this is true if the user has been warned of another valid address
        private ReflectionInterface ri;

        private string IvrResponse { get; set; }
        private bool IvrResponseActivated { get; set; } = false;

        public bool BackButtonClicked = true; //default to true unless save and continue is clicked
        public bool OverrideUheaa = false;

        public DemographicsUI()
        {
            InitializeComponent();
        }

        public DemographicsUI(Borrower uheaaBorrower, bool callsMode, ReflectionInterface ri)
        {
            InitializeComponent();

            this.ri = ri;
            this.UheaaBorrower = uheaaBorrower;

            ////figure out if the demographics are different if the borrower is in both regions
            //if(cornerstoneBorrower != null && uheaaBorrower != null)
            //{
            //    if(cornerstoneBorrower.CompassDemographics.Addr1 != uheaaBorrower.CompassDemographics.Addr1 ||
            //        cornerstoneBorrower.CompassDemographics.Addr2 != uheaaBorrower.CompassDemographics.Addr2 ||
            //        cornerstoneBorrower.CompassDemographics.AltPhone != uheaaBorrower.CompassDemographics.AltPhone ||
            //        cornerstoneBorrower.CompassDemographics.City != uheaaBorrower.CompassDemographics.City ||
            //        cornerstoneBorrower.CompassDemographics.Email != uheaaBorrower.CompassDemographics.Email ||
            //        cornerstoneBorrower.CompassDemographics.OtherEmail != uheaaBorrower.CompassDemographics.OtherEmail ||
            //        cornerstoneBorrower.CompassDemographics.OtherEmail2 != uheaaBorrower.CompassDemographics.OtherEmail2 ||
            //        cornerstoneBorrower.CompassDemographics.OtherPhoneNum != uheaaBorrower.CompassDemographics.OtherPhoneNum ||
            //        cornerstoneBorrower.CompassDemographics.Phone != uheaaBorrower.CompassDemographics.Phone ||
            //        cornerstoneBorrower.CompassDemographics.State != uheaaBorrower.CompassDemographics.State ||
            //        cornerstoneBorrower.CompassDemographics.Zip != uheaaBorrower.CompassDemographics.Zip)
            //    {
            //        CompassAndOnelinkDiffer = true;
            //    }
            //}

            //set the form
            CallsMode = callsMode;
            if(CallsMode)
            {
                activityCommentSelection.CallsMode(UheaaBorrower);
            }
            else
            {
                activityCommentSelection.ProcessingMode(UheaaBorrower);
            }
            activityCommentSelection.SelectionChanged += ActivtyCommentSelection_SelectionChanged;

            SelectDemographics = new SelectDemographicsFormFromAnotherThreadDelegate(SelectDemographicsFromAnotherThread);

            if(callsMode)
            {
                IvrResponse = DataAccess.DA.GetIVRResponse(Borrower.AccountNumber);
            }
        }

        public void ActivtyCommentSelection_SelectionChanged()
        {
            AcpSelectionResult result = activityCommentSelection.Selection;
            
            if(CallsMode && IvrResponseActivated)
            {
                borrowerInfoControl1.RevertChanges();
                IvrResponseActivated = false;
            }    

            if(result != null)
            {
                borrowerInfoControl1.Focus();
                if(result.RecipientTarget != CallRecipientTarget.Borrower)
                {
                    borrowerInfoControl1.EmailEnabled = false;
                    //Added so that non-borrower calls require validation
                    if (CallsMode)
                    {
                        borrowerInfoControl1.EnableNinetyDayValidations = false;
                        borrowerInfoControl1.SetNeedsVerification();
                    }
                }
                else
                {
                    borrowerInfoControl1.EmailEnabled = true;
                    //Added so that Borrower Incoming/Outgoing calls require validation
                    if (CallsMode)
                    {
                        if(result.CallType == CallType.IncomingCall && result.ContactCodeSelection == ContactCode.FromBorrower && !IvrResponseActivated)
                        {
                            HandleIVRResponse();
                            IvrResponseActivated = true;
                        }
                        else if(IvrResponseActivated)
                        {
                            borrowerInfoControl1.RevertChanges();
                            IvrResponseActivated = false;
                        }

                        if (result.CallType == CallType.IncomingCall || (result.CallType == CallType.OutgoingCall && result.CallStatusType.HasValue && result.CallStatusType.Value == CallStatusType.CallSuccessful))
                        {
                            borrowerInfoControl1.EnableNinetyDayValidations = true;
                            borrowerInfoControl1.SetNeedsVerification();
                        }
                        else
                        {
                            borrowerInfoControl1.EnableNinetyDayValidations = false;
                            borrowerInfoControl1.SetNeedsVerification();
                        }
                    }
                }
            }
        }

        public void SelectDemographicsFromAnotherThread()
        {
            Activate();
        }

        public bool PopulateFrm()
        {
            AddressWarning = false;
            activityCommentSelection.Focus();

            if(UheaaBorrower != null)
            {
                labelSSN.Text = UheaaBorrower.SSN;
                labelName.Text = UheaaBorrower.CompassDemographics.Name;
                labelDOB.Text = UheaaBorrower.CompassDemographics.DOB;
                labelAccountNumber.Text = UheaaBorrower.CompassDemographics.AccountNumber;

                borrowerInfoControl1.Bind(UheaaBorrower.CompassDemographics, DataAccessHelper.Region.Uheaa);
                borrowerInfoControl1.IncludeRefused = CallsMode;
                bool hasDisbursement = DataAccess.DA.HasAnticipatedDisbursement(UheaaBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
                UheaaBorrower.HasPendingDisbursement = hasDisbursement.ToString();
                borrowerInfoControl1.AllowPoBox = !hasDisbursement;
                activityCommentSelection.EnableContactInfoButton = true;

            }

            borrowerInfoControl1.Purplify();

            return true;
        }

        public void WhatToUpdate(UpdateDemoCompassIndicators compass, Demographics borrowerDemographics)
        {
            DemographicVerifications demoVerifications = borrowerInfoControl1.DemographicVerifications;
            //value is true if the data is valid
            compass.Address = demoVerifications.Address.IsValidWithChanges();
            //value is true if the data is not valid
            compass.Phone = !demoVerifications.HomePhone.IsValidWithChanges();
            compass.OtherPhone = !demoVerifications.OtherPhone.IsValidWithChanges();
            compass.Other2Phone = !demoVerifications.OtherPhone2.IsValidWithChanges();
            compass.Email = !demoVerifications.Email.IsValidWithChanges();
            compass.OtherEmail = !demoVerifications.OtherEmail.IsValidWithChanges();
            compass.OtherEmail2 = !demoVerifications.OtherEmail2.IsValidWithChanges();

            compass.PhoneNoPhoneIndicator = borrowerInfoControl1.NoPhone;

            //indicator is true if the system provided address is invalid and the address was verified as valid
            compass.AddressIndicator = (borrowerDemographics.SPAddrInd == "N" && demoVerifications.Address.IsValid());
            //indicator is true if the data was verified, it is valid and it is not blank
            compass.PhoneIndicator = demoVerifications.HomePhone.IsValid();
            compass.OtherPhoneIndicator = demoVerifications.OtherPhone.IsValid();
            compass.Other2PhoneIndicator = demoVerifications.OtherPhone2.IsValid();
            compass.EmailIndicator = demoVerifications.Email.IsValid();
            compass.OtherEmailIndicator = demoVerifications.OtherEmail.IsValid();
            compass.OtherEmail2 = demoVerifications.OtherEmail2.IsValid();

            //address is not valid and the address entered by the user matches the system address
            if(demoVerifications.Address == VerificationSelection.InvalidNoChange)
            {
                compass.AddressIndicator = borrowerInfoControl1.HasAddressChanges;
            }
        }

        public void UpdateSystem()
        {
            bool isSchool = false;
            if(ValidateInput())
            {
                AcpSelectionResult selection = activityCommentSelection.Selection;
                AddressWarning = false;
                borrowerInfoControl1.Focus(); // give the addr1 field focus for when the form appears
                Visible = false;

                if (selection.ContactCodeSelection == ContactCode.FromSchool || selection.ContactCodeSelection == ContactCode.ToSchool)
                {
                    isSchool = true;
                }

                //create comment string
                string commentC = borrowerInfoControl1.GenerateCommentString();
                if(UheaaBorrower != null)
                {
                    UpdateDemoCompassIndicators uheaaCompassInds = new UpdateDemoCompassIndicators();
                    WhatToUpdate(uheaaCompassInds, UheaaBorrower.CompassDemographics);
                    HomePageSessionInteractionCoordinator.AddArcForAllLoans(UheaaBorrower.SSN, "MXADD", commentC, "", false, DataAccessHelper.Region.Uheaa);
                    COMPASSDemographicsProcessor compassDemos = new COMPASSDemographicsProcessor();
                    compassDemos.Update(selection.Tx1jSourceValue, uheaaCompassInds, isSchool, UheaaBorrower.UpdatedDemographics, borrowerInfoControl1.DemographicVerifications, UheaaBorrower.AltAddress);
                }
            }
        }

        public bool ValidateInput()
        {
            List<string> errorMessages = borrowerInfoControl1.ValidateInput(activityCommentSelection.Selection, CallsMode);
            string acpError = activityCommentSelection.Validate();
            if(acpError != null)
            {
                errorMessages.Add(acpError);
            }

            if(errorMessages.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, errorMessages), "Alert", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private string FormatNum(string num, string ext, string foreignCountry, string foreignCity, string foreignLocal)
        {
            if(num == "--")
            {
                return "";
            }
            if(!string.IsNullOrWhiteSpace(num))
            {
                num = num.Insert(3, "-").Insert(7, "-");
                if(string.IsNullOrWhiteSpace(ext))
                {
                    return num;
                }
                else
                {
                    return num + "x" + ext; 
                }
            }
            else
            {
                return foreignCountry + "-" + foreignCity + "-" + foreignLocal;
            }
        }

        private void buttonBackToMainMenu_Click(object sender, EventArgs e)
        {
            BrwInfo411Processor.Close411Form();
            BackButtonClicked = true;
            DialogResult = DialogResult.Cancel;
            Hide();
        }

        private void buttonRefreshScreen_Click(object sender, EventArgs e)
        {
            borrowerInfoControl1.RevertChanges();
            if (CallsMode)
            {
                borrowerInfoControl1.EnableNinetyDayValidations = false;
                borrowerInfoControl1.SetNeedsVerification();
            }
            activityCommentSelection.ClearSelection();
            borrowerInfoControl1.Focus(); //give the addr1 field focus
        }

        public void HandleIVRResponse()
        {
            if(IvrResponse.IsNullOrEmpty())
            {
                //MessageBox.Show("No IVR response from borrower found. Please validate the demos.", "No IVR Response", MessageBoxButtons.OK);
            }
            if(IvrResponse == "V")
            {
                borrowerInfoControl1.SetIVRValidated();
                //MessageBox.Show("Borrower verified their primary phone and address through the IVR.", "Valid Demos", MessageBoxButtons.OK);
            }
            if (IvrResponse == "U")
            {
                MessageBox.Show("Borrower advised that demos need to be updated.", "Invalid Demos", MessageBoxButtons.OK);
            }
            if (IvrResponse == "K")
            {
                //MessageBox.Show("Borrower is in skip and needs demos updated.", "Invalid Demos", MessageBoxButtons.OK);
            }
            if (IvrResponse == "N")
            {
                //MessageBox.Show("Borrower did not verify demos. Please verify demos", "Not Verified", MessageBoxButtons.OK);
            }
        }

        private void buttonSaveAndContinue_Click(object sender, EventArgs e)
        {
            if(ValidateInput())
            {
                borrowerInfoControl1.PersistChanges(activityCommentSelection.Selection.ActivityCodeSelection == ActivityCode.AccountMaintenance, Borrower.UpdatedDemographics);
                Borrower.DemographicsVerifications = borrowerInfoControl1.DemographicVerifications;
            }
            else
            {
                return;
            }

            AcpSelectionResult selection = activityCommentSelection.Selection;
            Borrower.AcpResponses.Selection = selection;

            if(new List<ActivityCode>() { ActivityCode.FailedCall, ActivityCode.TelephoneContact}.Contains(selection.ActivityCodeSelection))
            {
                if(selection.CallType == CallType.OutgoingCall)
                {
                    string homePhone = FormatNum(Borrower.CompassDemographics.HomePhoneNum, Borrower.CompassDemographics.HomePhoneExt, Borrower.CompassDemographics.HomePhoneForeignCountry, Borrower.CompassDemographics.HomePhoneForeignCity, Borrower.CompassDemographics.HomePhoneForeignLocalNumber);
                    string otherPhone = FormatNum(Borrower.CompassDemographics.OtherPhoneNum, Borrower.CompassDemographics.OtherPhoneExt, Borrower.CompassDemographics.OtherPhoneForeignCountry, Borrower.CompassDemographics.OtherPhoneForeignCity, Borrower.CompassDemographics.OtherPhoneForeignLocalNumber);
                    string otherPhone2 = FormatNum(Borrower.CompassDemographics.OtherPhone2Num, Borrower.CompassDemographics.OtherPhone2Ext, Borrower.CompassDemographics.OtherPhone2ForeignCountry, Borrower.CompassDemographics.OtherPhone2ForeignCity, Borrower.CompassDemographics.OtherPhone2ForeignLocalNumber);
                    PhoneIndicator phoneIndicator = new PhoneIndicator(Borrower, homePhone, otherPhone, otherPhone2); 
                    if(homePhone != "--" || otherPhone != "--" || otherPhone2 != "--" || homePhone != "" || otherPhone != "" || otherPhone2 != "")
                    {
                        phoneIndicator.ShowDialog();
                    }

                    Borrower.CompassDemographics.Phone = homePhone;
                    Borrower.CompassDemographics.OtherPhoneNum = otherPhone;
                    Borrower.CompassDemographics.OtherPhone2Num = otherPhone2;
                }
            }

            BackButtonClicked = false;
            BrwInfo411Processor.Close411Form();
            buttonUheaaAddress.Visible = false;
            if(Borrower.CompassDemographics.IsForeignAddress)
            {
                Borrower.UpdatedDemographics.State = "";
                Borrower.UpdatedDemographics.IsForeignAddress = true;
            }
            else
            {
                Borrower.UpdatedDemographics.IsForeignAddress = false;
                Borrower.UpdatedDemographics.Country = "";
                Borrower.UpdatedDemographics.ForeignState = "";
            }
            Hide();
        }

        //this ensures that addr1 is in focus whenever the form is activated
        private void DemographicsUI_Activated(object sender, EventArgs e)
        {
            borrowerInfoControl1.Focus();
        }

        //display the AskDUDE information form
        private void toolStripMenuItemFAQ_Click(object sender, EventArgs e)
        {
            FaqLinker.ShowFaq(this);
        }

        private void toolStripMenuItem411_Click(object sender, EventArgs e)
        {
            BrwInfo411Processor.Show411Form(true);
        }

        private void toolStripMenuItemFeatureRequest_Click(object sender, EventArgs e)
        {
            FeedbackLinker.FeatureRequestAction(this);
        }

        private void toolStripMenuItemBugReport_Click(object sender, EventArgs e)
        {
            FeedbackLinker.BugReportAction(this);
        }

        private void toolStripMenuItemSecurityIncident_Click(object sender, EventArgs e)
        {
            Borrower borrower = null;
            string region = "";
            if(UheaaBorrower != null)
            {
                borrower = UheaaBorrower;
                region = "UHEAA";
            }
            string arguments = $"--ticketType Incident --region {region} --accountNumber {borrower.AccountNumber} --name \"{borrower.FullName}\" --state {borrower.CompassDemographics.State}";
            if(DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);

        }

        private void toolStripMenuItemPhysicalThreat_Click(object sender, EventArgs e)
        {
            Borrower borrower = null;
            string region = "";
            if (UheaaBorrower != null)
            {
                borrower = UheaaBorrower;
                region = "UHEAA";
            }
            string arguments = $"--ticketType Threat --region {region} --accountNumber {borrower.AccountNumber} --name \"{borrower.FullName}\" --state {borrower.CompassDemographics.State}";
            if(DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void buttonUheaaAddress_Click(object sender, EventArgs e)
        {
            //borrowerInfoControl1.Bind(UheaaBorrower.CompassDemographics, DataAccessHelper.Region.Uheaa);
            //buttonCornerstoneAddress.Font = new Font("Arial", 10);
            buttonUheaaAddress.Font = new Font("Arial", 10, FontStyle.Bold);
            borrowerInfoControl1.IncludeRefused = true;
            borrowerInfoControl1.EnableEcorr = (activityCommentSelection.Selection != null && activityCommentSelection.Selection.RecipientTarget == CallRecipientTarget.Borrower);
            OverrideUheaa = true;
        }

        private void toolStripMenuItemTraining_Click(object sender, EventArgs e)
        {
            FaqLinker.ShowTraining(this);
        }

    }
}
