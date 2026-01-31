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
        private string resultDirectoryPath = "D:\\Temp\\Results";
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
            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.InitialDirectory = resultDirectoryPath;
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
                        NotificationWindow nw = new NotificationWindow(ex.Message);
                        nw.Show();
                        continue;
                    }
                    _parentPanel.Children.Add(panel);
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
