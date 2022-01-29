using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public struct RoundedPoint
    {
        public Point Point { get; set; }
        public RoundedPointType Type { get; set; }
        public RoundedPoint(Point point, RoundedPointType type)
            : this()
        {
            this.Point = point;
            this.Type = type;
        }
    }

    public enum RoundedPointType
    {
        Normal,
        Focal
    }
}
