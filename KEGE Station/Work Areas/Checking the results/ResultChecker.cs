using System.IO;
using Microsoft.Win32;
using System.Text.Json;
using Participant_Result;
using KEGE_Station.Windows;
using System.Windows.Controls;
using KEGE_Station.User_Controls;
using KEGE_Station.Models.Exceptions;

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

        public bool CheckValidation(Result result)
        {
            if (result.Answers.Count == 0 || string.IsNullOrEmpty(result.Name) ||
                string.IsNullOrEmpty(result.SecondName) || string.IsNullOrEmpty(result.MiddleName))
                    return false;
            return true;
        }

        public void ChoiseDirectory()
        {
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
                        throw new WrongDirectoryException("Выбрана пустая директория.");

                    foreach (var file in jsonFiles)
                    {
                        string json = File.ReadAllText(file);

                        Result result = JsonSerializer.Deserialize<Result>(json);
                        _results.Add(result);

                        if (!CheckValidation(result))
                            throw new IncorrectContentException($"{file} не соответствует необходимому формату.");

                        ParticipantPanel panel = new ParticipantPanel(result);

                        _parentPanel.Children.Add(panel);
                    }
                }
            }
            catch (WrongDirectoryException ex)
            {
                NotificationWindow.QuickShow(
                    "Выбор директории.",
                    ex.Message,
                    NotificationType.Error
                    );
                return;
            }
            catch (IncorrectContentException ex)
            {
                NotificationWindow.QuickShow(
                    "Обработка файлов.",
                    ex.Message,
                    NotificationType.Error
                    );
                return;
            }
            catch (OptionNotFoundException ex)
            {
                NotificationWindow.QuickShow(
                    "Ошибка конфигурации.",
                    ex.Message,
                    NotificationType.Error
                    );
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Неверный формат файла.",
                    "Обнаруженный файл не соответствует формату результата.\n" +
                    "\nПроверьте путь к ответам в настройках.",
                    NotificationType.Error
                    );
                return;
            }

            // СОРТИРОВКА!

            //var temp = _parentPanel.Children.OfType<ParticipantPanel>().ToList();

            //_parentPanel.Children.Clear();

            //foreach (var panel in temp.OrderBy(x => x.ParticipantSecondName))
            //    _parentPanel.Children.Add(panel);
        }
    }
}
