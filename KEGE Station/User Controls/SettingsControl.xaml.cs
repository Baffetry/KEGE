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
        public SettingsControl()
        {
            InitializeComponent();
            SetButtonsBehavior();
        }

        private void SetButtonsBehavior()
        {
            ButtonBehavior.Apply(BrowseTaskBase_btn);
            ButtonBehavior.Apply(BrowseScoreTable_btn);
            ButtonBehavior.Apply(BrowseOptions_btn);
            ButtonBehavior.Apply(BrowseAnswers_btn);
            ButtonBehavior.Apply(SaveSettings_btn);
        }

        public void RefreshSettings()
        {
            Setting_TaskPath.Text = App.GetResourceString("TaskBasePath");
            Setting_ScoreTable.Text = App.GetResourceString("ScoreTable");
            Setting_OptionsPath.Text = App.GetResourceString("SaveOptionsPath");
            Setting_AnswersPath.Text = App.GetResourceString("SaveAnswersPath");
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

                Application.Current.Resources["TaskBasePath"] = Setting_TaskPath.Text;
                Application.Current.Resources["SaveOptionsPath"] = Setting_OptionsPath.Text;
                Application.Current.Resources["SaveAnswersPath"] = Setting_AnswersPath.Text;
                Application.Current.Resources["ScoreTable"] = Setting_ScoreTable.Text;

                NotificationWindow notificate = new NotificationWindow("Настройки путей", "Все пути успешно сохранены!");
                notificate.Show();
            }
            catch (Exception ex)
            {
                NotificationWindow notificate = new NotificationWindow("Настройки путей", $"Ну удалось сохранить файл: {ex.Message}", true);
                notificate.Show();
            }
        }
    }
}
