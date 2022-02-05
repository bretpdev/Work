using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class TrackChangedValueBehavior<T> : Behavior where T : IHasValue, IHasBackColor
    {
        Color normalBackground = Color.White;
        Color changedBackground = Color.LightYellow;
        T control;
        public object InitialValue { get; internal set; }
        public object CurrentValue { get { return control.Value; } }
        public bool HasChanged { get { return !control.Value.Equals(InitialValue); } }
        public TrackChangedValueBehavior(T control)
        {
            this.control = control;
        }

        void Control_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SyncColors();
        }

        void Control_ValueSet(object sender, ValueSetEventArgs e)
        {
            Reset(e.Value);
        }

        public void SyncColors()
        {
            if (HasChanged)
                control.BackColor = changedBackground;
            else
                control.BackColor = normalBackground;
        }

        public void Reset(object value)
        {
            InitialValue = value;
            SyncColors();
        }
        public void Reset()
        {
            Reset(control.Value);
        }

        public override string FriendlyName
        {
            get { return "Track Changed Value"; }
        }

        #region Installation Methods
        public override void Install()
        {
            control.ValueChanged += Control_ValueChanged;
            control.ValueSet += Control_ValueSet;
            Reset();
        }
        public override void Uninstall()
        {
            control.ValueChanged -= Control_ValueChanged;
            control.ValueSet -= Control_ValueSet;
            control.BackColor = normalBackground;
        }
        #endregion
    }
}
