using KEGE_Station.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Task_Data;

namespace KEGE_Station.User_Controls
{
    /// <summary>
    /// Interaction logic for OptionTaskPanel.xaml
    /// </summary>
    public partial class OptionTaskPanel : UserControl
    {
        private Stack<OptionTaskPanel> _editedTask;
        private Stack<string> _addedFiles;

        private FilesBrowseWindow _browseWindow;
        private StackPanel _parentPanel;
        private TaskData _task;

        public TaskData Task
            => _task;

        public string TaskNumber
        {
            get { return _task.TaskNumber; }
            set { _taskNumber.Text = value; }
        }

        public string TaskWeight
        {
            get { return _taskWeight.Text; }
            set { _taskWeight.Text = value; }
        }

        public string FilesAmount
        {
            get { return _task.Files.Count.ToString(); }
            set { _taskFilesAmountButton.Content = value; }
        }

        public OptionTaskPanel()
        {
            InitializeComponent();
            SetButtonsBehavior();
        }

        public OptionTaskPanel(StackPanel panel, Stack<OptionTaskPanel> editedTask, TaskData task)
        {

            _parentPanel = panel;
            _editedTask = editedTask;
            _task = task;

            InitializeComponent();
            SetButtonsBehavior();
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            TaskNumber = _task.TaskNumber;
            _taskWeight.Text = _task.TaskWeight.ToString();
            FilesAmount = _task.Files.Count.ToString();
        }

        public void UndoAdd() // При отчистке панели должны отменяться все действия, если они не были сохранены
        {
            string fileName = _addedFiles.Pop();

            var file = _task.Files.FirstOrDefault(x => x.FileName.Equals(fileName));
            _task.Files.Remove(file);

            UpdateInfo();
        }

        private void SetButtonsBehavior()
        {
            // Green behavior
            ButtonBehavior.Apply(_taskAddFileButton);

            // Red behavior
            ButtonBehavior.Apply(_taskRemoveButton, true);
        }

        private void _taskFilesAmountButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _browseWindow?.Close();

            _browseWindow = new FilesBrowseWindow(_task.Files);

            Point locationFromScreen = _taskFilesAmountButton.PointToScreen(new Point(0, 0));

            _browseWindow.Left = locationFromScreen.X + _taskFilesAmountButton.ActualWidth + 10;
            _browseWindow.Top = locationFromScreen.Y - (_browseWindow.Height / 2) + (_taskFilesAmountButton.ActualHeight / 2);

            _browseWindow.Show();
        }

        private void _taskFilesAmountButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_browseWindow != null)
            {
                _browseWindow.CloseWithAnimation();
                _browseWindow = null;
            }
        }

        private void _taskRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_addedFiles != null)
                _addedFiles.Clear();
            _parentPanel.Children.Remove(this);
        }

        private void _taskAddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (_addedFiles is null)
                _addedFiles = new Stack<string>();

            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() is true)
            {
                string fileName = Path.GetFileName(ofd.FileName);
                byte[] data = File.ReadAllBytes(ofd.FileName);

                FileData newFile = new FileData()
                {
                    FileName = fileName,
                    Data = data
                };

                _task.Files.Add(newFile);
                UpdateInfo();

                _editedTask.Push(this);
                _addedFiles.Push(fileName);
            }
        }

        private void _taskWeight_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}
