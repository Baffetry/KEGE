using KEGE_Station.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace KEGE_Station.User_Controls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private readonly string[] taskNumbers = Enumerable.Range(1, 18)
                    .Select(x => x.ToString())                                  // 1-18
                    .Append("19-21")                                            // 19-21
                    .Concat(Enumerable.Range(22, 6).Select(x => x.ToString()))  // 22-27
                    .ToArray();


        public SettingsControl()
        {
            InitializeComponent();
            SetButtonsBehavior();

            TaskNumber_ComboBox.ItemsSource = taskNumbers;
        }

        private void SetButtonsBehavior()
        {
            // Конфигурация
            ButtonBehavior.Apply(BrowseTaskBase_btn);
            ButtonBehavior.Apply(BrowseScoreTable_btn);
            ButtonBehavior.Apply(BrowseOptions_btn);
            ButtonBehavior.Apply(BrowseAnswers_btn);
            ButtonBehavior.Apply(SaveSettings_btn);

            // Добавление задания | Green
            ButtonBehavior.Apply(AddTask_btn);
            ButtonBehavior.Apply(SingelAnswer_btn);
            ButtonBehavior.Apply(Task19_btn);
            ButtonBehavior.Apply(Task20_btn);
            ButtonBehavior.Apply(Task21_btn);
            ButtonBehavior.Apply(PNG_btn);
            ButtonBehavior.Apply(AditionalFiles_btn);

            // Добавление задания | Red
            ButtonBehavior.Apply(Clear_btn, true);
        }

        public void RefreshSettings()
        {
            Setting_TaskPath.Text = App.GetResourceString("TaskBasePath");
            Setting_ScoreTable.Text = App.GetResourceString("ScoreTable");
            Setting_OptionsPath.Text = App.GetResourceString("SaveOptionsPath");
            Setting_AnswersPath.Text = App.GetResourceString("SaveAnswersPath");
        }

        private void ClearAllFields(bool isSelectionChanged = false)
        {
            NewTask_ImagePath.Text = "";
            NewTask_AnswerPath.Text = "";
            Answer19_Path.Text = "";
            Answer20_Path.Text = "";
            Answer21_Path.Text = "";

            if (ExtraFiles_ListBox != null)
                ExtraFiles_ListBox.Items.Clear();

            if (!isSelectionChanged)
                TaskNumber_ComboBox.SelectedIndex = -1;
        }

        private void TaskNumber_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SingleAnswerArea == null || TripleAnswerArea == null) return;

            string selected = TaskNumber_ComboBox.SelectedItem?.ToString();

            if (selected == "19-21")
            {
                SingleAnswerArea.Visibility = Visibility.Collapsed;
                TripleAnswerArea.Visibility = Visibility.Visible;

                Clear_btn.Margin = new Thickness(0, 5, 0, 0);
                AddTask_btn.Margin = new Thickness(10, 5, 0, 0);
            }
            else
            {
                SingleAnswerArea.Visibility = Visibility.Visible;
                TripleAnswerArea.Visibility = Visibility.Collapsed;

                Clear_btn.Margin = new Thickness(0, 20, 0, 0);
                AddTask_btn.Margin = new Thickness(10, 20, 0, 0);
            }

            ClearAllFields(true);
        }

        private void SaveTaskFiles(string basePath, string taskNum, int index, string answerSrc)
        {
            string targetDir = Path.Combine(basePath, taskNum, index.ToString());
            Directory.CreateDirectory(targetDir);

            // 1. Копируем изображение (всегда task.png)
            File.Copy(NewTask_ImagePath.Text, Path.Combine(targetDir, "task.png"), true);

            // 2. Копируем ответ (если указан)
            if (!string.IsNullOrEmpty(answerSrc))
                File.Copy(answerSrc, Path.Combine(targetDir, "answer.txt"), true);

            // 3. Копируем доп. файл (Excel/Word), если есть
            foreach (FileInfo file in ExtraFiles_ListBox.Items)
            {
                // Сохраняем оригинальное имя файла (или можно переименовать в file1, file2...)
                string destFile = Path.Combine(targetDir, file.Name);
                file.CopyTo(destFile, true);
            }
        }

        private void SaveSettings_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string configContent =
                    $"TaskBasePath - {Setting_TaskPath.Text}\n" +
                    $"SaveOptionsPath - {Setting_OptionsPath.Text}\n" +
                    $"SaveAnswersPath - {Setting_AnswersPath.Text}\n" +
                    $"ScoreTable - {Setting_ScoreTable.Text}";

                string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                string filePath = Path.Combine(directoryPath, "config.txt");

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                File.WriteAllText(filePath, configContent);

                App.SetResourceString("TaskBasePath", Setting_TaskPath.Text);
                App.SetResourceString("SaveOptionsPath", Setting_OptionsPath.Text);
                App.SetResourceString("SaveAnswersPath", Setting_AnswersPath.Text);
                App.SetResourceString("ScoreTable", Setting_ScoreTable.Text);

                NotificationWindow.QuickShow(
                    "Настройки конфигурации.",
                    "Настройки успешно сохранены!",
                    NotificationType.Success
                    );
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Настройки конфигурации.",
                    $"Не удалось сохранить файл: {ex.Message}.",
                    NotificationType.Error
                    );
            }
        }

        private void AddTask_btn_Click(object sender, RoutedEventArgs e)
        {
            NotificationWindow notification;

            string selected = TaskNumber_ComboBox.SelectedItem?.ToString();
            string basePath = App.GetResourceString("TaskBasePath");

            if (string.IsNullOrEmpty(selected) || string.IsNullOrEmpty(NewTask_ImagePath.Text))
            {
                NotificationWindow.QuickShow(
                    "Создание задания.",
                    "Не выбран номер задания или не указан путь к изображению.",
                    NotificationType.Warning
                    );
                return;
            }

            try
            {
                int nextIndex;

                if (selected.Equals("19-21"))
                {
                    if (string.IsNullOrEmpty(Answer19_Path.Text) ||
                        string.IsNullOrEmpty(Answer20_Path.Text) ||
                        string.IsNullOrEmpty(Answer21_Path.Text))
                    {
                        NotificationWindow.QuickShow(
                            "Создание задания.",
                            "Для заданий 19-21 должны быть указаны все три файла с ответами.",
                            NotificationType.Warning
                            );
                    }

                    string[] tripleTasks = { "19", "20", "21" };
                    nextIndex = GetNextSharedIndex(basePath, tripleTasks);

                    SaveTaskFiles(basePath, "19", nextIndex, Answer19_Path.Text);
                    SaveTaskFiles(basePath, "20", nextIndex, Answer20_Path.Text);
                    SaveTaskFiles(basePath, "21", nextIndex, Answer21_Path.Text);
                }
                else
                {
                    if (string.IsNullOrEmpty(NewTask_AnswerPath.Text))
                    {
                        NotificationWindow.QuickShow(
                            "Создание задания.",
                            "Необходимо указать файл с ответом.",
                            NotificationType.Warning
                            );
                        return;
                    }

                    nextIndex = GetNextSharedIndex(basePath, new[] { selected });
                    SaveTaskFiles(basePath, selected, nextIndex, NewTask_AnswerPath.Text);
                }

                NotificationWindow.QuickShow(
                    "Создание задания.",
                    $"Задание {selected} успешно добавлено! (номер папки: {nextIndex})",
                    NotificationType.Success
                    );
                ClearAllFields();
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Ошибка программы",
                    $"{ex.Message}",
                    NotificationType.Error
                    );
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string tag = button.Tag.ToString();

            string filter = "All files (*.*)|*.*";

            if (tag.StartsWith("TXT")) filter = "Text files (*.txt)|*.txt";
            if (tag == "PNG") filter = "Image files (*.png)|*.png";
            if (tag == "ExtraMulti") filter = "Data files|*.xlsx;*.xls;*.docx;*.doc;*.txt;*.odt;*.ods|All files|*.*";

            var ofd = new OpenFileDialog 
            { 
                Filter = filter,
                Multiselect = (tag == "ExtraMulti")
            };

            if (ofd.ShowDialog() is true)
            {
                switch (tag)
                {
                    case "PNG": NewTask_ImagePath.Text = ofd.FileName; break;
                    case "TXT": NewTask_AnswerPath.Text = ofd.FileName; break;
                    case "TXT19": Answer19_Path.Text = ofd.FileName; break;
                    case "TXT20": Answer20_Path.Text = ofd.FileName; break;
                    case "TXT21": Answer21_Path.Text = ofd.FileName; break;
                    case "ExtraMulti":
                        // Обрабатываем массив выбранных файлов
                        foreach (string fileName in ofd.FileNames)
                        {
                            var fileInfo = new FileInfo(fileName);

                            // Проверка на дубликаты в списке по полному пути
                            bool exists = ExtraFiles_ListBox.Items
                                .Cast<FileInfo>()
                                .Any(f => f.FullName == fileInfo.FullName);

                            if (!exists)
                                ExtraFiles_ListBox.Items.Add(fileInfo);
                        }
                        break;
                }
            }
        }

        private int GetNextSharedIndex(string basePath, string[] tasks)
        {
            int maxIndex = 0;
            foreach (var task in tasks)
            {
                string taskPath = Path.Combine(basePath, task);

                // Если папки с номером задания (напр. "27") нет — создаем её
                if (!Directory.Exists(taskPath))
                {
                    Directory.CreateDirectory(taskPath);
                    continue;
                }

                // Берем все подпапки, пытаемся распарсить их имена как числа
                var dirs = Directory.GetDirectories(taskPath)
                                    .Select(Path.GetFileName)
                                    .Where(name => int.TryParse(name, out _))
                                    .Select(int.Parse)
                                    .ToList();

                if (dirs.Any())
                {
                    int currentMax = dirs.Max();
                    if (currentMax > maxIndex) maxIndex = currentMax;
                }
            }
            return maxIndex + 1; // Возвращаем следующий свободный номер
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.Tag.ToString() != "ScoreTable")
            {

                var dialog = new OpenFolderDialog
                {
                    Title = "Выберите директорию",
                    InitialDirectory = "D:\\"
                };

                if (dialog.ShowDialog() is true)
                {
                    string selectedPath = dialog.FolderName;

                    switch (button.Tag.ToString())
                    {
                        case "TaskPath":
                            Setting_TaskPath.Text = selectedPath;
                            break;

                        case "OptionsPath":
                            Setting_OptionsPath.Text = selectedPath;
                            break;

                        case "AnswersPath":
                            Setting_AnswersPath.Text = selectedPath;
                            break;
                    }
                }
            }
            else
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Выберите файл",
                    InitialDirectory = "D:\\"
                };

                if (dialog.ShowDialog() is true)
                    Setting_ScoreTable.Text = dialog.FileName;
            }
        }

        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields(true);
        }
    }
}
