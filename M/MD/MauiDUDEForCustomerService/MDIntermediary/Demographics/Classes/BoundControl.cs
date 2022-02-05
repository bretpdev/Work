using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace MDIntermediary
{
    public class BoundControl
    {
        public Control Control { get; set; }
        public Label Label { get; set; }
        public string OriginalValue { get; set; }
        public MethodInfo ControlGetter { get; set; }
        public MethodInfo ControlSetter { get; set; }
        public MethodInfo ValueGetter { get; set; }
        public MethodInfo ValueSetter { get; set; }
        public bool Required { get; set; }
        public Action SetState { get; set; }
        public string GetControlValue()
        {
            return (string)ControlGetter.Invoke(Control, null);
        }
        public void SetControlValue(string value)
        {
            ControlSetter.Invoke(Control, new object[] { value });
        }
        public string GetValue(MDBorrowerDemographics demographics)
        {
            return (string)ValueGetter.Invoke(demographics, null);
        }
        public void SetValue(MDBorrowerDemographics demographics, string value)
        {
            ValueSetter.Invoke(demographics, new object[] { value });
        }
    }
}
