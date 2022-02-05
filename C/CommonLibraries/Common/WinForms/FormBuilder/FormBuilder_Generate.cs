using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace Uheaa.Common.WinForms
{
    public partial class FormBuilder
    {
        public static BuiltForm Generate<T>(T generateFrom, string windowTitle)
        {
            return Generate(generateFrom, new FormBuilder(windowTitle));
        }

        /// <summary>
        /// Generates a built form that will edit the given object.  Supports the following options:
        /// RequiredAttribute - Makes Properties Required
        /// Description - Fields will use the given description as the label.
        /// DataType.MultilineText - Will create a multiline textbox.
        /// Browsable(false) - No controls will be generated for this field.
        /// </summary>
        /// <param name="generateFrom">The object that will be edited by the form.</param>
        /// <param name="initialOptions">A FormBuilder with initial options set.</param>
        /// <returns>A BuiltForm capable of processing the given object.</returns>
        public static BuiltForm Generate<T>(T generateFrom, FormBuilder initialOptions)
        {
            FormBuilder fb = initialOptions;
            List<Action<BuiltForm>> setFields = new List<Action<BuiltForm>>();
            foreach (PropertyInfo pi in generateFrom.GetType().GetProperties())
            {
                if (pi.HasAttribute<HiddenAttribute>())
                    continue; //don't process hidden attributes
                if (pi.HasAttribute<ManualAttribute>())
                    continue; //don't process manual attributes;
                if (pi.CanRead && pi.CanWrite && !pi.GetGetMethod().IsStatic)
                {
                    string label = pi.GetAttributeValue<LabelAttribute, string>(o => o.Label, null);
                    if (label == null) label = pi.Name.PascalToWords();
                    int? ordinal = pi.GetAttributeValue<OrdinalAttribute, int?>(o => o.Ordinal, null);
                    PropertyProcessor pp = new PropertyProcessor(fb, generateFrom, pi, label, ordinal);
                    bool required = pi.HasAttribute<RequiredAttribute>();
                    int? max = pi.GetAttributeValue<MaxAttribute, int?>(o => o.Max, null);
                    int? min = pi.GetAttributeValue<MinAttribute, int?>(o => o.Min, null);
                    int? maxLength = pi.GetAttributeValue<MaxLengthAttribute, int?>(o => o.MaxLength, null);
                    int? lines = pi.GetAttributeValue<TextBoxLinesAttribute, int?>(o => o.LineCount, null);
                    bool nud = pi.HasAttribute<IncludeUpDownArrowsAttribute>();
                    if (max.HasValue || min.HasValue) nud = true;


                    if (pi.PropertyType == typeof(bool))
                        setFields.Add(pp.AddField(pp.GetYesNoBox()));
                    else if (pi.PropertyType == typeof(string))
                    {
                        setFields.Add(pp.AddTextbox(required, maxLength, lines));
                    }
                    else if (pi.PropertyType == typeof(int))
                    {
                        if (nud)
                            setFields.Add(pp.AddNud(min, max));
                        else
                            setFields.Add(pp.AddNumericTextBox());
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        setFields.Add(pp.AddDateTimePicker());
                    }
                    else if (pi.PropertyType == typeof(DateTime?))
                    {
                        setFields.Add(pp.AddNullableDateTimePicker());
                    }
                }
            }
            fb.FormAccepted += (form) =>
            {
                foreach (var action in setFields)
                    action(form);
                return true;
            };
            return fb.Build();
        }
    }
}
