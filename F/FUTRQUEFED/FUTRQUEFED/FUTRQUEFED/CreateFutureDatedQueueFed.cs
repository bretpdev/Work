using System;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace FUTRQUEFED
{
    public class CreateFutureDatedQueueFed : FedScriptBase
    {
        public CreateFutureDatedQueueFed(ReflectionInterface ri)
            : base(ri, "FUTRQUEFED", Region.CornerStone)
        {
        }

        public CreateFutureDatedQueueFed(ReflectionInterface ri, MDBorrower borrower, int runNumber)
            : base(ri, "FUTRQUEFED", Region.CornerStone, borrower, runNumber)
        {
        }

        public override void Main()
        {
            //create object to hold queue information
            QueueInfo queInfo = new QueueInfo();

            //add the account number from Maui DUDE to the que information
			if (CalledByMauiDUDE)
			{
                queInfo.AccountNumber = MauiDUDEBorrower.CLAccNum;
			}

            //create and display the entry form
            QueueInfoEntry infoEntryForm = new QueueInfoEntry(queInfo);
            if (infoEntryForm.ShowDialog() == DialogResult.Cancel) { EndDLLScript(); }

            //check for duplicate requests
            if (DataAccess.HasDuplicateRequest(RI.TestMode, queInfo))
            {
                if (MessageBox.Show("Future Queue Request already exists for borrower/recipient. Are you sure you want to continue?", "Request Already Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { EndDLLScript(); }
            }

            //try to add the record and warn the user of the result
            string result = DataAccess.AddRequest(RI.TestMode, queInfo);
            if (result != string.Empty)
            {
                MessageBox.Show(string.Format("Error adding data to ARC Add Database {0}.",result),"Database Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Process complete.", "Process Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }//public override void Main
    }
}
