using Participant_Result;
using KEGE_Station.User_Controls;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using KEGE_Station.Windows;

namespace KEGE_Station.Work_Areas.Checking_the_results
{
    internal class ResultChecker
    {
        private readonly string _resultDirectoryPath;
        private StackPanel _parentPanel;
        private Label _labelPath;
        private List<Result> _results;

        public ResultChecker(StackPanel parentPanel, Label label) 
        {
            _resultDirectoryPath = App.GetResourceString("LoadResults");
            _parentPanel = parentPanel;
            _labelPath = label;
            _results = new List<Result>();
        }

        public void ChoiseDirectory()
        {
            string errors = string.Empty;

            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.InitialDirectory = _resultDirectoryPath;
            ofd.Title = "Выберите папку с результатами";

            if (ofd.ShowDialog() is true)
            {
                _parentPanel.Children.Clear();
                _labelPath.Content = ofd.FolderName;

                var jsonFiles = Directory.GetFiles(ofd.FolderName);

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
                    catch (Exception ex)
                    {
                        errors += ex.Message + '\n';
                        continue;
                    }
                    _parentPanel.Children.Add(panel);
                }

                if (errors != string.Empty)
                {
                    NotificationWindow nw = new NotificationWindow("Не удалось найти вариант, которые решали:", errors);
                    nw.Show();
                }
            }

            // СОРТИРОВКА!

            //var temp = _parentPanel.Children.OfType<ParticipantPanel>().ToList();

            //_parentPanel.Children.Clear();

            //foreach (var panel in temp.OrderBy(x => x.ParticipantSecondName))
            //    _parentPanel.Children.Add(panel);
        }
    }
}
