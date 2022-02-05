using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Reflection;

namespace MdSession
{
    public static class ColorCodes
    {
        public static List<ColorCode> Codes { get; private set; }
        private static void AddCode(string name, Constants reflectionValue, Color? color = null)
        {
            if (color == null) color = Color.FromName(name.Replace(" ", ""));
            Codes.Add(new ColorCode(reflectionValue, name, color.Value));
        }
        static ColorCodes()
        {
            Codes = new List<ColorCode>();
            AddCode("White", Constants.rcWhite);
            AddCode("Black", Constants.rcBlack);
            AddCode("Light Gray", Constants.rcGrey);
            AddCode("Gray", Constants.rcDkGrey);
            AddCode("Red", Constants.rcRed);
            AddCode("Dark Red", Constants.rcDkRed);
            AddCode("Yellow", Constants.rcYellow);
            AddCode("Olive", Constants.rcDkYellow);
            AddCode("Light Green", Constants.rcGreen, Color.FromArgb(255, 0, 255, 0));
            AddCode("Green", Constants.rcDkGreen);
            AddCode("Cyan", Constants.rcCyan);
            AddCode("Turquoise", Constants.rcDkCyan, Color.FromArgb(255, 0, 128, 128));
            AddCode("Blue", Constants.rcBlue);
            AddCode("Dark Blue", Constants.rcDkBlue);
            AddCode("Magenta", Constants.rcMagenta);
            AddCode("Purple", Constants.rcDkMagenta);
        }
        public static ColorCode GetByReflectionValue(Reflection.Constants reflectionValue)
        {
            return Codes.Single(o => o.ReflectionValue == reflectionValue);
        }
    }
}
