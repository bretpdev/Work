using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    [Flags]
    public enum FormAnchor
    {
        None = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomRight = 4,
        BottomLeft = 8,
        Top = 16,
        Left = 32,
        Bottom = 64,
        Right = 128,
        Corners = TopLeft | TopRight | BottomRight | BottomLeft,
        Sides = Top | Left | Bottom | Right,
        StaticWidth = Bottom | Top,
        StaticHeight = Left | Right
    }
}
