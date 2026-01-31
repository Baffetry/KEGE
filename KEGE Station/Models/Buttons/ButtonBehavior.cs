using System.Windows.Controls;
using System.Windows.Media;

namespace KEGE_Station
{
    class ButtonBehavior
    {
        private static SolidColorBrush _notDangerous = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3fc08d"));
        private static SolidColorBrush _dangerous = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e63946"));
        private static SolidColorBrush _default = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));

        public static void Apply(Button button, bool isDangerous = false)
        {
            SolidColorBrush behavior = !isDangerous ? _notDangerous : _dangerous;

            button.MouseEnter += (s, e) =>
            {
                if (button.Template?.FindName("InnerBorder", button) is Border border)
                    border.Background = behavior;
            };


            button.MouseLeave += (s, e) =>
            {
                if (button.Template?.FindName("InnerBorder", button) is Border border)
                    border.Background = _default;
            };
        }
    }
}
