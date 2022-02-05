using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;
using Uheaa.Common.DataAccess;

namespace MDIntermediary.Demographics
{
    public partial class ActivityCommentSelection : UserControl
    {
        ActivityCommentHelper helper = new ActivityCommentHelper();
        public ActivityCommentSelection()
        {
            InitializeComponent();
            EnableContactInfoButton = true;
            foreach (var box in new ComboBox[] { FirstSelectionBox, SecondSelectionBox, CallStatusBox })
            {
                box.ValueMember = "Value";
                box.DisplayMember = "Text";
            }
            ProcessingLeft = SecondSelectionBox.Left;
            CallsLeft = ContactCodeBox.Left;
            ProcessingWidth = SecondSelectionBox.Width;
            CallsWidth = SecondSelectionBox.Width + (SecondSelectionBox.Left - CallsLeft);
        }

        private int ProcessingLeft { get; set; }
        private int ProcessingWidth { get; set; }
        private int CallsLeft { get; set; }
        private int CallsWidth { get; set; }
        private IBorrower Borrower { get; set; }
        private bool enableContactInfoButton;
        public bool EnableContactInfoButton
        {
            get { return enableContactInfoButton; }
            set{
                enableContactInfoButton = value;
                SetCallRecipientButtonVisibility();
            } 
        }

        public delegate void SelectionChangedDelegate();
        public event SelectionChangedDelegate SelectionChanged;
        private void TriggerSelectionChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged();
        }

        public void ClearSelection()
        {
            FirstSelectionBox.SelectedIndex = 0;
            SecondSelectionBox.SelectedIndex = 0;
            ContactCodeBox.Text = ActivityCodeBox.Text = "";
        }

        private Func<AcpSelectionResult> getSelection = null;
        public AcpSelectionResult Selection
        {
            get
            {
                if (getSelection == null)
                    return null;
                return getSelection();
            }
        }

        private Func<List<string>> validate = null;
        /// <summary>
        /// Returns null if all fields are properly filled out.  Otherwise returns an error message.
        /// </summary>
        public string Validate()
        {
            if (validate == null)
                return null;
            List<string> messages = validate();
            if (messages.Any())
                return string.Join(Environment.NewLine, messages);
            return null;
        }

        public void CallsMode(IBorrower borrower)
        {
            this.Borrower = borrower;
            FirstBoxCallsDisplay();
            SecondBoxCallsDisplay();

            FirstSelectionBox.DataSource = helper.GetCallTypes();

            CallStatusBox.DataSource = helper.GetCallStatuses();
            FirstSelectionBox.SelectedValueChanged += (o, ea) =>
            {
                var visible = GetComboBoxItem<CallType>(FirstSelectionBox) == CallType.OutgoingCall;
                CallStatusBox.Visible = visible;
                CallStatusLabel.Visible = visible;
                TriggerSelectionChanged();
            };

            SecondBoxCallsBinding();

            this.getSelection = () =>
            {
                var callType = GetComboBoxItem<CallType>(FirstSelectionBox);
                var recipientTarget = GetComboBoxItem<CallRecipientTarget>(SecondSelectionBox);
                if (callType.HasValue && recipientTarget.HasValue)
                {
                    Reference reference = (SecondSelectionBox.SelectedItem as RecipientComboBoxItem).Reference;
                    if (reference != null)
                    {
                        selectedRecipient = new AcpRecipientInfo()
                        {
                            Relationship = "Reference",
                            Authorized = reference.AuthorizedThirdPartyIndicator == "Y",
                            RecipientName = reference.FullName
                        };
                    }
                    return helper.CalculateSelection(callType.Value, recipientTarget.Value, reference, GetComboBoxItem<CallStatusType>(CallStatusBox), recipient);
                }
                return null;
            };
            this.validate = () =>
            {
                var callType = GetComboBoxItem<CallType>(FirstSelectionBox);
                var recipientTarget = GetComboBoxItem<CallRecipientTarget>(SecondSelectionBox);
                List<string> messages = new List<string>();
                if (callType == null)
                    messages.Add("Please select a Call Type");
                if (recipientTarget == null)
                    messages.Add("Please select a Recipient Target");
                if (callType == CallType.OutgoingCall && GetComboBoxItem<CallStatusType>(CallStatusBox) == null)
                    messages.Add("Please select a Call Status");
                if (ContactInfoButton.Visible)
                {
                    string message = recipient.Validate();
                    if (message != null)
                        messages.Add(message);
                }
                return messages;
            };
        }

