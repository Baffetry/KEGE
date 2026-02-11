using System.Windows.Media;

namespace KEGE_Station.Models.Custom_brushes
{
    public class CustomBrusher
    {
        public static Brush White = GetBrush("#FFFFFF");
        public static Brush Green = GetBrush("#3FC08D");
        public static Brush Yellow = GetBrush("#FFDD75");
        public static Brush Blue = GetBrush("#66D9FF");
        public static Brush Red = GetBrush("#E85460");
        public static Brush LightBlue = GetBrush("#2D6DFE");
        public static Brush LightGray = GetBrush("#E0E0E0");
        public static Brush DarkGray = GetBrush("#A0A0A0");
        public static Brush Black = GetBrush("#000000");

        private static Brush GetBrush(string hex)
        {
            var brush = (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
            if (brush != null)
                brush.Freeze();
            return brush;
        }
    }
}
