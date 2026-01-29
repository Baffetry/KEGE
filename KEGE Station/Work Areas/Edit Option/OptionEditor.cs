using KEGE_Station.User_Controls;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Task_Data;
using Testing_Option;

namespace Edit_Option
{
    public class OptionEditor
    {
        private string savePath = @"D:\Temp\Options";
        private Stack<OptionTaskPanel> _editedTask;
        private TestingOption _currentOption;
        private StackPanel _parentPanel;
        private Label _pathLabel;

        public OptionEditor(StackPanel panel, Label label)
        {
            _parentPanel = panel;
            _pathLabel = label;
            _currentOption = null;
            _editedTask = new Stack<OptionTaskPanel>();
        }

        public void ChoiseOption()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultDirectory = savePath;

            if (ofd.ShowDialog() is true)
            {
                _pathLabel.Content = ofd.FileName;

                string json = File.ReadAllText(ofd.FileName);
                _currentOption = JsonSerializer.Deserialize<TestingOption>(json);

                foreach (var task in _currentOption.TaskList)
                {
                    OptionTaskPanel otp = new OptionTaskPanel(_parentPanel, _editedTask, task);
                    _parentPanel.Children.Add(otp);
                }
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
            if (_editedTask is null || _editedTask.Count == 0)
                return;

            var panel = _editedTask.Pop();
            panel.UndoAdd();
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_pathLabel.Content.ToString()))
            {
                MessageBox.Show("Save option error");
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
            

            string json = JsonSerializer.Serialize(_currentOption, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            File.WriteAllText(_pathLabel.Content.ToString(), json);

            MessageBox.Show("Файл успешно обновлён");

            EraseAll();
        }
    }
}