        public void ProcessingMode(IBorrower borrower)
        {
            this.Borrower = borrower;
            CallStatusLabel.Visible = CallStatusBox.Visible = ContactInfoButton.Visible = false;
            SecondSelectionBox.Width = ProcessingWidth;
            SecondSelectionBox.Left = ProcessingLeft;
            ContactCodeBox.Visible = ActivityCodeBox.Visible = true;

            var activityCodes = helper.GetActivityCodes();
            var contactCodes = helper.GetContactCodes();
            if (!(FirstSelectionBox.DataSource is ComboBoxItem<ActivityCode>[])) //only rebind if datasource is different
                FirstSelectionBox.DataSource = activityCodes;
            if (!(SecondSelectionBox.DataSource is ComboBoxItem<ContactCode>[]))
                SecondSelectionBox.DataSource = contactCodes;

            this.getSelection = () =>
            {
                var ac = GetComboBoxItem<ActivityCode>(FirstSelectionBox);
                var cc = GetComboBoxItem<ContactCode>(SecondSelectionBox);
                if (ac.HasValue && cc.HasValue)
                    return helper.CalculateSelection(ac.Value, cc.Value);
                return null;
            };
            this.validate = () =>
            {
                List<string> messages = new List<string>();
                if (ActivityCodeBox.TextLength != 2 || ContactCodeBox.TextLength != 2 || Selection == null)
                    messages.Add("You must provide a valid activity and contact code.");
                if (ActivityCodeBox.TextLength == 0 || ContactCodeBox.TextLength == 0)
                    messages.Add("You must enter the activity code and the contact code to continue.");
                return messages;
            };
        }

        private void OfficeProcessingMode()
        {
            SecondBoxCallsDisplay();
            SecondBoxCallsBinding();

            this.getSelection = () =>
            {
                var ac = GetComboBoxItem<ActivityCode>(FirstSelectionBox);
                var target = (SecondSelectionBox.SelectedItem as RecipientComboBoxItem);
                if (ac.HasValue && target.Value.HasValue)
                    return helper.CalculateSelection(CallType.OfficeVisit, target.Value.Value, target.Reference, null, recipient);
                return null;
            };
            this.validate = () =>
            {
                List<string> messages = new List<string>();
                if (SecondSelectionBox.SelectedIndex == 0)
                    messages.Add("You must select a recipient to continue.");
                if (ContactInfoButton.Visible)
                {
                    string message = recipient.Validate();
                    if (message != null)
                        messages.Add(message);
                }
                return messages;
            };
        }

        #region Display
        private void FirstBoxCallsDisplay()
        {
            FirstSelectionBox.Width = CallsWidth;
            FirstSelectionBox.Left = CallsLeft;
            FirstLabel.Text = "Call Type";
            ActivityCodeBox.Visible = false;
        }
        private void FirstBoxProcessingDisplay()
        {
            FirstSelectionBox.Width = ProcessingWidth;
            FirstSelectionBox.Left = ProcessingLeft;
            FirstLabel.Text = "Activity Code";
            ActivityCodeBox.Visible = true;
        }
        private void SecondBoxCallsDisplay()
        {
            SecondSelectionBox.Width = CallsWidth;
            SecondSelectionBox.Left = CallsLeft;
            SecondLabel.Text = "Recipient";
            ContactCodeBox.Visible = false;
        }
        private void SecondBoxProcessingDisplay()
        {
            SecondSelectionBox.Width = ProcessingWidth;
            SecondSelectionBox.Left = ProcessingLeft;
            SecondLabel.Text = "Contact Code";
            ContactCodeBox.Visible = false;
        }
        #endregion
        #region Bindings
        private void SecondBoxCallsBinding()
        {
            SecondSelectionBox.DataSource = helper.GetCallRecipientTarget(Borrower);
        }
        #endregion

