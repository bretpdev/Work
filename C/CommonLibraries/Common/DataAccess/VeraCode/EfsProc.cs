using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.DataAccess
{
    /// <summary>
    /// A wrapper around System.Diagnostics.Process combined with EnterpriseFileSystem.  
    /// Mitigated in VeraCode
    /// </summary>
    public static class Proc
    {
        //This Start Method is condensed to solve some security issues.
        //Allowing a method to take a ProcessStartInfo as a parameter assumes that the security is compromised because the method allows direct control of the content
        //Of the Process to start, by forcing the method to consume a string instead makes sure that the process is authenticated and not injected
        //The arguments parameter's default is a blank string so we initialize it to a blank string if no argument is provided.
        public static Process Start(string efsKey, string arguments = "", bool? useShellExecute = null, bool? createNoWindow = null, bool? redirectStandardError = null)
        {
            if (arguments == "" && !useShellExecute.HasValue && !createNoWindow.HasValue && !redirectStandardError.HasValue)
            {
                var psi = new ProcessStartInfo(EnterpriseFileSystem.GetPath(efsKey));
                return Process.Start(psi);
            }
            else
            {
                var psi = new ProcessStartInfo(EnterpriseFileSystem.GetPath(efsKey), arguments)
                {
                    UseShellExecute = useShellExecute.HasValue ? useShellExecute.Value : false,
                    CreateNoWindow = createNoWindow.HasValue ? createNoWindow.Value : false,
                    RedirectStandardError = redirectStandardError.HasValue ? redirectStandardError.Value : false
                };
                return Process.Start(psi);
            }
        }
    }
}
