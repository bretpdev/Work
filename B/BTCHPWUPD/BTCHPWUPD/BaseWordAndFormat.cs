using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace BTCHPWUPD
{
    public partial class BaseWordAndFormat : Form
    {
        public DataAccess DA { get; set; }
        public BaseWordAndFormat(DataAccess da)
        {
            InitializeComponent();
            DA = da;
        }

        private void BaseWord_Leave(object sender, EventArgs e)
        {
            if (BaseWord.Text.IsNullOrEmpty())
            {
                ValidateBaseWord(string.Empty);
                return;
            }
            else if (BaseWord.Text.Length != 4)
            {
                ValidateBaseWord("The base word must be 4 characters.");
                return;
            }
            else if (!BaseWord.Text.Any(p => char.IsUpper(p)))
            {
                ValidateBaseWord("The base word must have at least 1 capitial letter.");
                return;
            }
            else if (!BaseWord.Text.Any(p => char.IsLower(p)))
            {
                ValidateBaseWord("The base word must have at least 1 capitial letter.");
                return;
            }
            else
            {
                BaseWord.BackColor = SystemColors.Window;
                B1.Text = BaseWord.Text[0].ToString();
                B2.Text = BaseWord.Text[1].ToString();
                B3.Text = BaseWord.Text[2].ToString();
                B4.Text = BaseWord.Text[3].ToString();
                label3.Visible = M1.Visible = M2.Visible = Y1.Visible = Y2.Visible = B1.Visible = B2.Visible = B3.Visible = B4.Visible = true;
                BaseWord.Enabled = false;
            }


        }

        private void ValidateBaseWord(string message)
        {
            BaseWord.BackColor = Color.LightPink;
            if (message.IsPopulated())
                Dialog.Error.Ok(message);
            BaseWord.Focus();
        }

        Point ptOriginal = Point.Empty; //the original location of the button
        private void M1_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void Mouse_Down_L(object sender, MouseEventArgs e)
        {
            ptOriginal = new Point(e.X, e.Y);
            P1.AllowDrop = true;
            ((Control)sender).DoDragDrop(sender, DragDropEffects.Move);
            if (this.Controls.OfType<Label>().Where(p => p.Name.Length == 2).Select(p => p.Location.Y).Distinct().Count() == 1)
                OK.Enabled = true;

            startingLocationX = 0;
        }

        private void M2_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
            startingLocationX = 0;
        }

        private void Y1_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void Y2_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void B1_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void B2_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void B3_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void B4_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse_Down_L(sender, e);
        }

        private void P1_1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.Label)))
                e.Effect = DragDropEffects.Move;

        }
        private int startingLocationY = 0;
        private int startingLocationXFixed = 22;
        private int startingLocationX = 0;
        private void P1_1_DragOver(object sender, DragEventArgs e)
        {
            ((Control)e.Data.GetData(typeof(System.Windows.Forms.Label))).Location =
           this.PointToClient(new Point(e.X - ptOriginal.X, e.Y - ptOriginal.Y));
            var c = ((Control)e.Data.GetData(typeof(System.Windows.Forms.Label)));
            c.BringToFront();
            if (startingLocationY == 0)
                startingLocationY = c.Location.Y + 14;

            if (startingLocationX == 0)
            {
                startingLocationXFixed = startingLocationXFixed + 30;
                startingLocationX = startingLocationXFixed;
            }

            c.Location = new Point(startingLocationX, startingLocationY);
        }

        private void P1_1_DragDrop(object sender, DragEventArgs e)
        {
            ((Label)e.Data.GetData(typeof(System.Windows.Forms.Label))).BringToFront();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            string format = string.Join("", this.Controls.OfType<Label>().Where(p => p.Name.Length == 2).OrderBy(p => p.Location.X).Select(q => q.Name));
            DA.UpdateBaseWordAndFormat(BaseWord.Text, format);
            Dialog.Info.Ok("The base word and format have been updated.");
            DialogResult = DialogResult.OK;
        }

        private void BaseWordAndFormat_Load(object sender, EventArgs e)
        {
            BaseWord.Focus();
        }

        private void BaseWord_TextChanged(object sender, EventArgs e)
        {
            if (BaseWord.Text.Length == 4)
                BaseWord_Leave(sender, e);
        }
    }
}
