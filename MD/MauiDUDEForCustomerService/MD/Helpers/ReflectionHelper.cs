using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Form = System.Windows.Forms.Form;
using CreateParams = System.Windows.Forms.CreateParams;
using Uheaa.Common;
using System.Runtime.InteropServices;
using Reflection;
using System.Diagnostics;
using System.Reflection;

namespace MD
{
    public class ReflectionHelper
    {
        #region Singleton
        private static ReflectionHelper instance = new ReflectionHelper();
        public static ReflectionHelper Instance { get { return instance; } }
        #endregion

        public static void ErrorHook()
        {
            //Quit the entire application if the reflection session goes down
            //TODO: Consider recovering with a new session if one goes down
            AppDomain.CurrentDomain.UnhandledException += (o, ea) =>
            {
                if (ea.ExceptionObject is COMException || ea.ExceptionObject is TargetException)
                {
                    try
                    {
                        Instance.CurrentSession.ReflectionSession.BDTIgnoreScrollLock = 1;
                    }
                    catch (Exception)
                    {
                        //we'll get an awful exception if the session is really no longer available.
                        //if it isn't available, it's time to jump ship
                        UIHelper.Instance.QuitApplication();
                    }
                }
            };
        }

        public void Instantiate()
        {
            CurrentSession = new ReflectionInterface(InstantiateSession());
            ReflectionHandle = FindWindowByCaption(IntPtr.Zero, CurrentSession.ReflectionSession.Caption);

            Thread validator = new Thread(ValidateSession);
            validator.SetApartmentState(ApartmentState.STA);
            validator.Start();
        }

        public IntPtr ReflectionHandle { get; private set; }
        public ReflectionInterface CurrentSession { get; private set; }

        public void SendMessage(string message)
        {
            if (CurrentSession == null) return;
            CurrentSession.ReflectionSession.Caption = message;
        }

        public void Kill()
        {
            try
            {
                SendMessage("CMD:KILL");
            }
            catch (COMException)
            {
                //eat this exception, session is already gone
            }
            catch (TargetException)
            {
                //eat this exception, session is already gone
            }
            CurrentSession = null;

        }

        //continually polls the session to ensure we're still logged in and not disconnected.
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
                            Hlpr.UI.DisableAllForms();
                            Hlpr.UI.CreateForm<LoginForm>((f) => f.Initialize(true)).ShowDialog();
                            Hlpr.UI.EnableAllForms();
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

        private ReflectionHelper()
        {

        }

        public Session InstantiateSession()
        {
            string newNameBase = "MD Session";
            string newName = newNameBase;

            int ordinal = 0;
            while (FindWindowByCaption(IntPtr.Zero, newName) != IntPtr.Zero)
            {
                ordinal++;
                newName = newNameBase + " (" + ordinal + ")";
            }
            if (ordinal != 0) BaseForm.SetReflectionOrdinal(ordinal);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "MdSession.exe");
            string guid = Guid.NewGuid().ToString().Replace("-", "");  //dashes make the guid too long for an OLEName
            Process.Start(path, $"{guid} {ordinal} {DataAccessHelper.CurrentRegion} {DataAccessHelper.CurrentMode}");

//TODO REMOVE ONCE THE OLD CODE IS VERIFIED TO WORK
//#if DEBUG
//            Proc.Start("MDSessionDebug", $"{guid} {ordinal} {DataAccessHelper.CurrentRegion} {DataAccessHelper.CurrentMode}");
//#else
//            Proc.Start("MDSession", $"{guid} {ordinal} {DataAccessHelper.CurrentRegion} {DataAccessHelper.CurrentMode}");
//#endif

            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds <= 30)
            {
                try
                {
                    var session = (Session)Microsoft.VisualBasic.Interaction.GetObject(guid, null);
                    session.BDTIgnoreScrollLock = 1;
                    session.Caption = newName;
                    return session;
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                }
            }
            System.Windows.Forms.MessageBox.Show("Unable to start MD Session");
            UIHelper.Instance.QuitApplication();
            return null;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    }
}
