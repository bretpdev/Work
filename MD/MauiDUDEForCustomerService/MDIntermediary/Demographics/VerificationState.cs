using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIntermediary
{
    public partial class VerificationState : UserControl
    {
        public VerificationState()
        {
            InitializeComponent();
            NoChangesMode();
        }

        /// <summary>
        /// Make the buttons on this control purple.
        /// </summary>
        public void Purplify()
        {
            Color purple = Color.FromArgb(184, 174, 231);
            foreach (Button b in this.MainPanel.Controls.Cast<Button>())
            {
                b.BackColor = purple;
            }
        }

        public VerificationSelection Selection
        {
            get
            {
                if (NoChangeButton.IsChecked)
                    return VerificationSelection.NoChange;
                if (ValidButton.IsChecked)
                {
                    if (CurrentMode == CurrentModeEnum.NoChangesMode)
                        return VerificationSelection.ValidNoChange;
                    else if (InvalidateFirstButton.IsChecked)
                        return VerificationSelection.ValidWithChangeAndInvalidateFirst;
                    else
                        return VerificationSelection.ValidWithChange;
                }
                if (InvalidButton.IsChecked)
                    return VerificationSelection.InvalidNoChange;
                if (RefusedButton.IsChecked)
                    return VerificationSelection.RefusedNoChange;

                throw new Exception("Encountered an invalid state in VerificationState control.");
            }
        }

        public enum CurrentModeEnum
        {
            NoChangesMode,
            ChangesMode,
            InvalidDataMode
        }
        public CurrentModeEnum CurrentMode { get; private set; }

        public bool IncludeRefused { get { return RefusedButton.Visible; } set { RefusedButton.Visible = value; } }

        public void ChangesMode()
        {
            CurrentMode = CurrentModeEnum.ChangesMode;
            InvalidButton.Visible = false;
            InvalidateFirstButton.Visible = true;
            ValidButton.Visible = true;

            UncheckAll();
            ValidButton.IsChecked = true;
            InvalidateFirstButton.IsChecked = true;
        }

        /// <summary>
        /// Puts the control in No Changes mode 
        /// </summary>
        public void NoChangesMode(bool overrideRefused = false)
        {
            bool refused = RefusedButton.IsChecked;
            CurrentMode = CurrentModeEnum.NoChangesMode;
            ValidButton.Visible = true;
            InvalidateFirstButton.Visible = false;
            InvalidateFirstButton.IsChecked = false;
            InvalidButton.Visible = true;

            UncheckAll();
            if (refused && !overrideRefused)
                RefusedButton.IsChecked = true;
            else
                NoChangeButton.IsChecked = true;
        }

        public void InvalidDataMode()
        {
            CurrentMode = CurrentModeEnum.InvalidDataMode;
            InvalidateFirstButton.Visible = false;
            ValidButton.Visible = false;
            InvalidButton.Visible = false;
        }

        //Used to set the selection to valid for IVR responses
        public void SetValidSelection()
        {
            if(ValidButton.Visible)
            {
                UncheckAll();
                ValidButton.IsChecked = true;
            }
        }

        public delegate void RevertChangesDelegate();
        /// <summary>
        /// This event is raised when the No Changes button is clicked.
        /// </summary>
        public event RevertChangesDelegate RevertChanges;

        private void NoChangeButton_Click(object sender, EventArgs e)
        {
            NoChangesMode();
            UncheckAll();
            NoChangeButton.IsChecked = true;
            if (RevertChanges != null)
                RevertChanges();
        }

        private void RefusedButton_Click(object sender, EventArgs e)
        {
            UncheckAll();
            RefusedButton.IsChecked = true;
            if (RevertChanges != null)
                RevertChanges();
        }

        /// <summary>
        /// Uncheck all buttons (does not include Consent button).
        /// </summary>
        private void UncheckAll()
        {
            RefusedButton.IsChecked = NoChangeButton.IsChecked = ValidButton.IsChecked = InvalidButton.IsChecked
                = false;
        }

        private void ValidButton_Click(object sender, EventArgs e)
        {
            UncheckAll();
            ValidButton.IsChecked = true;
            if (CurrentMode == CurrentModeEnum.ChangesMode)
            {
                InvalidateFirstButton.Visible = true;
                InvalidateFirstButton.IsChecked = true;
            }
            InvalidButton.Visible = false;
        }

        private void InvalidButton_Click(object sender, EventArgs e)
        {
            if (CurrentMode == CurrentModeEnum.NoChangesMode)
            {
                UncheckAll();
                InvalidButton.IsChecked = true;
            }
        }

        public void InvalidSelection()
        {
            NoChangesMode();
            UncheckAll();
            InvalidButton.Visible = true;
            InvalidButton.IsChecked = true;
        }
    }
}
