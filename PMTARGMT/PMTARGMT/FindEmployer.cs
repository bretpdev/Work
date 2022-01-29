using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace Payments
{
    public partial class FindEmployer : FormBase
    {

        /// <summary>
        /// Form results
        /// </summary>
        public enum ButtonClicked
        {
            Search,
            Add,
            Cancel
        }

        public ButtonClicked GetButtonClickedResults { get; set; }

        public FindEmployer()
        {
            InitializeComponent();
        }

        public FindEmployer(ExtendedEmployerDemographics demos)
        {
            InitializeComponent();
            extendedEmployerDemographicsBindingSource.DataSource = demos;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            GetButtonClickedResults = ButtonClicked.Cancel;
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetButtonClickedResults = ButtonClicked.Search;
            this.Close();
        }

        private void btnAddEmployer_Click(object sender, EventArgs e)
        {
            GetButtonClickedResults = ButtonClicked.Add;
            this.Close();
        }

    }
}
