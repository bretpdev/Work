using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Uheaa.Common.WinForms
{
    public class InstalledBehaviorsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
                return UITypeEditorEditStyle.Modal;
            return base.GetEditStyle(context);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    var form = new InstalledBehaviorsForm((Control)context.Instance);
                    if (service.ShowDialog(form) == DialogResult.OK)
                        return (context.Instance as IHasBehaviors).InstalledBehaviors = form.SelectedBehaviors;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
}
