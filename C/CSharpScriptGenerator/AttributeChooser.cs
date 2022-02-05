using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSharpScriptGenerator
{
	public partial class AttributeChooser : Form
	{
		private ScriptAttributes _attributes;

		public AttributeChooser(ScriptAttributes attributes)
		{
			InitializeComponent();
			sackerScriptBindingSource.DataSource = DataAccess.SackerScripts.OrderBy(p => p.Id);
			_attributes = attributes;
			scriptAttributesBindingSource.DataSource = _attributes;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string solutionName = GetValidSolutionName(cmbScriptId.Text);
			if (string.IsNullOrEmpty(solutionName))
			{
				return;
			}
			else
			{
				_attributes.Namespace = solutionName;
			}
			string startingClassName = GetValidStartingClassName(txtClassName.Text);
			if (string.IsNullOrEmpty(startingClassName))
			{
				return;
			}
			else
			{
				txtClassName.Text = startingClassName;
			}
			_attributes.ScriptName = (cmbScriptId.SelectedItem as SackerScript).Name;
            _attributes.IsCSharp = cboUseCSharp.Checked;
			this.DialogResult = DialogResult.OK;
		}

		private void cmbScriptId_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtClassName.Text = Sanitize((cmbScriptId.SelectedItem as SackerScript).Name, false);
			chkBatch.Checked = ((cmbScriptId.SelectedItem as SackerScript).Type.ToLower().Contains("batch"));
			chkFederalDirect.Checked = ((cmbScriptId.SelectedItem as SackerScript).Id.ToUpper().EndsWith("FED"));
		}

		private string GetValidSolutionName(string scriptId)
		{
			//Check that the script ID will work as a sanitary solution name,
			//i.e., it doesn't change if we put it through the sanitizer.
			string sanitizedId = Sanitize(scriptId, true).ToUpper();
			if (sanitizedId.ToUpper() == scriptId.ToUpper()) { return scriptId.ToUpper(); }

			//Suggest using the sanitized version if it's not already another script's ID.
			List<string> existingScriptIds = DataAccess.SackerScripts.Select(p => (p.Id ?? "").ToUpper()).ToList();
			if (!existingScriptIds.Contains(sanitizedId))
			{
				string message = string.Format("The script ID ({0}) won't work as a solution name. Would you like to use {1} as the solution name instead?", scriptId, sanitizedId);
				if (MessageBox.Show(message, "Alternate Solution Name", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					return sanitizedId;
				}
			}

			//Prompt for a custom solution name.
			string suggestedName = scriptId.ToUpper(); ;
			while (true)
			{
				if (string.IsNullOrEmpty(suggestedName))
				{
					return null;
				}
				else if (suggestedName != Sanitize(suggestedName, true).ToUpper())
				{
					string message = string.Format("{0} won't work as a solution name. Please choose another.", suggestedName);
					suggestedName = Microsoft.VisualBasic.Interaction.InputBox(message, "Alternate Solution Name", "", 450, 350).ToUpper();
				}
				else if (existingScriptIds.Contains(suggestedName))
				{
					string message = string.Format("{0} is already in use as a script ID. Please choose another solution name.", suggestedName);
					suggestedName = Microsoft.VisualBasic.Interaction.InputBox(message, "Alternate Solution Name", "", 450, 350).ToUpper();
				}
				else
				{
					return suggestedName;
				}
			}//while
		}//GetValidSolutionName()

		private string GetValidStartingClassName(string className)
		{
			//See if the proposed class name is already valid.
			string sanitizedClassName = Sanitize(className, false);
			if (sanitizedClassName.ToUpper() == className.ToUpper()) { return className; }

			//Prompt for a valid class name.
			string suggestedName = className;
			while (true)
			{
				if (string.IsNullOrEmpty(suggestedName))
				{
					return null;
				}
				else if (suggestedName.ToUpper() != Sanitize(suggestedName, false).ToUpper())
				{
					string message = string.Format("{0} won't work as a class name. Please choose another.", suggestedName);
					suggestedName = Microsoft.VisualBasic.Interaction.InputBox(message, "Starting Class Name", "", 450, 350);
				}
				else
				{
					return suggestedName;
				}
			}//while
		}//GetValidStartingClassName()

		private string Sanitize(string name, bool isSolutionName)
		{
			StringBuilder nameBuilder = new StringBuilder();
			bool newWord = false;

			//Make sure the first character is a letter.
			int charIndex = 0;
			while (nameBuilder.Length == 0 && charIndex < name.Length)
			{
				if (Regex.IsMatch(name[charIndex].ToString(), "[a-z]", RegexOptions.IgnoreCase))
				{
					nameBuilder.Append(name[charIndex].ToString().ToUpper());
				}
				charIndex++;
			}//while

			//Go through the rest of the name and accept valid characters.
			while (charIndex < name.Length)
			{
				string currentCharacter = name[charIndex].ToString();
				//See if this character is valid.
				if (isSolutionName && currentCharacter == " " && name[charIndex - 1] != '_')
				{
					//For solution names, replace space with underscore, unless the last character was also an underscore.
					nameBuilder.Append("_");
					//the next letter will start a new word.
					newWord = true;
				}
				else if (Regex.IsMatch(currentCharacter, @"\d"))
				{
					//It's a digit. Use it as-is.
					nameBuilder.Append(currentCharacter);
					//The next letter will start a new word.
					newWord = true;
				}
				else if (Regex.IsMatch(currentCharacter, "[a-z]", RegexOptions.IgnoreCase))
				{
					//It's a letter. Add it to the sanitized name, setting the case according to whether it starts a new word.
					string correctCase = (newWord ? currentCharacter.ToUpper() : currentCharacter.ToLower());
					nameBuilder.Append(correctCase);
					//The next letter will not start a new word.
					newWord = false;
				}
				else
				{
					//Skip it, it's not a valid character. The next letter will start a new word.
					newWord = true;
				}
				charIndex++;
			}//while

			return nameBuilder.ToString();
		}//Sanitize()
	}//class
}//namespace
