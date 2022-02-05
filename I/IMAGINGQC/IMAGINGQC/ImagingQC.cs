using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace IMAGINGQC
{
    public class ImagingQC : BatchScriptBase
    {
        public ImagingQC(ReflectionInterface ri)
            : base(ri, "IMAGINGQC")
        {
        }

        public override void Main()
        {
			if (!CalledByMasterBatchScript())
			{
				string startupMessage = "This script checks for unprocessed files in imaging system folders. Click OK to continue, or Cancel to quit.";
				if (MessageBox.Show(startupMessage, ScriptID, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK) { EndDLLScript(); }
			}

			IEnumerable<ImagingFolder> folders = GetFolders();
			SetOldestFileDateForEachFolder(folders);
			SendNotifications(folders.Where(p => p.OldestFileCreateDate < DateTime.Now.Date));

			ProcessingComplete();
        }//Main()

		private IEnumerable<ImagingFolder> GetFolders()
		{
			List<ImagingFolder> folders = new List<ImagingFolder>();
			
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTCL_hld")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTCL_imp")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTCR_hld")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTCR_imp")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTLN_hld")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTLN_imp")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTCL_Otr_imp")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTCR_Otr_imp")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_UTLN_Otr_imp")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_COMM")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_ComClaims")));
			folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_ComHR")));
			if (RI.TestMode) { folders.Add(new ImagingFolder(Efs.GetPath("ImagingQC_Test"))); }

			//Warn the user if any folders cannot be found.
			foreach (ImagingFolder folder in folders)
			{
				if (!Directory.Exists(folder.Path))
				{
					string message = string.Format("Folder {0} cannot be found.  Please contact Systems Support for assistance.", folder.Path);
					MessageBox.Show(message, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Error);
					EndDLLScript();
				}
			}//foreach
			
			return folders;
		}//GetFolders()

		private void SendNotifications(IEnumerable<ImagingFolder> geezers)
		{
            IEnumerable<User> qcUsers = DataAccess.GetUsersInThatRoleForThatBusinessUnit(RI.TestMode, "Member Of", "Document Services");
			string qcEmails = string.Join(",", qcUsers.Select(p => p.WindowsUserName + "@utahsbr.edu").ToArray());
			string sender = string.Format("{0}@utahsbr.edu", Environment.UserName);
			string subject = string.Format("Imaging System Daily QC - {0:MM/dd/yyyy}", DateTime.Now);
			if (geezers.Count() == 0)
			{
				string body = "The process was complete and no issues were identified.";
				SendMail(RI.TestMode, qcEmails, sender, subject, body, "", "", "", Common.EmailImportanceLevel.Normal, RI.TestMode);
			}
			else
			{
				subject += " - Action Required";
				StringBuilder bodyBuilder = new StringBuilder();
				bodyBuilder.Append("There were unexpected files found in the imaging folders.");
				bodyBuilder.Append("  The folders identified are outlined below along with the oldest file date found.");
				bodyBuilder.Append("  Please review these folders to address the imaging backlog issues.");
				bodyBuilder.AppendFormat("{0}{0}Folder Name\t\t\t\t\t\tFile Date/Time", Environment.NewLine);
				foreach (ImagingFolder geezer in geezers)
				{
					bodyBuilder.AppendFormat("{0}{1}\t{2:MM/dd/yyyy}", Environment.NewLine, geezer.Path, geezer.OldestFileCreateDate);
				}
				string body = bodyBuilder.ToString();
				SendMail(RI.TestMode, qcEmails, sender, subject, body, "", "", "", Common.EmailImportanceLevel.Normal, RI.TestMode);
				string imagingQcEmails = DataAccess.GetEmailRecipientString(RI.TestMode, "IMAGINGQC", DataAccessBase.EmailLookupOption.None);
				SendMail(RI.TestMode, imagingQcEmails, sender, subject, body, "", "", "", Common.EmailImportanceLevel.Normal, RI.TestMode);
			}
		}//SendNotifications()

		private void SetOldestFileDateForEachFolder(IEnumerable<ImagingFolder> folders)
		{
			foreach (ImagingFolder folder in folders)
			{
				//Use LINQ to get a collection of FileInfo.CreateTime properties
				//for all files/folders in the path and return the oldest one.
				string[] imagingFiles = Directory.GetFileSystemEntries(folder.Path);
				if (imagingFiles.Length > 0) { folder.OldestFileCreateDate = imagingFiles.Select<string, DateTime>(p => new FileInfo(p).CreationTime).Min(); }
			}
		}//SetOldestFileDateForEachFolder()
    }//class
}//namespace
