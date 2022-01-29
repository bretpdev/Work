using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Uheaa.Common.Wpf
{
    public interface IStateAwareControl<T> : IStateAwareControl
    {
        new ControlState<T> State { get; }
        new T OriginalValue { get; }
        new T Value { get; set; }
    }

    public interface IStateAwareControl
    {
        ControlState State { get; }
        bool IsChanged { get; }
        object OriginalValue { get; }
        object Value { get; set; }

        Label Label { get; set; }
        event EventHandler ValueChanged;
        void CancelChanges();
        void AcceptChanges();
    }
}
