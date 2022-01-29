using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Reflection;

namespace MdSession
{
    public class ColorCode
    {
        public Constants ReflectionValue { get; set; }
        public string FriendlyName { get; set; }
        public Color Color { get; set; }
        public ColorCode(Constants reflectionValue, string friendlyName, Color color)
        {
            ReflectionValue = reflectionValue;
            FriendlyName = friendlyName;
            Color = color;
        }
    }
}
