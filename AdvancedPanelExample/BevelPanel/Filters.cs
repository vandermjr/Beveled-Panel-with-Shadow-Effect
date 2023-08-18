using System.Drawing;

namespace BevelPanel
{
    public enum Filter
    {
        DropShadow,
        Blur
    }
    public static class Filters
    {
        public static SolidBrush Blur(int opacity, Color color)
        {
            return new SolidBrush(Color.FromArgb(opacity / 2, color));
        }
        public static void DropShadow(Graphics g, string text, Font font, Color shadow, int opacity, PointF location)
        {
            g.DrawString(text,
                         font,
                         Blur(opacity, Color.FromArgb(opacity, shadow)),
                         location.X + 1,
                         location.Y + 1);
        }

        public static void ApplyFilters(Graphics g, string number, ref PointF location, Filter[] filter)
        {
            foreach (var f in filter)
            {
                switch (f)
                {
                    case Filter.DropShadow:
                        Color bShadow = Color.FromArgb(255, 0, 0, 0);
                        DropShadow(g, number, new Font("Arial", 32, FontStyle.Regular, GraphicsUnit.Pixel), bShadow, 75, location);
                        break;
                }
            }
        }
        public static PointF AbsMiddle(int num1W, int num1H, float num2W, float num2H) => new PointF()
        {
            X = num1W / 2 - num2W / 2,
            Y = num1H / 2 - num2H / 2
        };
        public static Point AbsMiddle(int num1W, int num1H, int num2W, int num2H) => new Point()
        {
            X = num1W / 2 - num2W / 2,
            Y = num1H / 2 - num2H / 2
        };
    }
}