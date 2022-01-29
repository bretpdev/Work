using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace MD
{
    public class BaseForm : Form
    {
        private string originalText;
        public BaseForm()
            : base()
        {
            this.Font = new Font(new FontFamily("Arial"), 10, FontStyle.Regular);
            this.Icon = Properties.Resources.waveicon;
            this.Load += (o, ea) =>
            {
                originalText = this.Text;
                UpdateTitle();
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    ApplyContrast();
            };
        }

        private static int? mdOrdinal = null;
        public static void SetReflectionOrdinal(int ordinal)
        {
            mdOrdinal = ordinal;
            foreach (BaseForm bf in Hlpr.UI.Forms)
                bf.UpdateTitle();
        }

        private void UpdateTitle()
        {
            string md = "MD";
            if (mdOrdinal.HasValue)
                md += " (" + mdOrdinal.Value + ")";
            this.Text = md + " - " + this.Text;
            try
            {
                if (DataAccessHelper.TestMode)
                    this.Text = "[TEST MODE] " + this.Text;
            }
            catch (DataAccessHelper.ModeNotSetException)
            {
                //mode not set, eat the exception
            }
        }

        protected void Async(Action action)
        {
            Thread thread = new Thread(() =>
            {
                while (!base.IsHandleCreated)
                    Thread.Sleep(100);
                action();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        protected void BeginInvoke(Action action)
        {
            base.BeginInvoke(action);
        }

        protected void Invoke(Action action)
        {
            base.Invoke(action);
        }

        protected void AsyncInvoke(Action action)
        {
            Async(() => Invoke(action));
        }

        protected override void OnClosed(EventArgs e)
        {
            Hlpr.UI.RemoveForm(this);
            if (!Hlpr.UI.Forms.Any())
            {
                new Thread(() => Hlpr.UI.QuitApplication()).Start();
            }
            base.OnClosed(e);
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            var form = owner as BaseForm;
            if (form != null && form.TopMost)
                WinApiHelper.ForceTopMost(form);
            return base.ShowDialog(owner);
        }

        public void NormalContrast()
        {
            Color purple = Color.FromArgb(184, 174, 231);
            ApplyContrast(purple, Color.Black, SystemColors.Control, Color.Black);
        }

        public void HighContrast()
        {
            ApplyContrast(Color.White, Color.Black, Color.White, Color.Black);
        }

        public void ProfessionalContrast()
        {
            ApplyContrast(null, null, default(Color), Color.Gray);
        }

        public void ApplyContrast()
        {
            switch (CommonMenu.ContrastMode)
            {
                case ContrastModeEnum.Normal:
                    NormalContrast();
                    break;
                case ContrastModeEnum.HighContrast:
                    HighContrast();
                    break;
                case ContrastModeEnum.Professional:
                    ProfessionalContrast();
                    break;
            }
        }

        private void IfIs<T>(object o, Action<T> act) where T : class { if (o is T) act(o as T); }
        Dictionary<Control, Color> origForeColors = new Dictionary<Control, Color>();
        Dictionary<Control, Color> origBackColors = new Dictionary<Control, Color>();
        protected Color? CurrentBackColor = null;
        protected Color? CurrentForeColor = null;
        private void ApplyContrast(Color? backcolor, Color? forecolor, Color semigroundcolor, Color watermark)
        {
            CurrentBackColor = backcolor;
            CurrentForeColor = forecolor;
            Action<Control> standardBackColor = new Action<Control>(c =>
            {
                if (!origBackColors.ContainsKey(c))
                    origBackColors[c] = c.BackColor;
                if (backcolor.HasValue)
                    c.BackColor = backcolor.Value;
                else
                    c.BackColor = origBackColors[c];
            });
            Action<Control> standardForeColor = new Action<Control>(c =>
            {
                if (!origForeColors.ContainsKey(c))
                    origForeColors[c] = c.ForeColor;
                if (forecolor.HasValue)
                    c.ForeColor = forecolor.Value;
                else
                    c.ForeColor = origForeColors[c];
            });
            Action<Control> standard = new Action<Control>(c =>
            {
                standardBackColor(c);
                standardForeColor(c);
            });
            WatermarkTextBox.WatermarkColor = watermark;
            //iterate in reverse so that children get their colors saved/set before parents (so no color inheritance interferes)
            foreach (Control c in this.Controls.Cast<Control>().Recurse(o => o.Controls.Cast<Control>()).Reverse())
            {
                IfIs<WatermarkTextBox>(c, o => o.SyncUI());
                IfIs<TabPage>(c, o => standard(o));
                IfIs<Panel>(c, o => standard(o));
                IfIs<UtIdTextBox>(c, o => standard(o));
                IfIs<Button>(c, o =>
                {
                    standardForeColor(o);
                    o.BackColor = semigroundcolor;
                    if (semigroundcolor == default(Color))
                        o.UseVisualStyleBackColor = true;
                });
                IfIs<CommonMenu>(c, o => o.BackColor = semigroundcolor);
            }
            standard(this);
            ContrastApplied();
        }

        protected virtual void ContrastApplied()
        {
            //do nothing here, derived classes will override
        }
    }
}
