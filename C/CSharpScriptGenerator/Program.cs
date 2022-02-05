using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSharpScriptGenerator
{
	public class Program
	{
		[STAThread()]
		public static void Main(string[] args)
		{
			ScriptAttributes attributes = new ScriptAttributes();
			AttributeChooser chooser = new AttributeChooser(attributes);

			//Check that we have a valid C#Scripts directory.
			while (string.IsNullOrEmpty(attributes.CSharpCodeRoot) || !Directory.Exists(attributes.CSharpCodeRoot) || Directory.GetDirectories(attributes.CSharpCodeRoot, "Nexus").Length == 0)
			{
				const string MESSAGE = "Please select the folder with your working copy of the C#Scripts repository.";
				const string CAPTION = "C#Scripts Folder";
				if (MessageBox.Show(MESSAGE, CAPTION, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { return; }
				FolderBrowserDialog folderDialog = new FolderBrowserDialog();
				if (folderDialog.ShowDialog() != DialogResult.OK) { return; }
				attributes.CSharpCodeRoot = folderDialog.SelectedPath;
				//Allow for cases where the user selected the "Scripts" subdirectory.
				DirectoryInfo codeRoot = new DirectoryInfo(attributes.CSharpCodeRoot);
				if (codeRoot.Name == "FedScripts" || codeRoot.Name == "FfelScripts") { attributes.CSharpCodeRoot = codeRoot.Parent.FullName; }
			}//while

			if (chooser.ShowDialog() != DialogResult.OK) { return; }
			ScriptGenerator generator = new ScriptGenerator();
			generator.Generate(attributes);
		}
	}//class
}//namespace
