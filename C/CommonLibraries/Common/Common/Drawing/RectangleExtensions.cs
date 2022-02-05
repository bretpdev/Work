using System.Drawing;

namespace Uheaa.Common
{
    public static class RectangleExtensions
    {
        public static Rectangle Shrink(this Rectangle r, int amount)
        {
            int doubleAmount = amount * 2;
            return new Rectangle(r.Left + amount, r.Top + amount, r.Width - doubleAmount, r.Height - doubleAmount);
        }
    }
}
