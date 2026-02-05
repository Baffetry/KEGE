using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KEGE_Station.User_Controls
{
    /// <summary>
    /// Interaction logic for ScorePanel.xaml
    /// </summary>
    public partial class ScorePanel : UserControl
    {
        public ScorePanel()
        {
            InitializeComponent();
            this.Loaded += (s, e) => UpdateArc(Value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ScorePanel),
                new PropertyMetadata(0.0, OnValueChanged));

        // Свойство для текста в центре
        public static readonly DependencyProperty ScoreTextProperty =
            DependencyProperty.Register("ScoreText", typeof(string), typeof(ScorePanel),
                new PropertyMetadata("0"));

        public string ScoreText
        {
            get => (string)GetValue(ScoreTextProperty);
            set => SetValue(ScoreTextProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ScorePanel)d;
            control.UpdateArc((double)e.NewValue);
        }

        private void UpdateArc(double value)
        {
            // Ограничиваем значение от 0 до 100
            value = Math.Max(0, Math.Min(100, value));

            // Радиус и центр (согласно размерам в XAML)
            double radius = 90;
            double centerX = 100;
            double centerY = 100;

            double renderValue = value;
            if (value < 0.2) renderValue = 0.2;
            if (value > 99.8) renderValue = 99.8;
            // Вычисляем угол в радианах (180 градусов для полукруга)
            // 0 баллов = 180°, 100 баллов = 0°
            double angleInDegrees = 180 - (renderValue / 100.0 * 180.0);
            double angleInRadians = angleInDegrees * (Math.PI / 180.0);

            // Вычисляем конечную точку
            double x = centerX + radius * Math.Cos(angleInRadians);
            double y = centerY - radius * Math.Sin(angleInRadians);

            // Обновляем дугу в XAML
            if (ProgressArc != null)
            {
                ProgressArc.Point = new Point(x, y);
                // Важно: если дуга должна быть больше половины круга (у вас полукруг, так что всегда False)
                ProgressArc.IsLargeArc = false;
            }

            // Меняем цвет
            UpdateColor(value);
        }

        private void UpdateColor(double value)
        {
            double ratio = Math.Max(0, Math.Min(100, value)) / 100.0;

            // Опорные цвета
            Color red = Color.FromRgb(230, 57, 70);     // #e63946
            Color yellow = Color.FromRgb(255, 202, 58); // #ffca3a
            Color green = Color.FromRgb(63, 192, 141);  // #3fc08d

            Color resultColor;

            if (ratio <= 0)
            {
                resultColor = red;
            }
            else if (ratio < 0.5)
            {
                // Первый этап: Красный -> Желтый (0% - 50%)
                // Масштабируем ratio из [0...0.5] в [0...1]
                double localRatio = ratio * 2;
                resultColor = Interpolate(red, yellow, localRatio);
            }
            else
            {
                // Второй этап: Желтый -> Зеленый (50% - 100%)
                // Масштабируем ratio из [0.5...1] в [0...1]
                double localRatio = (ratio - 0.5) * 2;
                resultColor = Interpolate(yellow, green, localRatio);
            }

            ProgressPath.Stroke = new SolidColorBrush(resultColor);
        }

        // Вспомогательный метод для смешивания цветов
        private Color Interpolate(Color start, Color end, double ratio)
        {
            byte r = (byte)(start.R + (end.R - start.R) * ratio);
            byte g = (byte)(start.G + (end.G - start.G) * ratio);
            byte b = (byte)(start.B + (end.B - start.B) * ratio);
            return Color.FromRgb(r, g, b);
        }
    }
}
