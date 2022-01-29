using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public static class RoundedRectangleGenerator
    {
        public static GraphicsPath Generate(IEnumerable<RoundedPoint> points)
        {
            return Generate(points, 0, points.Count());
        }
        public static GraphicsPath Generate(IEnumerable<RoundedPoint> pointList, int start, int stop)
        {
            List<RoundedPoint> points = RefineList(pointList, start, stop);
            GraphicsPath gp = new GraphicsPath();
            gp.StartFigure();
            for (int i = 0; i < points.Count; i++)
            {
                RoundedPoint current = Get(points, i);
                RoundedPoint next = Get(points, i + 1);
                if (next.Type == RoundedPointType.Normal)
                {
                    AddLine(gp, current.Point, next.Point);
                }
                if (next.Type == RoundedPointType.Focal)
                {
                    RoundedPoint focal = next;
                    next = Get(points, i + 2);
                    AddArc(gp, current.Point, next.Point, focal.Point);
                    i++; //used up one more point than normal
                }
            }
            gp.CloseFigure();
            return gp;
        }

        private static List<RoundedPoint> RefineList(IEnumerable<RoundedPoint> pointList, int start, int stop)
        {
            List<RoundedPoint> points = new List<RoundedPoint>();
            if (start > stop)
            {
                points.AddRange(pointList.Skip(start));
                points.AddRange(pointList.Take(stop));
            }
            else
                points.AddRange(pointList.Skip(start).Take(stop - start));
            return points;

        }
        private static RoundedPoint Get(List<RoundedPoint> points, int index)
        {
            return points[NormalizeIndex(points, index)];
        }
        private static int NormalizeIndex(List<RoundedPoint> points, int index)
        {
            if (points.Count == 0)
                throw new IndexOutOfRangeException();
            while (index < 0) index += points.Count;
            while (index >= points.Count) index -= points.Count;
            return index;
        }

        private static void AddLine(GraphicsPath gp, Point from, Point to)
        {
            gp.AddLine(from, to);
        }

        private static void AddArc(GraphicsPath gp, Point from, Point to, Point focus)
        {
            Rectangle rect = RectangleFromPoints(from, to);
            float angle = AngleFromPoints(focus, from);
            gp.AddArc(rect, angle, 90);
        }

        private static Rectangle RectangleFromPoints(Point one, Point two)
        {
            Point topLeft = new Point(Math.Min(one.X, two.X), Math.Min(one.Y, two.Y));
            Point bottomRight = new Point(Math.Max(one.X, two.X), Math.Max(one.Y, two.Y));
            Size size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            return new Rectangle(topLeft, size);
        }

        private static float AngleFromPoints(Point focus, Point point)
        {
            int deltaY = point.Y - focus.Y;
            int deltaX = point.X - focus.X;

            double angle = Math.Atan2(deltaY, deltaX) * 180 / Math.PI;
            return (float)angle;
        }
    }
}
