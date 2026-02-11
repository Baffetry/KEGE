using System.IO;
using Task_Data;
using KEGE_Station;
using Testing_Option;
using Microsoft.Win32;
using System.Text.Json;
using KEGE_Station.Windows;
using System.Windows.Controls;
using KEGE_Station.User_Controls;
using KEGE_Station.Models.Exceptions;

namespace Edit_Option
{
    public enum EditorActionType { AddFile, DeletePanel }

    public struct EditorHistoryItem
    {
        public EditorActionType ActionType { get; set; }
        public OptionTaskPanel TargetPanel { get; set; }
    }

    public class OptionEditor
    {
        private Stack<EditorHistoryItem> _history = new Stack<EditorHistoryItem>(); 

        private TestingOption _currentOption;
        private StackPanel _parentPanel;
        private Label _pathLabel;

        public OptionEditor(StackPanel panel, Label label)
        {
            _parentPanel = panel;
            _pathLabel = label;
            _currentOption = null;
        }

        public void ChoiseOption()
        {
            EraseAll();
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = App.GetResourceString("SaveOptionsPath");

                if (ofd.ShowDialog() is true)
                {
                    _pathLabel.Content = ofd.FileName;

                    string extension = Path.GetExtension(ofd.FileName).ToLower();

                    if (extension != ".json")
                        throw new WrongFileFormatException("Расширение файла не соответствует JSON формату.");

                    string json = File.ReadAllText(ofd.FileName);
                    _currentOption = JsonSerializer.Deserialize<TestingOption>(json);

                    if (_currentOption.TaskList.Count == 0)
                        throw new IncorrectContentException("Выбранный файл не соответствует необходимому формату");

                    foreach (var task in _currentOption.TaskList)
                    {
                        OptionTaskPanel otp = new OptionTaskPanel(_parentPanel, this, task);
                        _parentPanel.Children.Add(otp);
                    }
                    SortPanels();
                }
            }
            catch (WrongFileFormatException ex)
            {
                NotificationWindow.QuickShow(
                    "Выбор файла.",
                    ex.Message,
                    NotificationType.Error
                    );
                EraseAll();
            }
            catch (IncorrectContentException ex)
            {
                NotificationWindow.QuickShow(
                    "Выбор файла.",
                    ex.Message,
                    NotificationType.Error
                    );
                EraseAll();
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Ошибка программы.",
                    ex.Message,
                    NotificationType.Error
                    );
                EraseAll();
            }
        }

        public void EraseAll()
        {
            _parentPanel.Children.Clear();
            _pathLabel.Content = string.Empty;
            _currentOption = null;
        }

        public void Undo()
        {
            if (_history.Count == 0) return;

            var lastItem = _history.Pop();

            if (lastItem.ActionType == EditorActionType.AddFile)
            {
                // Отменяем добавление файла внутри конкретной панели
                lastItem.TargetPanel.UndoAdd();
            }
            else if (lastItem.ActionType == EditorActionType.DeletePanel)
            {
                // Возвращаем панель в UI
                _parentPanel.Children.Add(lastItem.TargetPanel);
                SortPanels();
            }
        }

        // Вспомогательные методы, которые будут вызывать панели
        public void RegisterAction(EditorActionType type, OptionTaskPanel panel)
        {
            _history.Push(new EditorHistoryItem { ActionType = type, TargetPanel = panel });
        }

        public void Save()
        {
            NotificationWindow notification;

            if (_pathLabel.Content is null || string.IsNullOrEmpty(_pathLabel.Content.ToString()))
            {
                NotificationWindow.QuickShow(
                    "Ошибка сохранения.",
                    "Выберите вариант для редактирования.",
                    NotificationType.Error
                    );
                return;
            }

            var taskPanels = _parentPanel.Children;
            List<TaskData> updatedTasks = new List<TaskData>();

            foreach (OptionTaskPanel panel in taskPanels)
            {
                panel.Task.TaskWeight = int.Parse(panel.TaskWeight);
                updatedTasks.Add(panel.Task);
            }

            _currentOption.TaskList = updatedTasks;

            SortPanels();

            string json = JsonSerializer.Serialize(_currentOption, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            File.WriteAllText(_pathLabel.Content.ToString(), json);

            NotificationWindow.QuickShow(
                "Сохранение варианта.",
                "Вариант успешно сохранён!",
                NotificationType.Success
                );

            EraseAll();
        }

        public void SortPanels()
        {
            // 1. Берем все дочерние элементы и приводим их к типу OptionTaskPanel
            var sortedList = _parentPanel.Children.Cast<OptionTaskPanel>()
                .OrderBy(p =>
                {
                    // Пытаемся распарсить номер как число для корректной сортировки (1, 2, 10 вместо 1, 10, 2)
                    if (int.TryParse(p.TaskNumber, out int num))
                        return num;
                    return int.MaxValue; // Если не число, кидаем в конец
                })
                .ToList();

            // 2. Очищаем панель и добавляем отсортированные элементы
            _parentPanel.Children.Clear();
            foreach (var panel in sortedList)
            {
                _parentPanel.Children.Add(panel);
            }
        }
    }
}
