using System.IO;
using Exceptions;
using Edit_Option;
using System.Windows;
using Option_Generator;
using System.Text.Json;
using KEGE_Station.Windows;
using System.Windows.Controls;
using KEGE_Station.Work_Areas.Checking_the_results;

namespace KEGE_Station
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridFacade facade;
        private OptionEditor optionEditor;
        private ResultChecker resultChecker;

        public MainWindow()
        {
            InitializeComponent();
            SetGrids();
            SetButtonsBehavior();

            optionEditor = new OptionEditor(EditOptionPanel_TaskPanel, _OptionPathLabel);
            resultChecker = new ResultChecker(_ResultPanel, RepositoryPath);
        }

        private void SetGrids()
        {
            facade = GridFacade.Instance();

            GridFacade.SetLogo(LogoControl);
            GridFacade.SetGOP(GenerateOptionsPanel);
            GridFacade.SetEOP(EditOptionPanel);
            GridFacade.SetCRP(CheckResultPanel);
            GridFacade.SetSG(SettingsControlPanel);
        }
        private void SetButtonsBehavior()
        {
            // Green behavior
            ButtonBehavior.Apply(GenerateOptionsPanel_Generate_btn);
            ButtonBehavior.Apply(EditOptionPanel_Save_btn);
            ButtonBehavior.Apply(ChoiceOption);
            ButtonBehavior.Apply(ChoiceRepository);

            // Red behavior
            ButtonBehavior.Apply(GenerateOptionsPanel_Back_btn, true);
            ButtonBehavior.Apply(EditOptionPanel_Erase_btn, true);
            ButtonBehavior.Apply(EditOptionPanel_Undo_btn, true);
        }

        #region Buttons

        #region Logo
        private void Logo_btn_Click(object sender, RoutedEventArgs e)
        {
            facade.OpenLogo();
        }
        #endregion

        #region Generator
        private void OpenGenerator_btn_Click(object sender, RoutedEventArgs e)
        {
            facade.OpenGenerator();
        }

        #region Generate options buttons
        private void GenerateOptionsPanel_Generate_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AmountOfOptions.Text, out int count))
            {
                NotificationWindow.QuickShow(
                    "Ошибка генерации.",
                    "Введите корректное количество вариантов.",
                    NotificationType.Error
                    );
                return;
            }

            string taskBasePath = App.GetResourceString("TaskBasePath");
            string optionsPath = App.GetResourceString("SaveOptionsPath");
            string answersPath = App.GetResourceString("SaveAnswersPath");

            try
            {
                var generator = new OptionGenerator(taskBasePath);

                for (int i = 0; i < count; i++)
                {
                    var (option, response) = generator.GetOption();


                    string fileName = $"{option.OptionID}_Вариант_{i + 1:000}_{DateTime.Now:dd-MM-yyyy}.json";
                    string optionFilePath = Path.Combine(optionsPath, fileName);
                    string answerFilePath = Path.Combine(answersPath, fileName);

                    string jsonOption = JsonSerializer.Serialize(option, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    });

                    string jsonAnswer = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    });

                    File.WriteAllText(optionFilePath, jsonOption);
                    File.WriteAllText(answerFilePath, jsonAnswer);
                }

                NotificationWindow.QuickShow(
                    "Генерация вариантов.",
                    $"Варианты успешно созданы ({count} шт.) и сохранены в {optionsPath}.",
                    NotificationType.Success
                    );

                AmountOfOptions.Text = string.Empty;
            }
            catch (ConfigurationException cfgEx)
            {
                NotificationWindow.QuickShow(
                    "Ошибка конфигурации.",
                    cfgEx.Message,
                    NotificationType.Error
                    );
            }
            catch (Exception ex)
            {
                NotificationWindow.QuickShow(
                    "Ошибка генерации вариантов.",
                    "База заданий повреждена или не заполнена.\n\nПроверить названия папок " +
                    "(они должны быть названы числом - номером задания) или их наличие.",
                    NotificationType.Error
                    );
            }
        }

        private void GenerateOptionsPanel_Back_btn_Click(object sender, RoutedEventArgs e)
        {
            facade.OpenLogo();
        }
        #endregion
        #endregion

        #region Edit
        private void OpenEdit_btn_Click(object sender, RoutedEventArgs e)
        {
            facade.OpenEditorOption();
        }
        
        private void ChoiceOption_Click(object sender, RoutedEventArgs e)
        {
            optionEditor.ChoiseOption();
        }
        #region Edit optional buttons
        private void EditOptionPanel_Save_btn_Click(object sender, RoutedEventArgs e)
        {
            optionEditor.Save();
        }

        private void EditOptionPanel_Erase_btn_Click(object sender, RoutedEventArgs e)
        {
            optionEditor.EraseAll();
        }

        private void EditOptionPanel_Undo_btn_Click(object sender, RoutedEventArgs e)
        {
            optionEditor.Undo();
        }
        #endregion

        #endregion

        #region Results
        private void OpenResults_btn_Click(object sender, RoutedEventArgs e)
        {
            facade.OpenResults();
        }

        private void ChoiceRepository_Click(object sender, RoutedEventArgs e)
        {
            CloseAllStatisticWindows();
            resultChecker.ChoiseDirectory();
        }
        #endregion

        #region Exit
        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region ScrollViewer

        private void CheckResultPanel_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            if (scrollViewer.ExtentHeight > scrollViewer.ViewportHeight)
                _ResultPanel.Margin = new Thickness(10, 10, 15, 10);
            else
                _ResultPanel.Margin = new Thickness(10, 10, 25, 10);
        }

        private void CloseAllStatisticWindows()
        {
            var windowsToClose = Application.Current.Windows.OfType<StatisticWindow>().ToList();

            foreach (var window in windowsToClose)
            {
                window.Close();
            }
        }
        #endregion

        #region Text box input handler
        private void AmountOfOptions_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        #endregion

    }
}