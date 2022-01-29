using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ScriptCoordinator
{
    static class Program
    {
        /// <summary>
        /// See ScriptCoordinator.Args documentation for list of arguments.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Form bringToFront = new Form() { Opacity = 0 })
                bringToFront.Show(); //this fixes an issue where scripts start up minimized.

            if (args.Any())
                LaunchWithArgs(args);
            else
                LaunchGui();
        }

        static void LaunchWithArgs(string[] args)
        {
            var validationResult = KvpArgValidator.ValidateArguments<Args>(args);
            if (!validationResult.IsValid)
                Dialog.Warning.Ok(validationResult.ValidationMesssage, "Script Coordinator - Invalid Arguments");
            else
            {
                var coordinator = new ScriptCoordinator(new Args(args));

                var commonCacheResult = coordinator.CacheCommonCode();
                if (!commonCacheResult.Successful)
                    Dialog.Warning.Ok(commonCacheResult.Message);
                else
                {
                    var scriptCacheResult = coordinator.CacheScript();
                    if (!scriptCacheResult.Successful)
                        Dialog.Warning.Ok(scriptCacheResult.Message);
                    else
                    {
                        coordinator.LaunchScript();
                        coordinator.Finish();
                    }
                }
            }
        }

        static void LaunchGui()
        {
            //Application.Run();
        }
    }
}
