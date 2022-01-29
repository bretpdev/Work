using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Windows.Forms;
using Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace MdSession
{
    [Serializable]
    public class ScriptLauncher
    {
        //TODO remove test mode since it shouldn't be needed
        public ScriptLauncher(string localLocation, string scriptId, string startingType, string reflectionGuid, bool isCSharp, bool testMode)
        {
            var session = (Session)Microsoft.VisualBasic.Interaction.GetObject(reflectionGuid, null);
            List<object> args = new List<object>();
            if (!isCSharp)
                args.Add(new Uheaa.Common.Scripts.ReflectionInterface(session));
            else
            {
                args.Add(new Uheaa.Common.Scripts.ReflectionInterface(session));
                DataAccessHelper.CurrentMode = testMode ? DataAccessHelper.Mode.Dev : DataAccessHelper.Mode.Live;
            }
            try
            {
                var handle = (Activator.CreateInstanceFrom(localLocation, startingType, true, BindingFlags.Default, null, args.ToArray(), null, null) as ObjectHandle);
                var script = handle.Unwrap();
                var main = script.GetType().GetMethod("Main");
                main.Invoke(script, null);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException;
                if (inner != null)
                {
                    if (inner is Uheaa.Common.Scripts.EndDLLException)
                        return; //successful
                    MessageBox.Show(string.Format("Unable to run script {0}.  Error: {1}", scriptId, inner.ToString()));
                }
            }
        }
    }
}
