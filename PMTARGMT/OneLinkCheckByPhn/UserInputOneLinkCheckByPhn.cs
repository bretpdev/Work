using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace OneLinkCheckByPhn
{
    public partial class UserInputOneLinkCheckByPhn : FormBase
    {

        /// <summary>
        /// indicator of what to set focus to on the form
        /// </summary>
        public enum ControlToHaveFocus
        {
            SSNOrAcctNum,
            PaymentAmount,
            RoutingNum,
            CheckingAcctNum,
            PaymentPostingDate,
            AllDataValid
        }

        private ControlToHaveFocus _cntrlToFocusOn;

        public UserInputOneLinkCheckByPhn()
        {
            InitializeComponent();
        }
        
        public UserInputOneLinkCheckByPhn(CheckByPhoneData data)
        {
            InitializeComponent();
            checkByPhoneDataBindingSource.DataSource = data;
        }

        public DialogResult ShowDialog(ControlToHaveFocus cntrlToFocusOn)
        {
            _cntrlToFocusOn = cntrlToFocusOn; //make note of which control to give focus so the Activiated event can give the control focus
            return base.ShowDialog();
        }

        private void UserInputOneLinkCheckByPhn_Shown(object sender, EventArgs e)
        {
            if (_cntrlToFocusOn == ControlToHaveFocus.SSNOrAcctNum)
            {
                txtSSNorAcctNum.Focus();
            }
            else if (_cntrlToFocusOn == ControlToHaveFocus.PaymentAmount)
            {
                txtPayment.Focus();
            }
            else if (_cntrlToFocusOn == ControlToHaveFocus.RoutingNum)
            {
                txtRouting.Focus();
            }
            else if (_cntrlToFocusOn == ControlToHaveFocus.CheckingAcctNum)
            {
                txtChecking.Focus();
            }
            else if (_cntrlToFocusOn == ControlToHaveFocus.PaymentPostingDate)
            {
                txtPaymentEffectDate.Focus();
            }
        }


    }
}
