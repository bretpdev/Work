using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class IsValidDirectoryBehavior<T> : Behavior where T : IHasValue, IHasBackColor
    {
        Color normalBackground = Color.White;
        Color invalidBackground = Color.Pink;
        T control;
        public bool IsValidDirectory{ get { return Directory.Exists(control.Value.ToString());}}
        public IsValidDirectoryBehavior(T control)
        {
            this.control = control;
        }

        void Control_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SyncColors();
        }

        public void SyncColors()
        {
            if (IsValidDirectory)
                control.BackColor = normalBackground;
            else
                control.BackColor = invalidBackground;
        }
        public override string FriendlyName
        {
            get { return "Is Valid Directory"; }
        }

        #region Installation Methods
        public override void Install()
        {
            control.ValueChanged += Control_ValueChanged;
            SyncColors();
        }
        public override void Uninstall()
        {
            control.ValueChanged -= Control_ValueChanged;
        }
        #endregion
    }
}
