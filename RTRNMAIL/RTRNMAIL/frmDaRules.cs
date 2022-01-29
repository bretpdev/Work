using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Q;

namespace RTRNMAIL
{
	partial class frmDaRules : FormBase
	{
		/// <summary>
		/// DO NOT USE!!! The no-parameter constructor is requred by Visual Studio's Form Designer, but it will not work with the script.
		/// </summary>
		public frmDaRules()
		{
			InitializeComponent();
		}

		public frmDaRules(bool testMode)
		{
			const int LEFT_COLUMN_WIDTH = 15;
			const char PADDING_CHARACTER = ' ';

			InitializeComponent();
			
			StringBuilder rulesBuilder = new StringBuilder();
			rulesBuilder.Append("*We should use these abbreviations instead of spelling out the words.");
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("Street Suffix Abbreviations");
			rulesBuilder.Append(Environment.NewLine);
			foreach (KeyValuePair<string, string> suffix in DataAccess.GetStreetSuffixes(testMode))
			{
				rulesBuilder.Append(suffix.Value.PadRight(LEFT_COLUMN_WIDTH, PADDING_CHARACTER));
				rulesBuilder.Append(suffix.Key);
				rulesBuilder.Append(Environment.NewLine);
			}
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("Secondary unit abbreviations");
			rulesBuilder.Append(Environment.NewLine);
			foreach (KeyValuePair<string, string> unit in DataAccess.GetSecondaryUnits(testMode))
			{
				rulesBuilder.Append(unit.Value.PadRight(LEFT_COLUMN_WIDTH, PADDING_CHARACTER));
				rulesBuilder.Append(unit.Key);
				rulesBuilder.Append(Environment.NewLine);
			}
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("*       Eliminate all punctuation and/or symbols.");
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("*       If the apartment, unit or suite number won't fit on the first");
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("        line, move everything else to the second line and keep the");
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("        apt, unit or suite on the first line.");
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("*       c/o \"John Doe\" needs to be handles the same way as");
			rulesBuilder.Append(Environment.NewLine);
			rulesBuilder.Append("        apartment numbers.");
			lblRules.Text = rulesBuilder.ToString();
		}
	}//class
}//namsepace
