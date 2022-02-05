using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace CMPLNTRACK
{
    public partial class ListTableEditor : Form
    {
        string singularName;
        string pluralName;
        string displayMember;
        Func<object> refreshItems;
        Action<string> newItem;
        Action<int> deleteItem;
        Action<int, ContextMenu> processContextMenu;
        public ListTableEditor(string singularName, string pluralName, string displayMember, string valueMember, Func<object> refreshItems, Action<string> newItem, Action<int> deleteItem, Action<int, ContextMenu> processContextMenu = null)
        {
            InitializeComponent();

            this.singularName = singularName;
            this.pluralName = pluralName;
            this.refreshItems = refreshItems;
            this.newItem = newItem;
            this.deleteItem = deleteItem;
            this.displayMember = displayMember;
            this.processContextMenu = processContextMenu;
            if (this.processContextMenu != null)
                RetireButton.Text = "Modify";

            this.MainList.DisplayMember = displayMember;
            this.MainList.ValueMember = valueMember;

            this.Text = Format(this.Text);
            NewButton.Text = Format(NewButton.Text);

            RefreshList();
        }

        private string Format(string format)
        {
            return string.Format(format, singularName, pluralName);
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            var box = new Uheaa.Common.WinForms.InputBox<TextBox>(Format("Enter a {0} name"));
            if (box.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (MainList.Items.Cast<object>().Any(o => o.GetType().GetProperty(displayMember).GetValue(o).ToString().ToLower().Trim() == box.InputControl.Text.ToLower().Trim()))
                {
                    Dialog.Warning.Ok(string.Format("There is already a {0} with that value.", singularName));
                    return;
                }
                newItem(box.InputControl.Text);
                RefreshList();
            }
        }

        private void RefreshList()
        {
            this.MainList.DataSource = refreshItems();
        }

        private void MainList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetireButton.Enabled = MainList.SelectedIndex >= 0;
        }

        private void RetireButton_Click(object sender, EventArgs e)
        {
            int id = (int)MainList.SelectedValue;
            Action retire = new Action(() =>
            {
                if (Dialog.Def.YesNo(string.Format("Really retire {0} '{1}'?", singularName, MainList.Text)))
                {
                    deleteItem(id);
                    RefreshList();
                }
            });
            if (processContextMenu == null)
                retire();
            else
            {
                ContextMenu menu = new ContextMenu();
                var retireMenu = new MenuItem() { Text = "Retire " + singularName };
                retireMenu.Click += (o, ea) => retire();
                menu.MenuItems.Add(retireMenu);
                processContextMenu(id, menu);

                menu.Show(RetireButton, RetireButton.PointToClient(Cursor.Position));
            }
        }
    }
}