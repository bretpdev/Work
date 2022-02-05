using System.Collections.Generic;

namespace CentralizedPrintingProcess
{
    public abstract class PropsToStringBase
    {
        public override string ToString()
        {
            List<string> items = new List<string>();
            foreach (var prop in GetType().GetProperties())
                items.Add(prop.Name + ":" + prop.GetValue(this));
            return string.Join(";", items);
        }
    }
}