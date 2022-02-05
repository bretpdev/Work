using System;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CRTREFQTSK
{
    public class CreateReferenceQueueTask : ScriptBase
    {
        public CreateReferenceQueueTask(ReflectionInterface ri)
            : base(ri, "CRTREFQTSK", DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            Dialog.Def.Ok("This script creates MREFRADD queue tasks for Compass references not on OneLINK listed in a SAS file.  Click OK to continue or cancel to quit.");

            string pattern = "ULWK08.LWK08R2.*.*";

            foreach (var sasFileName in Directory.GetFiles(EnterpriseFileSystem.FtpFolder, pattern))
            {
                if (string.IsNullOrEmpty(sasFileName) || string.IsNullOrEmpty(File.ReadAllText(sasFileName).Trim()))
                    Dialog.Error.Ok("The " + pattern + " file was not found or was empty.");
                else
                {
                    using (StreamReader fileReader = new StreamReader(sasFileName))
                    {
                        string line = null;
                        while ((line = fileReader.ReadLine()) != null)
                        {
                            var record = new Demographics(line);
                            base.AddQueueTaskInLP9O(record.SSN, "MREFRADD", null, record.BuildComment(), "", "", "");
                        }
                    }
                    File.Delete(sasFileName);
                }
            }
            Dialog.Info.Ok("Processing Complete", this.ScriptId);
        }
    }
}
