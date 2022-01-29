using Reflection;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace OLDEMOS
{
    public class ReflectionHelper
    {
        #region Singleton
        private static readonly ReflectionHelper instance = new ReflectionHelper();
        public static ReflectionHelper Instance { get { return instance; } }
        #endregion

        public ReflectionInterface CurrentSession { get; private set; }
        public IntPtr ReflectionHandle { get; private set; }

        public void Instantiate()
        {
            CurrentSession = new ReflectionInterface(InstantiateSession());
            ReflectionHandle = FindWindowByCaption(IntPtr.Zero, CurrentSession.ReflectionSession.Caption);

            Thread validator = new Thread(ValidateSession);
            validator.SetApartmentState(ApartmentState.STA);
            validator.Start();
        }

        private void ValidateSession()
        {
            while (CurrentSession != null)
            {
                try
                {
                    if (CurrentSession.Check(Screen.LoginScreen) || CurrentSession.Check(Screen.FirstScreen) || CurrentSession.ReflectionSession.Connected == 0)
                    {
                        if (!UIHelper.Instance.LoginFormVisible && UIHelper.Instance.AnyFormVisible)
                        {
                            Helper.UI.DisableAllForms();
                            Helper.UI.CreateForm<LoginForm>((f) => f.Initialize()).ShowDialog();
                            Helper.UI.EnableAllForms();
                        }
                    }
                }
                //session has been killed, time to abort
                catch (InvalidComObjectException)
                {
                    break;
                }
                catch (COMException)
                {
                    break;
                }
                catch (Exception)
                {
                    //The Session might have been closed.  If so, we've got a problem.
                    break;
                }
                Sleep.For(5).Seconds();
            }
        }

        static readonly object sessionLock = new object();
        public Session InstantiateSession()
        {
            Session ses;
            lock (sessionLock)
            {
                string session = Path.Combine(EnterpriseFileSystem.GetPath("Sessions"), $"LoanManagement{(DataAccessHelper.TestMode ? "Tst" : "")}.rsf");
                ProcessStartInfo psi = new ProcessStartInfo(session);
                Process.Start(psi);
                Thread.Sleep(new TimeSpan(0, 0, 3));

                while (true)
                {
                    try
                    {
                        ses = ReflectionInterface.OpenExistingSession(ReflectionInterface.Flag.OpenSession);
                        break;
                    }
                    catch (Exception)
                    {
                        // There were no open sessions
                    }
                }
            }
            return ses;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    }
}