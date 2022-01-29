using MauiDUDE.Properties;
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

namespace MauiDUDE
{
    public partial class Processing : Form
    {
        private static Processing Instance { get; set; }

        public Processing()
        {
            InitializeComponent();

            if(DateTime.Today.Month == 12)
            {
                labelSurfer.Image = Resources.SantaGoinRight;
            }
            else if(DateTime.Today.Month.IsIn(1,2,3,4,5))
            {
                labelSurfer.Image = Resources.ShaggyGoingRight;
            }
            else
            {
                labelSurfer.Image = Resources.DopyGoinRight;
            }
        }

        /// <summary>
        /// Makes the processing form visible
        /// </summary>
        public static void MakeVisible()
        {
//#if !DEBUG
            if(Instance == null)
            {
                Instance = new Processing();
            }
            Instance.Visible = true;
            Instance.Refresh();
//#endif
        }

        /// <summary>
        /// Makes the processing form invisible
        /// </summary>
        public static void MakeInvisible()
        {
//#if !DEBUG
            Instance.Visible = false;
//#endif
        }

        private void Processing_VisibleChanged(object sender, EventArgs e)
        {
            ReDrawBackGround();
        }

        private void ReDrawBackGround()
        {
            System.Drawing.Drawing2D.LinearGradientBrush waterBrush;
            System.Drawing.Drawing2D.LinearGradientBrush skyBrush;
            System.Drawing.Drawing2D.LinearGradientBrush sunBrush;
            Point aPoint;

            var textPen = new SolidBrush(ForeColor);
            var font = new Font("Chiller", 20, FontStyle.Bold);
            var rectText = new RectangleF(4, 156, 496, 40);
            var strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            var pen1 = new Pen(Color.Black);
            var graphics = CreateGraphics();
            //create an empty bitmap from that graphics
            var bmp = new Bitmap(Width, Height, graphics);
            //create a graphics object in memory from that bitmap
            var graphicsMem = Graphics.FromImage(bmp);
            //sky
            var biggerRec = new RectangleF(0, 0, Width, Height - 100);
            //water
            var bigRec = new RectangleF(0, Height - 101, Width, 100);
            RectangleF sunRec;
            //draw sky
            if(DateTime.Now.Hour < 10)
            {
                //morning
                sunRec = new RectangleF(0, Height - 10, Width, Height - 20);
                waterBrush = new System.Drawing.Drawing2D.LinearGradientBrush(bigRec, Color.Cyan, Color.DarkBlue, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                skyBrush = new System.Drawing.Drawing2D.LinearGradientBrush(biggerRec, Color.Salmon, Color.LemonChiffon, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                sunBrush = new System.Drawing.Drawing2D.LinearGradientBrush(sunRec, Color.Yellow, Color.LightGoldenrodYellow, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                graphicsMem.FillRectangle(skyBrush, biggerRec);
                graphicsMem.FillEllipse(sunBrush, (Width / 2) - 50, 50, 100, 100);
                graphicsMem.FillRectangle(waterBrush, bigRec);
            }
            else if(DateTime.Now.Hour < 15)
            {
                //afternoon
                sunRec = new RectangleF(0, Height - 10, Width, Height);
                waterBrush = new System.Drawing.Drawing2D.LinearGradientBrush(bigRec, Color.Blue, Color.DarkTurquoise, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                skyBrush = new System.Drawing.Drawing2D.LinearGradientBrush(biggerRec, Color.Aqua, Color.LightBlue, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                sunBrush = new System.Drawing.Drawing2D.LinearGradientBrush(sunRec, Color.Yellow, Color.Gold, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                graphicsMem.FillRectangle(skyBrush, biggerRec);
                graphicsMem.FillEllipse(sunBrush, (Width / 2) - 50, 0, 100, 100);
                graphicsMem.FillRectangle(waterBrush, bigRec);
            }
            else
            {
                //evening
                sunRec = new RectangleF(0, Height - 10, Width, Height);
                waterBrush = new System.Drawing.Drawing2D.LinearGradientBrush(bigRec, Color.Cyan, Color.Indigo, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                skyBrush = new System.Drawing.Drawing2D.LinearGradientBrush(biggerRec, Color.Purple, Color.LightSkyBlue, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                sunBrush = new System.Drawing.Drawing2D.LinearGradientBrush(sunRec, Color.Wheat, Color.WhiteSmoke, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                graphicsMem.FillRectangle(skyBrush, biggerRec);
                graphicsMem.FillEllipse(sunBrush, (Width / 2) - 50, 100, 100, 100);
                graphicsMem.FillRectangle(waterBrush, bigRec);
            }

            //label at bottom
            if(DateTime.Today.Month == 12)
            {
                aPoint = new Point(184, 50);
            }
            else
            {
                aPoint = new Point(184, 8);
            }
            labelSurfer.Location = aPoint;
            graphicsMem.DrawString("Surfing. . . . . . ", font, textPen, rectText, strFormat);
            BackgroundImage = bmp;
        }
    }
}
