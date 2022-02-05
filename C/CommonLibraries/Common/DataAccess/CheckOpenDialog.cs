using System.IO;
using System.Windows.Forms;
using Uheaa.Common;

namespace Uheaa.Common.DataAccess
{
    public static class CheckOpenDialog
    {
        /// <summary>
        /// Opens a Dialog box and creates an OpenFileDialog object to return.
        /// </summary>
        /// <param name="fileName">The name of the file the script needs to open. This will only be used to check the file name
        /// against the file the user selects to make sure it is the correct file.</param>
        /// <param name="initialDirectory">Defaults to FTP but can be set to any other directory. It will check the region.</param>
        /// <param name="multiSelect">Defaulted to false</param>
        /// <param name="filter">Defaulted to All Files but can be set to open a specific file type.</param>
        /// <returns>OpenFileDialog object</returns>
        public static OpenFileDialog ShowDialog(string initialDirectory = null, bool multiSelect = false, string filter = "ALL Files (*.*)|*.*")
        {
            OpenFileDialog file = new OpenFileDialog();
            file.CheckFileExists = true;
            file.CheckPathExists = true;
            file.Filter = filter;
            file.InitialDirectory = initialDirectory == null ? EnterpriseFileSystem.FtpFolder : initialDirectory;
            file.Multiselect = multiSelect;

            if (file.ShowDialog() == DialogResult.Cancel)
                return null;

            if (!ValidateRegion(file.FileName))
                return null;

            if (CheckEmptyFile(file.FileName))
                return null;

            return file;
        }

        /// <summary>
        /// Validates that the file being open is in the correct region
        /// </summary>
        /// <param name="fileName"></param>
        private static bool ValidateRegion(string fileName)
        {
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
            {
                if (!FileSystemHelper.IsSecureLocation(fileName))
                {
                    MessageBox.Show("You must chose a file from a CornerStone drive.", "Wrong Drive Location", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks if the file is emtpy
        /// </summary>
        /// <param name="fileName">The file path and name</param>
        /// <returns>True if data exists, false if empty file</returns>
        private static bool CheckEmptyFile(string fileName)
        {
            if (new FileInfo(fileName).Length == 0)
            {
                MessageBox.Show("The file is empty", "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            return false;
        }
    }
}
