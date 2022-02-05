using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class RegionSelectionCycleButton : EnumCycleButton<RegionSelectionEnum>
    {
        public bool UheaaEnabled
        {
            get
            {
                return Options.Any(o => o.Value == RegionSelectionEnum.Uheaa);
            }
            set
            {
                if (!value)
                    RemoveOption(RegionSelectionEnum.Uheaa);
                else
                    AddOption(RegionSelectionEnum.Uheaa);
            }
        }

        public bool OnelinkEnabled
        {
            get
            {
                return Options.Any(o => o.Value == RegionSelectionEnum.OneLINK);
            }
            set
            {
                if (!value)
                    RemoveOption(RegionSelectionEnum.OneLINK);
                else
                    AddOption(RegionSelectionEnum.OneLINK);
            }
        }

        private void RemoveOption(RegionSelectionEnum option)
        {
            var selectedItem = Options[SelectedIndex];
            var found = Options.SingleOrDefault(o => o.Value == option);
            if (found != null)
            {
                Options.Remove(found);
                var index = Options.IndexOf(selectedItem);
                SelectedIndex = index;
            }
        }

        private void AddOption(RegionSelectionEnum option)
        {
            var found = Options.SingleOrDefault(o => o.Value == option);
            if (found == null)
                Options.Add(new CycleOption<RegionSelectionEnum>() { Label = option.ToString(), Value = option });
        }
    }
}
