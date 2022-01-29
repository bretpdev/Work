using System.Linq;
using System.Windows.Forms;

namespace Uheaa.Common
{
    /// <summary>
    /// Facilitates the automatic databinding of Forms,
    /// </summary>
    public static class AutoDataBinder
    {
        /// <summary>
        /// Binds all properties in the given object to Controls on the given Form.
        /// Your Control names must start with the name of a Property to databind properly.
        /// </summary>
        public static void Bind(Form form, object obj)
        {
            foreach (Control control in form.Controls)
            {
                BindControl(control, obj);
            }
        }

        public static bool BindControl<T>(T control, object obj, bool bindIfReadOnly = true) where T : Control
        {
            var property = obj.GetType().GetProperties().SingleOrDefault(pi => control.Name.StartsWith(pi.Name));
            if (property == null) return false;
            string propName = "Text";
            control.DataBindings.Add(propName, obj, property.Name);
            return true;
        }
    }
}
