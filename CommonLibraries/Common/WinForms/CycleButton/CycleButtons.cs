using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.WinForms
{
    public class StringCycleButton : CycleButton<string> { }
    public class BoolCycleButton : CycleButton<bool> { }
    public class YesNoButton : BoolCycleButton
    {
        public YesNoButton()
            : base()
        {
            Options.Add(new CycleOption<bool>() { Label = "Yes", Value = true });
            Options.Add(new CycleOption<bool>() { Label = "No", Value = false });
        }
    }
    public class EnumCycleButton<T> : CycleButton<T> where T : struct
    {
        public EnumCycleButton()
            : base()
        {
            foreach (T value in Enum.GetValues(typeof(T)))
                Options.Add(new CycleOption<T>() { Label = value.GetDescription().IsNull(o => o.Description, value.ToString()), Value = value });
        }
    }
    public class EnumCycleButton : CycleButton<int>
    {
        private Type enumType;
        public Type EnumType
        {
            get { return enumType; }
            set
            {
                enumType = value;
                LoadValues();
            }
        }
        private void LoadValues()
        {
            foreach (int value in Enum.GetValues(enumType))
            {
                Options.Add(new CycleOption<int>() { Label = value.GetDescription().IsNull(o => o.Description, value.ToString()), Value = value });
            }
        }
    }
}
