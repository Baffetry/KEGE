using KEGE_Station.User_Controls;
using Result_Analyzer;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KEGE_Station.Windows
{
    public partial class StatisticWindow : Window
    {
        // Конструктор
        public StatisticWindow(List<AnswerStatistic> statistics)
        {
            InitializeComponent();

            this.Loaded += (s, e) => { this.Left -= 200; };

            // Если пришел пустой список, не пытаемся ничего рисовать
            if (statistics != null && statistics.Count > 0)
                DisplayStats(statistics);


            SetProps();
        }

        private void SetProps()
        {
            ButtonBehavior.Apply(_GetVariant_btn);
            ButtonBehavior.Apply(_Close_btn, true);
        }

        private void DisplayStats(List<AnswerStatistic> statistics)
        {
            // ОЧЕНЬ ВАЖНО: Очищаем сетку перед заполнением
            // Это гарантирует, что при пересоздании окна (если объект выжил) не будет наслоения
            _StatisticGrid.Children.Clear();
            _StatisticGrid.RowDefinitions.Clear();

            for (int i = 0; i < statistics.Count; i++)
            {
                var stat = statistics[i];
                var colorBrush = HexToBrush(stat.Color);

                // Создаем панель
                var answerPanel = new AnswerStatisticPanel(
                    stat.TaskNumber,
                    stat.ParticipantAnswer,
                    stat.CorrectAnswer,
                    colorBrush
                );

                // Добавляем новую строку
                _StatisticGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Устанавливаем позицию
                Grid.SetRow(answerPanel, i);

                // Добавляем в Grid
                _StatisticGrid.Children.Add(answerPanel);
            }
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно. Экземпляр будет уничтожен.
            this.Close();
        }

        private Brush HexToBrush(StatisticColor color)
        {
            string hexColor = color switch
            {
                StatisticColor.Green => "#3fc08d",
                StatisticColor.Red => "#e63946",
                StatisticColor.Yellow => "#f9ca24",
                _ => "#000000"
            };

            // Кэшируем BrushConverter или используем упрощенный вариант
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hexColor);
        }

        private void _Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void _GetVariant_btn_Click(object sender, RoutedEventArgs e)
        {
            string optionsPath = App.GetResourceString("SaveOptionsPath");
            string optionId = _TitleID.Text;

            var files = Directory.GetFiles(optionsPath, $"{optionId}*.json");

            string targetJsonPath = files.FirstOrDefault();

            if (targetJsonPath != null)
                Process.Start("explorer.exe", $"/select, \"{targetJsonPath}\"");
        }
    }
}