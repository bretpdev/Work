using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Uheaa.Common.Wpf
{
    public class StateAwareToggleButton : ExtendedToggleButton, IStateAwareControl<bool?>
    {
        private ControlState<bool?> state;
        public StateAwareToggleButton()
        {
            state = new ControlState<bool?>(this, () => this.IsChecked);
            this.Checked += (o, ea) => { if (ValueChanged != null) ValueChanged(o, ea); };
            this.Unchecked += (o, ea) => { if (ValueChanged != null) ValueChanged(o, ea); };
        }

        public new bool? IsChecked
        {
            get { return base.IsChecked; }
            set { state.Value = base.IsChecked = value; ValueChanged(null, null); }
        }

        public ControlState<bool?> State
        {
            get { return state; }
        }
        ControlState IStateAwareControl.State
        {
            get { return State; }
        }

        public bool? OriginalValue
        {
            get { return state.OriginalValue; }
        }
        object IStateAwareControl.OriginalValue
        {
            get { return OriginalValue; }
        }

        public bool? Value
        {
            get { return state.Value; }
            set { state.Value = value; }
        }

        object IStateAwareControl.Value
        {
            get { return Value; }
            set { Value = (bool?)value; }
        }

        public bool IsChanged
        {
            get { return state.IsChanged; }
        }

        public Label Label
        {
            get { return state.Label; }
            set { state.Label = value; }
        }

        public event EventHandler ValueChanged;

        public void CancelChanges()
        {
            state.CancelChanges();
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        public void AcceptChanges()
        {
            state.AcceptChanges();
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }
    }
}
