using System.Drawing;
using System.Linq;

namespace Uheaa.Common
{
    public static class RtfHelper
    {
        public static string Color(Color color)
        {
            return Color(color.R, color.G, color.B);
        }

        public static string Color(byte r, byte g, byte b)
        {
            return @"\red" + r + @"\green" + g + @"\blue" + b + ";";
        }

        public static string ColorTable(params Color[] colors)
        {
            return @"{\colortbl;" + string.Join(";", colors.Select(c => Color(c)).ToArray()) + "}";
        }
    }
}
