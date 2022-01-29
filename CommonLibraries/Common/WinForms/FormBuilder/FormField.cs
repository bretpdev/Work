using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public delegate void SetProperties(Control control);
    public interface IFormField : IFormField<Control> { }
    public interface IFormField<out T> where T : Control, new()
    {
        int? Ordinal { get; set; }
        string Label { get; set; }
        Type Type { get; }
        T GenerateControl();
        void FinalizeControl(object control);
    }
    public class FormField<T> : IFormField<T> where T : Control, new()
    {
        public string Label { get; set; }
        public Type Type { get { return typeof(T); } }
        public int? Ordinal { get; set; }
        private Action<T> SetProperties { get; set; }
        public T GenerateControl()
        {
            var control = new T();
            return control;
        }
        public void FinalizeControl(object control)
        {
            if (SetProperties != null)
                SetProperties((T)control);
        }
        public FormField(string label, Action<T> setProperties, int? ordinal = null)
        {
            Label = label;
            SetProperties = setProperties;
            Ordinal = ordinal;
        }
    }
}
