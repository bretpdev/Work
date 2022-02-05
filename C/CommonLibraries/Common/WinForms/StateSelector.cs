using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class StateSelector : ValidatableComboBox
    {
        [DefaultValue(false)]
        public bool AllowBlank { get; set; }

        private static List<State> States { get; set; }
        static StateSelector()
        {
            States = new List<State>();

        }
        private struct State
        {
            public string StateCode{get;set;}
            public string Description{get;set;}
        }
    }
}
