using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace QCDBUser
{
    public class QCDBUser : ScriptCommonBase
    {
        private List<string> _dbRecords;
        private List<String> _QCUserID;
        private TestModeResults _directories;
        public string userID;

        // public QCDBUser(ReflectionInterface ri)
        //    : base(ri, "QCDBUser")   
        //{
        //}

        public QCDBUser(bool tTestMode)
            : base(tTestMode)
        {
        }

        //public QCDBUser(ReflectionInterface ri, MDBorrower mdBorr, int runNum)
        //    : base(ri, "QCDBUser", mdBorr, runNum)
        //{
        //}

        public void Main()
        {
            //userID = GetUserIDFromLP40();

            userID = Environment.UserName.ToString();
            _QCUserID = DataAccess.GetUserID(Convert.ToBoolean(Properties.Resources.TestingMode), userID);
            userID = _QCUserID.ElementAt(0).ToString();

            frmGetBU QCform;
            QCform = new frmGetBU(Convert.ToBoolean(Properties.Resources.TestingMode));
            if (QCform.ShowDialog() == DialogResult.Cancel)
            {
                EndDLLScript();
            }

            frmViewQC QCprocess;

            QCprocess = new frmViewQC(QCform._busUnit, Convert.ToBoolean(Properties.Resources.TestingMode), userID);
            if (QCprocess.noIncidents == false) // There are records to view
            {
                if (QCprocess.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBox.Show("Script cancelled by user.");

                    EndDLLScript();
                }
                //MessageBox.Show("Processing Complete.");
            }

            if (QCprocess.repeat)
            {
                Main();
            }


            EndDLLScript();
        }


    }
}
