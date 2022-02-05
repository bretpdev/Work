using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MD
{
    public partial class ImageAnnotator : BaseForm
    {
        Image overlay;
        Image initialImage;
        float aspectRatio = 0;
        public ImageAnnotator(Image image, Image overlay = null, bool readOnly = false)
        {
            InitializeComponent();

            InitMenu();

            this.initialImage = image;
            EditPicture.BackgroundImageLayout = ImageLayout.Stretch;
            EditPicture.BackgroundImage = image;
            if (overlay == null)
            {
                if (!readOnly)
                    ResetOverlay();
            }
            else
                LoadOverlay(new Bitmap(overlay));

            ChangeColor(Color.Black);
            CalibrateAspect();

            if (readOnly) ReadOnly();
        }

        private void ReadOnly()
        {
            EditPicture.MouseDown -= EditPicture_MouseDown;
            EditPicture.MouseUp -= EditPicture_MouseUp;
            EditPicture.MouseMove -= EditPicture_MouseMove;
            ToolsMenu.Items.Clear();
            if (overlay != null)
            {
                var toggle = ToolsMenu.Items.Add("Toggle Overlay");
                toggle.Click += (o, ea) =>
                {
                    if (EditPicture.Image == null)
                    {
                        LoadOverlay(overlay);
                        toggle.Text = "Hide Annotations";
                    }
                    else
                    {
                        EditPicture.Image = null;
                        toggle.Text = "Show Annotations";
                    }
                };
                toggle.PerformClick(); toggle.PerformClick();
            }
            var save = ToolsMenu.Items.Add("Save Image");
            save.Alignment = ToolStripItemAlignment.Right;
            save.Click += (o, ea) =>
            {
                using (SaveFileDialog diag = new SaveFileDialog())
                {
                    diag.Filter = "PNG Files (*.png)|*.png";
                    if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Image combined = new Bitmap(initialImage);
                        if (EditPicture.Image != null)
                            using (Graphics g = Graphics.FromImage(combined))
                                g.DrawImage(overlay, Point.Empty);
                        combined.Save(diag.FileName);
                    }
                }
            };
        }

        private void CalibrateAspect()
        {
            this.Size = initialImage.Size;
            Func<float> calcDiff = () => Math.Abs(initialImage.Width / (float)initialImage.Height - EditPicture.Width / (float)EditPicture.Height);
            float aspectDifference = calcDiff();
            bool decreaseWidth = true;
            while (decreaseWidth)
            {
                this.Width--;
                decreaseWidth = calcDiff() < aspectDifference;
                aspectDifference = calcDiff();
            }
            bool increaseWidth = true;
            while (increaseWidth)
            {
                this.Width++;
                increaseWidth = calcDiff() < aspectDifference;
                aspectDifference = calcDiff();
            }
            aspectRatio = this.Width / (float)this.Height;
            lastSize = this.Size;
        }

        private void ResetOverlay()
        {
            overlay = new Bitmap(initialImage.Width, initialImage.Height);
        }

        private void LoadOverlay(Image newOverlay)
        {
            overlay = newOverlay;
            EditPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            EditPicture.Image = overlay;
        }

        private int lineSize = 3;
        private void InitMenu()
        {
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.BackColor = Color.Transparent;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));


            Label label = new Label();
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Dock = DockStyle.Fill;
            label.Text = "Line Size: ";
            panel.Controls.Add(label, 0, 0);

            NumericUpDown size = new NumericUpDown();

            size.GotFocus += (o, ea) => ToolsMenu.Focus();
            size.Minimum = 1;
            size.Maximum = 8;
            size.Value = lineSize;
            size.ValueChanged += (o, ea) =>
            {
                lineSize = (int)size.Value;
            };
            panel.Controls.Add(size, 1, 0);

            ToolsMenu.Items.Insert(1, new ToolStripControlHost(panel));
        }


        private void ClearAnnotationsMenu_Click(object sender, EventArgs e)
        {
            ResetOverlay();
            EditPicture.Image = overlay;
        }

        private void ChangeColorMenu_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
                if (cd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    ChangeColor(cd.Color);
        }

        private Color selectedColor;
        private void ChangeColor(Color c)
        {
            selectedColor = c;
            ChangeColorMenu.BackColor = c;
            if (c.GetBrightness() > 0.5f)
                ChangeColorMenu.ForeColor = Color.Black;
            else
                ChangeColorMenu.ForeColor = Color.White;
        }

        bool mouseIsDown = false;
        private void EditPicture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                lastLocation = e.Location;
                gp = new GraphicsPath();
                mouseIsDown = true;
            }
        }
        private void EditPicture_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                gp = null;
                mouseIsDown = false;
            }
        }

        Point? lastLocation;
        GraphicsPath gp;
        private void EditPicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                if (lastLocation.HasValue)
                {
                    using (Graphics g = Graphics.FromImage(overlay))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gp.AddLine(FormToOverlay(lastLocation.Value), FormToOverlay(e.Location));
                        g.DrawPath(new Pen(selectedColor, lineSize), gp);
                    }
                    LoadOverlay(overlay);
                }
                lastLocation = e.Location;
            }
            else
                lastLocation = null;
        }

        private Point FormToOverlay(Point formPoint)
        {
            float xPercent = formPoint.X / (float)EditPicture.Width;
            float yPercent = formPoint.Y / (float)EditPicture.Height;

            float x = overlay.Width * xPercent;
            float y = overlay.Height * yPercent;

            return new Point((int)x, (int)y);
        }

        private void SaveChangesMenu_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public Image Overlay { get { return overlay; } }

        private void CancelMenu_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void ChangeColorMenu_MouseEnter(object sender, EventArgs e)
        {
            ChangeColorMenu.ForeColor = Color.Black;
        }

        private void ChangeColorMenu_MouseLeave(object sender, EventArgs e)
        {
            ChangeColor(selectedColor);
        }

        Size lastSize;
        private void SyncSize()
        {
            Size change = this.Size - lastSize;
            float width = 0;
            float height = 0;
            if (change.Width != 0)
            {
                width = this.Width;
                height = (1 / aspectRatio) * width;
            }
            else if (change.Height != 0)
            {
                height = this.Height;
                width = aspectRatio * height;
            }
            else
                return;
            this.Size = new System.Drawing.Size((int)width, (int)height);
            lastSize = this.Size;
        }
        private void ImageAnnotator_ResizeEnd(object sender, EventArgs e)
        {
            SyncSize();
        }
        bool calling = false;
        protected override void ContrastApplied()
        {
            if (!calling)
            {
                calling = true;
                base.ProfessionalContrast();
                calling = false;
            }
        }
    }
}
