using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace PUTSUSPCOM
{
    public partial class DeleteAndReapplyRow : UserControl
    {

        private DeleteAndReapplyRowData _data;

        /// <summary>
        /// User input from amount field.
        /// </summary>
        public string Amount
        {
            get { return txtAmount.Text; }
            //set { txtAmount.Text = value; }
        }

        /// <summary>
        /// Default Constructor (do not use).
        /// </summary>
        public DeleteAndReapplyRow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DeleteAndReapplyRow(DeleteAndReapplyRowData data)
        {
            InitializeComponent();
            _data = data;
            deleteAndReapplyRowDataBindingSource.DataSource = _data;
        }

        /// <summary>
        /// Ensures that data is valid.
        /// </summary>
        public bool UserInputIsValid()
        {
            if (txtLoanSequence.Text.IsNumeric() == false || txtAmount.Text.IsNumeric() == false ||
                txtTransactionType.TextLength == 0  || (txtDisbursementDate.TextLength != 0 && txtDisbursementDate.Text.IsValidDate() == false))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
