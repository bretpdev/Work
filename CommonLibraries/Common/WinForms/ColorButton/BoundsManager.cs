using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class BoundsManager
    {
        public Size Size { get; internal set; }
        public int Radius { get; internal set; }
        public int Padding { get; internal set; }

        public int LeftBound { get { return Padding; } }
        public int LeftBoundRadius { get { return LeftBound + Radius; } }

        public int TopBound { get { return Padding; } }
        public int TopBoundRadius { get { return TopBound + Radius; } }

        public int RightBound { get { return Size.Width - Padding; } }
        public int RightBoundRadius { get { return RightBound - Radius; } }

        public int BottomBound { get { return Size.Height - Padding; } }
        public int BottomBoundRadius { get { return BottomBound - Radius; } }

        public List<RoundedPoint> PointList { get; internal set; }
        public GraphicsPath BorderPath { get; internal set; }
        public GraphicsPath TopHalfPath { get; internal set; }
        public GraphicsPath BottomHalfPath { get; internal set; }
        private void GeneratePointList()
        {
            List<RoundedPoint> points = new List<RoundedPoint>();
            Action<int, int> add = new Action<int, int>(
                (x, y) => points.Add(new RoundedPoint(new Point(x, y), RoundedPointType.Normal)));
            Action<int, int> addFocal = new Action<int,int>(
                (x, y) => points.Add(new RoundedPoint(new Point(x, y), RoundedPointType.Focal)));
            //top line
            add(LeftBoundRadius, TopBound);
            add(RightBoundRadius, TopBound);
            //top right arc
            addFocal(RightBoundRadius, TopBoundRadius);

            //right line
            add(RightBound, TopBoundRadius);
            add(RightBound, Size.Height / 2);
            int rightHalfwayPoint = points.Count - 1;
            add(RightBound, BottomBoundRadius);
            //bottom right arc
            addFocal(RightBoundRadius, BottomBoundRadius);

            //bottom line
            add(RightBoundRadius, BottomBound);
            add(LeftBoundRadius, BottomBound);
            //bottom left arc
            addFocal(LeftBoundRadius, BottomBoundRadius);

            //left line
            add(LeftBound, BottomBoundRadius);
            add(LeftBound, Size.Height / 2);
            int leftHalfwayPoint = points.Count - 1;
            add(LeftBound, TopBoundRadius);
            //top left arc
            addFocal(LeftBoundRadius, TopBoundRadius);

            PointList = points;
            BorderPath = RoundedRectangleGenerator.Generate(points);
            TopHalfPath = RoundedRectangleGenerator.Generate(points, leftHalfwayPoint, rightHalfwayPoint + 1);
            BottomHalfPath = RoundedRectangleGenerator.Generate(points, rightHalfwayPoint, leftHalfwayPoint + 1);
        }

        public BoundsManager(Size size, int radius, int padding)
        {
            this.Size = size;//new Size(size.Width - 1, size.Height - 1); //fix a 1-pixel-off issue with the render size
            this.Radius = radius;
            this.Padding = padding;
            GeneratePointList();
        }
    }

}
