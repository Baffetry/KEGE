using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Text.RegularExpressions;

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

            // Заменяем метку %noAnswer% на пустую строку, чтобы сохранить структуру массива
            string cleanedInput = Regex.Replace(input, @"%no[aA]nswer%", "", RegexOptions.IgnoreCase);

            // Разделяем строку
            var parts = cleanedInput.Split(' ');
            var formatted = new StringBuilder();

            // Находим максимальную длину числа для первой колонки
            int maxFirstColumnLength = 0;
            for (int i = 0; i < parts.Length; i += 2)
            {
                if (parts[i].Length > maxFirstColumnLength)
                    maxFirstColumnLength = parts[i].Length;
            }

            int padding = maxFirstColumnLength + 3;

            // Формируем строки
            for (int i = 0; i < parts.Length; i += 2)
            {
                // Выравниваем первое число. Если там был %noAnswer%, будет просто Padding из пробелов
                string firstColumn = parts[i].PadRight(padding);
                formatted.Append(firstColumn);

                // Добавляем второе число (или пустоту, если там был %noAnswer%)
                if (i + 1 < parts.Length) formatted.Append(parts[i + 1]);
                if (i + 2 < parts.Length) formatted.Append(Environment.NewLine);
            }

            // Убираем лишние переносы в конце
            return formatted.ToString().TrimEnd(); 
        }
    }
}
