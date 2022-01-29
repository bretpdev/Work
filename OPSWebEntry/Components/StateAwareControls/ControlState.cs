using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OPSWebEntry
{
    public class ControlState<T> : ControlState
    {
        public ControlState(IStateAwareControl<T> parent, Expression<Func<T>> parentValue)
            : base(parent, GetProperty<T>(parentValue)) { }
        public new T OriginalValue
        {
            get { return (T)base.OriginalValue; }
        }
        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

    }
    public class ControlState
    {
        protected PropertyInfo valueProperty;
        protected IStateAwareControl parent;
        public ControlState(IStateAwareControl parent, Expression<Func<object>> parentValue)
            : this(parent, GetProperty<object>(parentValue)) { }
        public ControlState(IStateAwareControl parent, PropertyInfo parentValue)
        {
            this.valueProperty = parentValue;
            this.parent = parent;
            this.OriginalValue = this.Value;
            parent.ValueChanged += (o, ea) => SyncLabel();
        }

        public bool IsChanged
        {
            get
            {
                if (OriginalValue == null)
                    return this.Value != null;
                return !OriginalValue.Equals(this.Value);
            }
        }

        public object OriginalValue { get; internal set; }
        public object Value
        {
            get
            {
                return valueProperty.GetValue(parent, null);
            }
            set
            {
                if (!Value.Equals(value))
                    valueProperty.SetValue(parent, value, null);
                OriginalValue = value;
                SyncLabel();
            }
        }

        private Label label;
        public Label Label
        {
            get { return label; }
            set
            {
                label = value;
                SyncLabel();
            }
        }
        public void SyncLabel()
        {
            if (label != null)
                label.FontWeight = IsChanged ? FontWeights.Bold : FontWeights.Normal;
        }

        public void CancelChanges()
        {
            Value = OriginalValue;
        }

        public void AcceptChanges()
        {
            valueProperty.SetValue(parent, Value, null);
        }

        protected static PropertyInfo GetProperty<P>(Expression<Func<P>> expr)
        {
            var property = expr.Body as MemberExpression;
            var convert = expr.Body as UnaryExpression;  //some expressions have a conversion step in the middle
            if (convert != null) property = convert.Operand as MemberExpression;
            return (PropertyInfo)property.Member;
        }
    }
}
