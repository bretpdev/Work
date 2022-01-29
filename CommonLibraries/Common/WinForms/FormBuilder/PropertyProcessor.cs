using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    class PropertyProcessor
    {
        public PropertyProcessor(FormBuilder fb, object generateFrom, PropertyInfo property, string label, int? ordinal)
        {
            Builder = fb;
            SourceObject = generateFrom;
            Property = property;
            Label = label;
            Ordinal = ordinal;
        }
        int? Ordinal { get; set; }
        FormBuilder Builder { get; set; }
        Object SourceObject { get; set; }
        PropertyInfo Property { get; set; }
        string Label { get; set; }

        const int LineHeight = 24;

        #region AddField
        public Action<BuiltForm> AddField<T>(AddResult<T> field) where T : Control, new()
        {
            return ProcessResult(field);
        }

        public Action<BuiltForm> ProcessResult<T>(AddResult<T> result) where T : Control, new()
        {
            var field = Builder.AddField<T>(Label, result.SetProperties, Ordinal);
            return new Action<BuiltForm>((form) => result.StoreValues(field, form));
        }
        public struct AddResult<T> where T : Control, new()
        {
            public Action<T> SetProperties { get; set; }
            public Action<IFormField<T>, BuiltForm> StoreValues { get; set; }
        }
        #endregion

        #region YesNoBox
        public Action<BuiltForm> AddYesNoBox()
        {
            return AddField(GetYesNoBox());
        }
        public AddResult<YesNoButton> GetYesNoBox()
        {
            return new AddResult<YesNoButton>()
            {
                SetProperties = (b) => b.SelectedValue = (bool)Property.GetValue(SourceObject, null),
                StoreValues = (field, form) => Property.SetValue(SourceObject, form.GetInput(field).SelectedValue, null)
            };
        }
        #endregion

        #region TextBox
        public Action<BuiltForm> AddReadOnlyTextBox(int? max, int? lines)
        {
            return AddField<TextBox>(GetTextBox<TextBox>(max, lines, true));
        }

        public Action<BuiltForm> AddTextbox(bool required, int? max, int? lines)
        {
            if (required)
                return AddField<RequiredTextBox>(GetTextBox<RequiredTextBox>(max, lines));
            else
                return AddField<TextBox>(GetTextBox<TextBox>(max, lines));

        }
        public AddResult<T> GetTextBox<T>(int? max, int? lines, bool isReadOnly) where T : TextBox, new()
        {
            //Limitations of C# generics seem to imply that making this one action is currently impossible.
            Action<T> normalAction = (b) =>
            {
                b.Text = (string)Property.GetValue(SourceObject, null);
                if (lines.HasValue)
                {
                    b.Multiline = true;
                    b.Height = LineHeight * lines.Value;
                    b.Enabled = !isReadOnly;
                }
                if (max.HasValue)
                    b.MaxLength = max.Value;
            };
            return new AddResult<T>()
            {
                StoreValues = (field, form) => Property.SetValue(SourceObject, form.GetInput<T>(field).Text, null),
                SetProperties = normalAction
            };
        }

        public AddResult<T> GetTextBox<T>(int? max, int? lines) where T : TextBox, new()
        {
            //Limitations of C# generics seem to imply that making this one action is currently impossible.
            //Action<T> normalAction = (b) =>
            //{
            //    b.Text = (string)Property.GetValue(SourceObject, null);
            //    if (lines.HasValue)
            //    {
            //        b.Multiline = true;
            //        b.Height = LineHeight * lines.Value;
            //    }
            //    if (max.HasValue)
            //        b.MaxLength = max.Value;
            //};
            //return new AddResult<T>()
            //{
            //    StoreValues = (field, form) => Property.SetValue(SourceObject, form.GetInput<T>(field).Text, null),
            //    SetProperties = normalAction
            //};
            return GetTextBox<T>(max, lines, false);
        }
        #endregion

        #region NumericUpDown

        public Action<BuiltForm> AddNud(int? min, int? max)
        {
            return AddField(GetNud(min, max));
        }

        public AddResult<NumericUpDown> GetNud(int? min, int? max)
        {
            Action<NumericUpDown> action = (b) =>
            {
                b.Value = (int)Property.GetValue(SourceObject, null);
                if (min.HasValue)
                    b.Minimum = min.Value;
                if (max.HasValue)
                    b.Maximum = max.Value;
            };

            return new AddResult<NumericUpDown>()
            {
                StoreValues = (field, form) => Property.SetValue(SourceObject, (int)form.GetInput(field).Value, null),
                SetProperties = action
            };
        }

        #endregion

        #region NumericTextBox

        public Action<BuiltForm> AddNumericTextBox()
        {
            return AddField(GetNumericTextBox());
        }

        public AddResult<NumericTextBox> GetNumericTextBox()
        {
            Action<NumericTextBox> action = (b) =>
            {
                b.Text = ((int)Property.GetValue(SourceObject, null)).ToString();
                b.AllowedSpecialCharacters = "-";
            };

            return new AddResult<NumericTextBox>()
            {
                StoreValues = (field, form) =>
                {
                    int outer = 0;
                    if (int.TryParse(form.GetInput(field).Text, out outer))
                        Property.SetValue(SourceObject, outer, null);
                    else
                        Property.SetValue(SourceObject, null, null);
                },
                SetProperties = action
            };
        }

        #endregion

        #region DateTimePickers

        public Action<BuiltForm> AddDateTimePicker()
        {
            return AddField(GetDateTimePicker());
        }
        public AddResult<DateTimePicker> GetDateTimePicker()
        {
            return new AddResult<DateTimePicker>()
            {
                StoreValues = (field, form) => Property.SetValue(SourceObject, (DateTime)form.GetInput(field).Value, null),
                SetProperties = GetDateTimePickerActionSet().Normal
            };
        }

        public Action<BuiltForm> AddNullableDateTimePicker()
        {
            return AddField(GetNullableDateTimePicker());
        }
        public AddResult<NullableControl<DateTimePicker>> GetNullableDateTimePicker()
        {
            return new AddResult<NullableControl<DateTimePicker>>()
            {
                StoreValues = (field, form) => 
                {
                    var control = form.GetInput(field);
                    DateTime? value = control.IsChecked ? control.Field.Value : (DateTime?)null;
                    Property.SetValue(SourceObject, value, null);
                },
                SetProperties = GetDateTimePickerActionSet().Nullable
            };
        }

        public struct NullableActionSet<T> where T : Control, new()
        {
            public Action<T> Normal { get; set; }
            public Action<NullableControl<T>> Nullable { get; set; }
        }

        public NullableActionSet<DateTimePicker> GetDateTimePickerActionSet()
        {
            DateTime? value = ((DateTime?)Property.GetValue(SourceObject, null));
            Action<DateTimePicker> normalAction = (b) =>
            {
                b.Value = value.Value;
            };

            Action<NullableControl<DateTimePicker>> nullableAction = (c) =>
            {
                if (value.HasValue)
                {
                    normalAction(c.Field);
                    c.IsChecked = true;
                }
                else
                {
                    c.IsChecked = false;
                    c.Field.Enabled = false;
                }
            };

            return new NullableActionSet<DateTimePicker>()
            {
                Normal = normalAction,
                Nullable = nullableAction
            };
        }

        #endregion
    }
}
