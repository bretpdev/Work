using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CSharpScriptGenerator
{
    public class ScriptGenerator
    {
        public ScriptGenerator()
        {
        }

        public void Generate(ScriptAttributes attributes)
        {
            //Check that this script doesn't already exist.
            string regionDirectory = (attributes.IsFederalDirectScript ? "FedScripts" : "FfelScripts");
            string solutionDirectory = string.Format(@"{0}\{1}\{2}", attributes.CSharpCodeRoot, regionDirectory, attributes.Namespace);
            if (Directory.Exists(solutionDirectory))
            {
                string message = string.Format("The {0} script appears to already exist. If you really want to wipe it out and start over, delete the script folder first (use SVN Delete and commit if it's under version control).", attributes.ScriptID);
                MessageBox.Show(message, "Script Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Create the solution directory...
            Directory.CreateDirectory(solutionDirectory);
            //...and the project directory.
            string projectDirectory = string.Format(@"{0}\{1}", solutionDirectory, attributes.Namespace);
            Directory.CreateDirectory(projectDirectory);

            //Create a GUID for the new project.
            if (string.IsNullOrEmpty(attributes.Guid)) { attributes.Guid = Guid.NewGuid().ToString().ToUpper(); }

            //Create the essential files from templates.
            CreateSolutionFile(solutionDirectory, attributes);
            CreateProjectFile(projectDirectory, attributes);
            CreateStartingClass(projectDirectory, attributes);
            CreateDataAccessClass(projectDirectory, attributes);
            DataAccess.UpdateScriptTable(attributes);
            OpenSolution(solutionDirectory, attributes);

            //Update Sacker with the code location.
            DataAccess.UpdateSackerCodeLocation(attributes.Namespace, attributes.ScriptName);
        }//Generate()

        private void CreateDataAccessClass(string projectDirectory, ScriptAttributes attributes)
        {
            //Read in the template.
            string[] templateLines = attributes.IsCSharp ? Properties.Resources.DataAccessCSharp.Split(new string[] { Environment.NewLine }, StringSplitOptions.None) : Properties.Resources.DataAccess.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            //Write out the new class file, replacing template tags as needed.
            string outputFile = string.Format(@"{0}\DataAccess.cs", projectDirectory);
            WriteNewFile(outputFile, templateLines, attributes);
        }//CreateDataAccessClass()

        private void CreateProjectFile(string projectDirectory, ScriptAttributes attributes)
        {
            XmlDocument projectXml = new XmlDocument();
            projectXml.LoadXml(attributes.IsCSharp ? Properties.Resources.PROJECTCSHARP : Properties.Resources.PROJECT);
            projectXml.GetElementsByTagName("ProjectGuid")[0].InnerText = "{" + attributes.Guid + "}";
            projectXml.GetElementsByTagName("RootNamespace")[0].InnerText = attributes.Namespace;
            projectXml.GetElementsByTagName("AssemblyName")[0].InnerText = attributes.Namespace;
            projectXml.GetElementsByTagName("ItemGroup")[1].ChildNodes[0].Attributes[0].Value = attributes.StartingClassName + ".cs";
            string fileName = string.Format(@"{0}\{1}.csproj", projectDirectory, attributes.Namespace);
            projectXml.Save(fileName);
        }//CreateProjectFile()

        private void CreateSolutionFile(string solutionDirectory, ScriptAttributes attributes)
        {
            //Read in the template.
            string[] templateLines = attributes.IsCSharp ? Properties.Resources.SOLUTIONCSHARP.Split(new string[] { Environment.NewLine }, StringSplitOptions.None) : Properties.Resources.SOLUTION.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            //Write out the new class file, replacing template tags as needed.
            string outputFile = string.Format(@"{0}\{1}.sln", solutionDirectory, attributes.Namespace);
            WriteNewFile(outputFile, templateLines, attributes);
        }//CreateSolutionFile()

        private void CreateStartingClass(string projectDirectory, ScriptAttributes attributes)
        {
            //Read in the correct template based on whether the script can be called by MauiDUDE.
            string templateFile;
            if (attributes.IsFederalDirectScript)
            {
                if (attributes.IsBatchScript)
                {
                    templateFile = attributes.IsCSharp ? Properties.Resources.StartingClassFedBatchCSharp : Properties.Resources.StartingClassFedBatch;
                }
                else if (attributes.CanRunFromMauiDude)
                {
                    templateFile = Properties.Resources.StartingClassFedDude;
                }
                else
                {
                    templateFile = attributes.IsCSharp ? Properties.Resources.StartingClassScriptCharpFed : Properties.Resources.StartingClassFedScript;
                }
            }
            else
            {
                if (attributes.IsBatchScript)
                {
                    templateFile = attributes.IsCSharp ? Properties.Resources.StartingClassBatchCSharp : Properties.Resources.StartingClassBatch;
                }
                else if (attributes.CanRunFromMauiDude)
                {
                    templateFile = Properties.Resources.StartingClassDude;
                }
                else
                {
                    templateFile = attributes.IsCSharp ? Properties.Resources.StartingClassScriptCSharp : Properties.Resources.StartingClassScript;
                }
            }
            string[] templateLines = templateFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            //Write out the new class file, replacing template tags as needed.
            string outputFile = string.Format(@"{0}\{1}.cs", projectDirectory, attributes.StartingClassName);
            WriteNewFile(outputFile, templateLines, attributes);
        }//CreateStartingClass()

        private void OpenSolution(string solutionDirectory, ScriptAttributes attributes)
        {
            //Start up Visual Studio with the new solution.
            string visualStudioExecutable = "";
            if (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor == 1)
            {
                visualStudioExecutable = @"C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe";
            }
            else if (System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor == 1)
            {
                visualStudioExecutable = @"C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe";
            }
            string solution = Path.Combine(solutionDirectory, attributes.Namespace + ".sln");
            Process.Start(visualStudioExecutable, string.Format("\"{0}\"", solution));
        }//OpenSolution()

        private string ReplaceTags(string templateLine, ScriptAttributes attributes)
        {
            string outputLine = templateLine;
            outputLine = outputLine.Replace("<<NAMESPACE>>", attributes.Namespace);
            outputLine = outputLine.Replace("<<STARTING_CLASS>>", attributes.StartingClassName);
            outputLine = outputLine.Replace("<<SCRIPT_ID>>", attributes.ScriptID);
            outputLine = outputLine.Replace("<<GUID>>", attributes.Guid);
            return outputLine;
        }//ReplaceTags()

        private void WriteNewFile(string fileName, string[] templateLines, ScriptAttributes attributes)
        {
            using (StreamWriter fileWriter = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                foreach (string templateLine in templateLines)
                {
                    if (templateLine.Contains("<<"))
                    {
                        string outputLine = ReplaceTags(templateLine, attributes);
                        fileWriter.WriteLine(outputLine);
                    }
                    else
                    {
                        fileWriter.WriteLine(templateLine);
                    }
                }//foreach
                fileWriter.Close();
            }//using
        }//WriteNewFile()
    }//class
}//namespace
