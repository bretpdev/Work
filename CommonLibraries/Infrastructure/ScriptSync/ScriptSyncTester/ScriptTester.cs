using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ScriptSyncTester
{
    public class ScriptTester : MarshalByRefObject
    {
        public void Test(ManagedReflectionInterface ri, ScriptTest test)
        {
            ScriptSync ss = new ScriptSync();
            DataAccessHelper.CurrentMode = test.IsLive ? DataAccessHelper.Mode.Live : DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = test.IsFed ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa;
            if (!ri.CheckForText(16, 2, "LOGON"))
                ri.LogOut();
            while (!ri.Login(InsecureCredentials.Username, InsecureCredentials.Password))
                if (MessageBox.Show("Unable to login for testing, please correct any login errors.", "Login Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                    return;
            try
            {
                if (ss.InstantiateScript(ri.ReflectionSession, (int)DataAccessHelper.CurrentMode, test.Script) != null)
                    test.TestResults = TestResult.Success;
                else
                    test.TestResults = TestResult.Failure;
            }
            catch (InvalidConstructorException)
            {
                test.TestResults = TestResult.BadConstructor;
            }
            catch (IncorrectRegionException)
            {
                test.TestResults = TestResult.BadRegion;
            }
        }
    }
}
