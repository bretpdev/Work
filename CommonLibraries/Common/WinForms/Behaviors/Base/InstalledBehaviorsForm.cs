using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public partial class InstalledBehaviorsForm : Form
    {
        public InstalledBehaviorsForm(Control control)
        {
            InitializeComponent();
            if (!(control is IHasBehaviors))
                throw new ArgumentException("The installed behaviors editor can only be used on control that implement the IHasBehaviors interface");
            LoadBehaviors(control);
        }

        List<Behavior> behaviors;
        private void LoadBehaviors(Control control)
        {
            behaviors = new List<Behavior>();
            Type controlType = control.GetType();
            //loop through all available classes in Uheaa.Common.Winforms
            Assembly winforms = Assembly.GetExecutingAssembly();
            foreach (Type type in winforms.GetTypes())
            {
                if (type.BaseType == typeof(Behavior)) //we've got a behavior
                {
                    Type genericType;
                    try
                    {
                        genericType = type.MakeGenericType(controlType);
                    }
                    catch (ArgumentException)
                    {
                        continue; //doesn't fulfill the requirements of the behavior
                    }
                    var behavior = (Behavior)Activator.CreateInstance(genericType, control);
                    behaviors.Add(behavior);
                    BehaviorsList.Items.Add(behavior.FriendlyName, (control as IHasBehaviors).InstalledBehaviors.Contains(behavior));
                }
            }
        }

        public BehaviorInstallation SelectedBehaviors
        {
            get
            {
                List<Behavior> selected = new List<Behavior>();
                foreach (int index in BehaviorsList.CheckedIndices)
                    selected.Add(behaviors[index]);
                var installation = new BehaviorInstallation(selected);
                installation.Install();
                return installation;
            }
        }
    }
}
