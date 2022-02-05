using Q;
using System.Windows.Forms;
using System.IO;

namespace SPLTRCAMP
{
    public class SPLTRCAMPN : BatchScriptBase
    {

        public SPLTRCAMPN(ReflectionInterface ri)
            : base(ri, "SPLTRCAMP")
        {
        }

        public override void Main()
        {
            while (true)
            {
                CampaignData data = new CampaignData();
                Entry entryForm = new Entry(data);
                if (entryForm.ShowDialog() == DialogResult.Cancel)
                {
                    EndDLLScript();
                }
                else
                {
                    CampaignProcessor Processor = new CampaignProcessor(RI, ScriptID, data);
                    Processor.RunCampaign();
                    if (File.Exists(Processor.ErrorLog))
                    {
                        MessageBox.Show(string.Format("An error report was create on {0}.  Please notify person who requested the script to be run.",DataAccessBase.PersonalDataDirectory),"Error Log",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    ProcessingComplete();
                }
            }
        }

    }
}
