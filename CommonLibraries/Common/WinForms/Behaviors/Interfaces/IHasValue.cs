using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// Use this as a common interface between controls.  Most controls will have to be modified to accomodate this interface.
    /// </summary>
    public interface IHasValue
    {
        object Value { get; set; }
        event OnValueChanged ValueChanged;
        event OnValueSet ValueSet;
    }
    public delegate void OnValueChanged(object sender, ValueChangedEventArgs e);
    public class ValueChangedEventArgs
    {
        public object NewValue { get; set; }
        public ValueChangedEventArgs(object value)
        {
            NewValue = value;
        }
    }
    public delegate void OnValueSet(object sender, ValueSetEventArgs e);
    public class ValueSetEventArgs
    {
        public object Value { get; set; }
        public ValueSetEventArgs() { }
        public ValueSetEventArgs(object value)
        {
            Value = value;
        }
    }
}
