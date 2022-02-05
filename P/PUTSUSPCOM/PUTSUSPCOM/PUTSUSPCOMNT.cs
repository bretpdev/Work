using Q;
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public class PUTSUSPCOMNT : ScriptBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ri">Reflection Interface</param>
        public PUTSUSPCOMNT(ReflectionInterface ri)
            : base(ri)
        {
        }

        public override void Main()
        {
            //check if the user is on target screen of TS3I
            if (Check4Text(1, 72, "TSX3G") == false)
            {
                //prompt user and end script
                MessageBox.Show("This is the the PUT Suspense Comments script.  You must start the script on the TS3I target screen.","Wrong Screen",MessageBoxButtons.OK,MessageBoxIcon.Information);
                EndDLLScript();
            }
            //collect needed information
            Suspense data = new Suspense();
            data.CTS3IGatheredAccountNumber = GetText(15,2,10);
            data.EffectiveDate = GetText(15, 25, 8).Replace(" ", @"/");
            data.TransactionType = GetText(15, 37, 2);
            data.TransactionSubType = GetText(15, 42, 2);
            data.Amount = GetText(15, 13, 11);
            data.AssignedTo = GetText(5, 70, 10);
            data.BatchNumber = GetText(3, 23, 16);
            data.SequenceNumber = GetText(3,56,7);
            data.BatchCode = GetText(4, 51, 8);
            data.FastPathText = GetText(1, 9, 30);
            data.BorrowerDemos = GetDemographicsFromTX1J(data.CTS3IGatheredAccountNumber);
            Entry entryFrm = new Entry(data,RI);
            if (entryFrm.ShowDialog() == DialogResult.OK)
            {
                entryFrm.ProcessingOption.Process();
                EndDLLScript();
            }
            else
            {
                EndDLLScript();
            }
        }

    }
}
