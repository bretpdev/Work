using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace MDIntermediary
{
    /// <summary>
    /// Manages the relationship between a given VerificationState control,
    /// and one or more Control/Label/OriginalValue combinations.
    /// </summary>
    public class BindManager
    {
        public VerificationState StateControl { get; private set; }
        public MDBorrowerDemographics Demographics { get; private set; }
        public List<BoundControl> Controls { get; private set; }
        public bool InitiallyInvalid { get; set; }
        public bool ResetSelectionToInvalid { get; set; } = false;
        public BindManager(VerificationState stateControl, MDBorrowerDemographics demographics, bool initiallyInvalid, bool resetSelectionToInvalid = false)
        {
            StateControl = stateControl;
            Demographics = demographics;
            InitiallyInvalid = initiallyInvalid;
            ResetSelectionToInvalid = resetSelectionToInvalid;
            Controls = new List<BoundControl>();
            StateControl.RevertChanges += this.RevertChangesHandler;
        }

        public bool ModificationInProgress { get; private set; }
        private void RevertChangesHandler()
        {
            ModificationInProgress = true;
            foreach (var boundControl in Controls)
            {
                boundControl.ControlSetter.Invoke(boundControl.Control, new object[] { boundControl.OriginalValue });
                //above line equivalent of: control.Text = boundControl.OriginalValue;
            }
            foreach (var boundControl in Controls)
            {
                boundControl.Label.MakeFontRegular();
                if (string.IsNullOrEmpty(boundControl.GetValue(Demographics)) || ResetSelectionToInvalid)
                    boundControl.SetState();
            }
            ModificationInProgress = false;
        }

        public bool Enabled
        {
            get { return StateControl.Enabled; }
            set
            {
                StateControl.Enabled = value;
                foreach (BoundControl bc in Controls)
                {
                    bc.Control.Enabled = value;
                }
            }
        }

        /// <summary>
        /// Set all bound properties of the Demographics object to the values of their associated controls.
        /// If a different demographics object is passed in, that object will be persisted instead of the original
        /// </summary>
        public void PersistChanges(MDBorrowerDemographics demographics = null)
        {
            foreach (var control in this.Controls)
            {
                var value = control.ControlGetter.Invoke(control.Control, null);
                control.ValueSetter.Invoke(demographics ?? Demographics, new object[] { value });
            }
        }
        /// <summary>
        /// Set all control values back to their originals.  Set the verification control back to No Changes.
        /// </summary>
        public void RevertChanges(bool overrideRefused = false)
        {
            RevertChangesHandler();
            StateControl.NoChangesMode(overrideRefused);
            if (InitiallyInvalid)
                StateControl.InvalidSelection();
            foreach (var control in Controls)
                control.SetState();
        }
        /// <summary>
        /// Add the given Control, its Label, and its original value to the manager.  Ties to the .Text property by default.
        /// </summary>
        /// <param name="control">Changes to this control will trigger validation.</param>
        /// <param name="label">When changes are made to the control, this label will become bold.</param>
        /// <param name="valuePropert">A reference to the property on the Demographics object that corresponds with this control.</param>
        /// <returns>A reference to this BindManager, for use as a semi-fluent api.</returns>
        public BindManager Add(Control control, Label label, Expression<Func<MDBorrowerDemographics, string>> valueProperty, bool required = true)
        {
            return Add<Control>(control, label, c => c.Text, valueProperty, required);
        }
        /// <summary>
        /// Add the given Control, its Label, and its original value to the manager.
        /// Use this overload to specify a Control Property other than .Text
        /// </summary>
        /// <param name="control">Changes to this control will trigger validation.</param>
        /// <param name="label">When changes are made to the control, this label will become bold.</param>
        /// <param name="controlProperty">A reference to the property on the Control that corresponds with this Demographics object.</param>
        /// <param name="valuePropert">A reference to the property on the Demographics object that corresponds with this control.</param>
        /// <returns>A reference to this BindManager, for use as a semi-fluent api.</returns>
        public BindManager Add<T>(T control, Label label, Expression<Func<T, string>> controlProperty, Expression<Func<MDBorrowerDemographics, string>> valueProperty, bool required = true) where T : Control
        {
            var controlProp = (controlProperty.Body as MemberExpression).Member as PropertyInfo;
            var valueProp = (valueProperty.Body as MemberExpression).Member as PropertyInfo;
            return Add(control, label, controlProp.GetGetMethod(), controlProp.GetSetMethod(), valueProp.GetGetMethod(), valueProp.GetSetMethod(), required);
        }
        private BindManager Add(Control control, Label label, MethodInfo controlGetter, MethodInfo controlSetter, MethodInfo valueGetter, MethodInfo valueSetter, bool required = true)
        {
            var boundControl = new BoundControl()
            {
                Control = control,
                Label = label,
                ControlGetter = controlGetter,
                ControlSetter = controlSetter,
                ValueGetter = valueGetter,
                ValueSetter = valueSetter,
                OriginalValue = ((string)valueGetter.Invoke(Demographics, null) ?? "").ToUpper(),
                Required = required
            };
            Controls.Add(boundControl);
            boundControl.ControlSetter.Invoke(control, new object[] { boundControl.OriginalValue });
            //above line equivalent of: control.Text = boundControl.OriginalValue;

            var setState = new Action(() =>
            {
                bool allRequiredFieldsFilled = true;
                bool anyChanges = false;
                bool hasChanges = false;
                foreach (var c in Controls)
                {
                    if (c.Required && string.IsNullOrEmpty(c.GetControlValue()))
                        allRequiredFieldsFilled = false;
                    if (c.GetControlValue() != (c.OriginalValue ?? ""))
                    {
                        anyChanges = true;
                        if (c.Label == label) //control connected to same label
                            hasChanges = true;
                    }
                }
                if (!allRequiredFieldsFilled)
                    StateControl.InvalidDataMode();
                else if (anyChanges)
                    StateControl.ChangesMode();
                else
                {
                    StateControl.NoChangesMode();
                    if (InitiallyInvalid && StateControl.Selection != VerificationSelection.RefusedNoChange)
                        StateControl.InvalidSelection();
                }

                if (hasChanges)
                    label.MakeFontBold();
                else
                    label.MakeFontRegular();
            });
            boundControl.SetState = setState;
            control.TextChanged += (o, ea) =>
            {
                if (!ModificationInProgress)
                    setState();
            };
            setState();
            return this;
        }
    }


}
