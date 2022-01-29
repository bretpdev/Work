using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class RecipientComboBoxItem : ComboBoxItem<CallRecipientTarget>
    {
        public Reference Reference { get; set; }

        public RecipientComboBoxItem() : base() { }
        public RecipientComboBoxItem(CallRecipientTarget value, string text, Reference reference = null)
            : base(value, text)
        {
            this.Reference = reference;
        }
    }

    public class ComboBoxItem<T> where T : struct
    {
        public T? Value { get; set; }
        public string Text { get; set; }
        public ComboBoxItem() { Text = ""; }
        public ComboBoxItem(T value, string text)
        {
            this.Value = value;
            this.Text = text;
        }
        public override bool Equals(object obj)
        {
            var c = obj as ComboBoxItem<T>;
            if (c == null)
                return false;
            if (this.Value == null)
                return false;
            if (c.Value == null)
                return false;
            return c.Value.Value.Equals(this.Value.Value);
        }

        public override int GetHashCode()
        {
            if (this.Value == null)
                return base.GetHashCode();
            else
                return this.Value.GetHashCode(); 
        }
    }
    public class ContactSource
    {
        public string ContactCode { get; set; }
        public string Lp22Source { get; set; }
        public string Tx1jSource { get; set; }
        public ContactSource(string contactCode, string lp22Source, string tx1jSource)
        {
            this.ContactCode = contactCode;
            this.Lp22Source = lp22Source;
            this.Tx1jSource = tx1jSource;
        }
    }
}