        private T? GetComboBoxItem<T>(ComboBox cb) where T : struct
        {
            var item = cb.SelectedItem as ComboBoxItem<T>;
            if (item == null)
                return null;
            return item.Value;
        }

        private AcpRecipientInfo recipient = new AcpRecipientInfo();
        private AcpRecipientInfo selectedRecipient = null;
        private void ContactInfoButton_Click(object sender, EventArgs e)
        {
            var recip = selectedRecipient ?? recipient;
            var form = new AcpRecipientForm(recip);
            if (GetComboBoxItem<CallRecipientTarget>(SecondSelectionBox) == CallRecipientTarget.Endorser)
            {
                recip.Authorized = true;
                recip.Relationship = "Endorser";
                form.AuthorizedEnabled = false;
                form.RelationshipEnabled = false;
            }
            form.Purplify();
            form.ShowDialog();
            recipient.RecipientName = recip.RecipientName;
            recipient.Relationship = recip.Relationship;
            recipient.Authorized = recip.Authorized;
            recipient.ContactPhoneNumber = recip.ContactPhoneNumber;
        }

        private void SetCallRecipientButtonVisibility()
        {
            var callRecip = GetComboBoxItem<CallRecipientTarget>(SecondSelectionBox);
            if (callRecip != null)
            {
                var visible = callRecip.HasValue && callRecip != CallRecipientTarget.Borrower && EnableContactInfoButton;
                if ((SecondSelectionBox.SelectedItem as RecipientComboBoxItem).Reference != null)
                    visible = false; //already have info
                ContactInfoButton.Visible = visible;
            }
        }

        private void SecondSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var callRecip = GetComboBoxItem<CallRecipientTarget>(SecondSelectionBox);
            if (callRecip != null)
            {
                TriggerSelectionChanged();
                SetCallRecipientButtonVisibility();
            }
            var item = GetComboBoxItem<ContactCode>(SecondSelectionBox);
            if (item != null)
            {
                if (item.HasValue)
                    ContactCodeBox.Text = helper.ContactCodes[item.Value].ContactCode;
                else
                    ContactCodeBox.Text = "";
                TriggerSelectionChanged();
            }
        }

        private void FirstSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = GetComboBoxItem<ActivityCode>(FirstSelectionBox);
            if (item != null)
            {
                if (item.HasValue)
                {
                    ActivityCodeBox.Text = helper.ActivityCodes[item.Value];
                    if (item.Value == ActivityCode.OfficeVisit)
                    {
                        OfficeProcessingMode();
                    }
                    else
                    {
                        ProcessingMode(Borrower);
                        ContactInfoButton.Visible = false;
                    }
                }
                else
                    ActivityCodeBox.Text = "";
                TriggerSelectionChanged();
            }
        }

        private void ActivityCodeBox_Leave(object sender, EventArgs e)
        {
            TryActivityCodeMatch();
        }

        private void ContactCodeBox_Leave(object sender, EventArgs e)
        {
            TryContactCodeMatch();
        }

        private void ActivityCodeBox_TextChanged(object sender, EventArgs e)
        {
            if (TryActivityCodeMatch())
            {
                ContactCodeBox.Focus();
            }
        }

        private bool TryActivityCodeMatch()
        {
            string value = ActivityCodeBox.Text.ToLower();
            var match = helper.GetActivityCodes().Where(a => a.Value.HasValue)
                .SingleOrDefault(a => helper.ActivityCodes[a.Value.Value].ToLower() == value);
            if (match == null)
                return false;
            else
                FirstSelectionBox.SelectedItem = match;

            return true;
        }

        private bool TryContactCodeMatch()
        {
            string value = ContactCodeBox.Text.ToLower();
            var match = helper.GetContactCodes().Where(c => c.Value.HasValue)
                .SingleOrDefault(c => helper.ContactCodes[c.Value.Value].ContactCode.ToLower() == value);
            if (match == null)
                return false;
            else
                SecondSelectionBox.SelectedItem = match;
            TriggerSelectionChanged();
            return true;
        }

        private void ContactCodeBox_TextChanged(object sender, EventArgs e)
        {
            TryContactCodeMatch();
        }

        private void CallStatusBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelectionChanged();
        }
    }
}
