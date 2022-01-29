using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CFSARPTFED
{
    class ReportsBase
    {
        protected ErrorReport Err { get; set; }
        protected ReportData Report { get; set; }

        protected Color Purple
        {
            get
            {
                return Color.FromArgb(102, 102, 153); 
            }

        }

        protected Font Italics
        {
            get
            {
                return new Font("Calibri", 10, FontStyle.Italic);
            }
        }
        protected Font FontWithBold
        {
            get
            {
                return new Font("Calibri", 9, FontStyle.Bold);
            }
        }
        protected Font FontWithNoBold
        {
            get
            {
                return new Font("Calibri", 9);
            }
        }

        protected Font ArialFontWithBold
        {
            get
            {
                return new Font("Arial", 10, FontStyle.Bold);
            }
        }

        protected Font ArialFontNoBold
        {
            get
            {
                return new Font("Arial", 10);
            }
        }

        public ReportsBase(ErrorReport err)
        {
            Err = err;
        }

        /// <summary>
        /// Creates a formatted string.  Used by various reports.
        /// </summary>
        /// <returns></returns>
        protected string CreateLoanTypeString()
        {
            string loanType = "The loan type code which the loan(s) belong to:" + Environment.NewLine +
                "- D1 = Direct Stafford Subsidized" + Environment.NewLine +
                "- D2 = Direct Stafford Unsubsidized" + Environment.NewLine +
                "- D3 = Direct Graduate PLUS" + Environment.NewLine +
                "- D4 = Direct PLUS" + Environment.NewLine +
                "- D5 = Direct Consolidation Unsubsidized" + Environment.NewLine +
                "- D6 = Direct Consolidation Subsidized" + Environment.NewLine +
                "- D7 = Direct PLUS Consolidation" + Environment.NewLine +
                "- D8 = TEACH loan converted from a TEACH grant" + Environment.NewLine +
                "- CL = FFEL Consolidation" + Environment.NewLine +
                "- GB = FFEL Graduate PLUS" + Environment.NewLine +
                "- PL = FFEL PLUS" + Environment.NewLine +
                "- RF = FFEL Refinanced" + Environment.NewLine +
                "- SF = FFEL Stafford Subsidized" + Environment.NewLine +
                "- SU = FFEL Stafford Unsubsidized" + Environment.NewLine +
                "- FI = Federally Insured (FISL)";

            return loanType;
        }

        /// <summary>
        /// Will get all SAS files needed for each report.  Will error is none or multiple are found.
        /// </summary>
        /// <returns>SAS files path and name if only one file is found.  If no files are found it will error and send an empty string.</returns>
        protected string GetTheSasFile()
        {
            string[] files = Directory.GetFiles(EnterpriseFileSystem.FtpFolder, Report.FileName);
            if (files.Count() > 1)
            {
                AddError("Multiple SAS files exist.");
                return string.Empty;
            }
            else if (files.Count() < 1)
            {
                AddError("No SAS files exist.");
                return string.Empty;
            }

            return files[0];
        }

        /// <summary>
        /// Displays a error to the user and adds a record to the error report.
        /// </summary>
        /// <param name="error">Error to add</param>
        protected void AddError(string error)
        {
            MessageBox.Show(string.Format("{0}.  Please review the error report, the script will continue to the next report.", error), "Multiple Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Err.AddRecord(string.Format("{0}", error), Report);
        }

        protected bool CheckMutlipleFiles(List<string> filesToProcess)
        {
            if (filesToProcess.Where(p => p.Contains("R2")).Count() > 1)
            {
                AddError("Multiple SAS files exist. R2");
                return false;
            }
            else if (filesToProcess.Where(p => p.Contains("R3")).Count() > 1)
            {
                AddError("Multiple SAS files exist. R3");
                return false;
            }
            else if (filesToProcess.Where(p => p.Contains("R2")).Count() > 1)
            {
                AddError("No SAS files exist. R2");
                return false;
            }
            else if (filesToProcess.Where(p => p.Contains("R3")).Count() > 1)
            {
                AddError("No SAS files exist. R3");
                return false;
            }

            return true;
        }
    }
}
