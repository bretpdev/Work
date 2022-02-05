using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;

namespace OLDEMOS
{
    public class UIHelper
    {
        #region Instance
        private static readonly UIHelper instance = new UIHelper();
        public static UIHelper Instance { get { return instance; } }
        #endregion

        public bool IsRunning { get; set; }
        private readonly Dictionary<Guid, Type> pendingFormQueue = new Dictionary<Guid, Type>();
        private readonly Dictionary<Guid, BaseForm> pendingForms = new Dictionary<Guid, BaseForm>();
        private readonly List<BaseForm> forms = new List<BaseForm>();
        public IEnumerable<BaseForm> Forms { get { return forms.ToArray(); } }

        private readonly Thread UIThread;
        private UIHelper()
        {
            IsRunning = true;
            UIThread = new Thread(UISync);
            UIThread.SetApartmentState(ApartmentState.STA);
            UIThread.Start();
        }

        private void UISync()
        {
            while (IsRunning)
            {
                lock (pendingFormQueue)
                    foreach (var token in pendingFormQueue.Keys.ToArray())
                    {
                        var type = pendingFormQueue[token];
                        pendingFormQueue.Remove(token);

                        var form = (BaseForm)Activator.CreateInstance(type);
                        lock (pendingForms)
                        {
                            pendingForms[token] = form;
                            forms.Add(form);
                        }
                    }
                Sleep.ForOneSecond();
            }
        }

        public bool AnyFormVisible { get { return Forms.Any(o => o.IsHandleCreated && o.Visible); } }

        public void QuitApplication()
        {
            IsRunning = false;
            foreach (BaseForm bf in Forms.ToArray())
                if (bf.IsHandleCreated)
                    bf.Close();
            Application.Exit();
            Environment.Exit(1);
        }

        public bool LoginFormVisible
        {
            get
            {
                var types = new Type[] { typeof(LoginForm), typeof(ConsentForm) };
                return Forms.Any(o => types.Contains(o.GetType()) && o.IsHandleCreated && o.Visible);
            }
        }

        public void DisableAllForms() { ToggleFormsEnabled(false); }
        public void EnableAllForms() { ToggleFormsEnabled(true); }
        private void ToggleFormsEnabled(bool enabled = false)
        {
            foreach (BaseForm bf in Forms.Where(f => f.IsHandleCreated))
                bf.Invoke(new Action(() => bf.Enabled = enabled));
        }

        #region Form Lifecycle
        public T CreateAndShowDialog<T>(Action<T> initializeAction = null, IWin32Window owner = null) where T : BaseForm
        {
            T form = CreateForm<T>(initializeAction);
            form.ShowDialog(owner);
            return form;
        }

        public T CreateAndShow<T>(Action<T> initializeAction = null) where T : BaseForm
        {
            T form = CreateForm<T>(initializeAction);
            form.Show();
            return form;
        }

        public T CreateForm<T>(Action<T> initializeAction = null) where T : BaseForm
        {
            Guid token = Guid.NewGuid();
            //add token to queue
            lock (pendingFormQueue)
                pendingFormQueue.Add(token, typeof(T));
            //wait until main thread picks up token and creates form
            while (!pendingForms.ContainsKey(token))
                Thread.Sleep(100);
            //retrieve form based on token, then remove from queue
            lock (pendingForms)
            {
                var form = (T)pendingForms[token];
                pendingForms.Remove(token);
                if (initializeAction != null)
                    initializeAction(form);
                return form;
            }
        }

        public void RemoveForm(BaseForm bf)
        {
            forms.Remove(bf);
        }
        #endregion
    }
}