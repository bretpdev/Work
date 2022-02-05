using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OPSWebEntry
{
    public class StateAwareTextBox : ExtendedTextBox, IStateAwareControl<string>
    {
        private ControlState<string> state;
        public StateAwareTextBox()
        {
            state = new ControlState<string>(this, () => this.Text);
            this.TextChanged += (o, ea) => { if (ValueChanged != null) ValueChanged(o, new EventArgs()); };
        }

        public new string Text
        {
            get { return base.Text; }
            set { state.Value = base.Text = value; }
        }
        public ControlState<string> State
        {
            get { return state; }
        }
        ControlState IStateAwareControl.State
        {
            get { return State; }
        }

        public string OriginalValue
        {
            get { return state.OriginalValue; }
        }
        object IStateAwareControl.OriginalValue
        {
            get { return OriginalValue; }
        }

        public string Value
        {
            get { return state.Value; }
            set { state.Value = value; }
        }

        object IStateAwareControl.Value
        {
            get { return Value; }
            set { Value = (string)value; }
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
