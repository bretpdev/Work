using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.Scripts
{
	public partial class ManualComments : Form
	{
		private int MaxCharactersAllowed = 900;
		public string Comment { get; set; }

        public ManualComments(string comment, int maxLength)
        {
            InitializeComponent();
            txtComments.Text = comment;
            txtComments.MaxLength = maxLength;
            MaxCharactersAllowed = maxLength;
            SetCounts();
        }

		public ManualComments(string comment)
		{
			InitializeComponent();
			txtComments.Text = comment;
            txtComments.MaxLength = MaxCharactersAllowed;
            
			SetCounts();
		}
        public void SetLabel(string text)
        {
            label1.Text = text;
        }

		private void btnOk_Click(object sender, EventArgs e)
		{
			Comment = txtComments.Text;
			DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void SetCounts()
		{
			int remainingCharacters = MaxCharactersAllowed - txtComments.TextLength;

			lblCharacterRemaining.Text = "Number of characters remaining: " + remainingCharacters.ToString();

			if (remainingCharacters == 10)
			{
				lblCharacterRemaining.ForeColor = Color.Red;
			}
		}

		private void txtComments_TextChanged(object sender, EventArgs e)
		{
			SetCounts();
		}
	}
}
