using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Uheaa.Common.WinForms
{
    public class CycleOptionCollection<T> : ObservableCollection<CycleOption<T>>
    {
        public void AddRange(IEnumerable<CycleOption<T>> range)
        {
            foreach (var option in range)
                base.Add(option);
        }
    }
}