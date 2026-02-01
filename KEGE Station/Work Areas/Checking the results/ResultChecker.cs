using Participant_Result;
using KEGE_Station.User_Controls;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using KEGE_Station.Windows;
using KEGE_Station.Models.Exceptions;
using System.Linq.Expressions;

namespace KEGE_Station.Work_Areas.Checking_the_results
{
    internal class ResultChecker
    {
        private StackPanel _parentPanel;
        private Label _labelPath;
        private List<Result> _results;

        public ResultChecker(StackPanel parentPanel, Label label) 
        {
            _parentPanel = parentPanel;
            _labelPath = label;
            _results = new List<Result>();
        }

        public void ChoiseDirectory()
        {
            string errors = string.Empty;

            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.Title = "Выберите папку с результатами";

            try
            {

                if (ofd.ShowDialog() is true)
                {
                    _parentPanel.Children.Clear();
                    _labelPath.Content = ofd.FolderName;

                    var jsonFiles = Directory.GetFiles(ofd.FolderName, "*.json");

                    if (jsonFiles.Length == 0)
                        throw new IncorrectContentException("Выбранная папка не содержит файлы с результатами");

                    foreach (var file in jsonFiles)
                    {
                        string json = File.ReadAllText(file);

                        Result result = JsonSerializer.Deserialize<Result>(json);
                        _results.Add(result);

                        ParticipantPanel panel = null;

                        try
                        {
                            panel = new ParticipantPanel(result);
                        }
                        catch (OptionNotFoundException ex)
                        {
                            errors += ex.Message + '\n';
                            continue;
                        }

                        _parentPanel.Children.Add(panel);
                    }
                }
            }
            catch (IncorrectContentException ex)
            {
                NotificationWindow notification = new NotificationWindow("Неверный формат файла", ex.Message, true);
                notification.Show();
                return;
            }
            catch (Exception ex)
            {
                NotificationWindow notification = new NotificationWindow("Неверный формат файла", 
                    "Обнаруженный файл не соответствует формату результата", true);
                notification.Show();
                return;
            }

            if (errors != string.Empty)
            {
                NotificationWindow nw = new NotificationWindow("Не удалось найти варианты", errors, true);
                nw.Show();
            }

            // СОРТИРОВКА!

            //var temp = _parentPanel.Children.OfType<ParticipantPanel>().ToList();

            //_parentPanel.Children.Clear();

            //foreach (var panel in temp.OrderBy(x => x.ParticipantSecondName))
            //    _parentPanel.Children.Add(panel);
        }
    }
}
