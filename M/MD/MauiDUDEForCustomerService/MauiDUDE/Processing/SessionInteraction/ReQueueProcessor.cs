using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace MauiDUDE
{
    class ReQueueProcessor
    {
        private string _ssn;
        private ReflectionInterface RI;
        public ReQueueProcessor(string ssn)
        {
            _ssn = ssn;
            RI = SessionInteractionComponents.RI;
        }

        public void Process()
        {
            ReQueue ui = new ReQueue();
            DialogResult result = ui.ShowDialog();

            //if form not cancelled then add task
            if(!ui.Data.FormCancelled && result == DialogResult.OK)
            {
                //add queue task
                RI.FastPath($"LP9OA{_ssn};;ZZZZCOMP");
                if(RI.GetText(22,3,5) == "48012")
                {
                    MessageBox.Show("Unable to Requeue task for borrower", "Failure", MessageBoxButtons.OK);
                    return;
                }
                RI.PutText(11, 25, ui.Data.ReQueueDate.ToString("MMddyyyy"));
                RI.PutText(16, 12, $"{_ssn}, DCALL, D, {ui.Data.Comments}");
                RI.Hit(Key.F6);
            }
        }
    }
}
