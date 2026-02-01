using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;

namespace KEGE_Station.User_Controls
{
    /// <summary>
    /// Interaction logic for AnswerStatisticPanel.xaml
    /// </summary>
    public partial class AnswerStatisticPanel : UserControl
    {
        public AnswerStatisticPanel(string taskNumber, string participantText, string correctText, Brush color)
        {
            InitializeComponent();

            _TaskNumber.Text = taskNumber;
            _CorrectAnswer.Text = FormatToTable(correctText);
            _ParticipantAnswer.Text = FormatToTable(participantText);
            _ParticipantAnswer.Foreground = color;
        }

        private string FormatToTable(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            // 1. Заменяем метку %noAnswer% на пустую строку, чтобы сохранить структуру массива
            // Используем RegexOptions.IgnoreCase, чтобы не зависеть от регистра
            string cleanedInput = Regex.Replace(input, @"%noAnswer%", "", RegexOptions.IgnoreCase);

            // Разделяем строку, но теперь НЕ используем RemoveEmptyEntries, 
            // чтобы пустые ответы (где был %noAnswer%) остались в массиве как ""
            var parts = cleanedInput.Split(' ');
            var formatted = new StringBuilder();

            // 2. Находим максимальную длину числа для первой колонки
            int maxFirstColumnLength = 0;
            for (int i = 0; i < parts.Length; i += 2)
            {
                if (parts[i].Length > maxFirstColumnLength)
                    maxFirstColumnLength = parts[i].Length;
            }

            int padding = maxFirstColumnLength + 3;

            // 3. Формируем строки
            for (int i = 0; i < parts.Length; i += 2)
            {
                // Выравниваем первое число. Если там был %noAnswer%, будет просто Padding из пробелов
                string firstColumn = parts[i].PadRight(padding);
                formatted.Append(firstColumn);

                // Добавляем второе число (или пустоту, если там был %noAnswer%)
                if (i + 1 < parts.Length) formatted.Append(parts[i + 1]);
                if (i + 2 < parts.Length) formatted.Append(Environment.NewLine);
            }

            return formatted.ToString().TrimEnd(); // Убираем лишние переносы в конце
        }
    }
}
