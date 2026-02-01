using Edit_Option;
using KEGE_Station.Windows;
using KEGE_Station.Work_Areas.Checking_the_results;
using Option_Generator;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace KEGE_Station
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string taskBasePath;
        private readonly string optionsPath;
        private readonly string answersPath;
        private GridFacade facade;
        private OptionEditor optionEditor;
        private ResultChecker resultChecker;

        public MainWindow()
        {
            InitializeComponent();
            SetButtonsBehavior();
            SetGrids();

            taskBasePath = App.GetResourceString("TaskBasePath");
            optionsPath = App.GetResourceString("SaveOptionsPath");
            answersPath = App.GetResourceString("SaveAnswersPath");


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
                MessageBox.Show("Введите кол-во вариантов", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var generator = new OptionGenerator(taskBasePath);

            for (int i = 0; i < count; i++)
            {
                var (option, response)= generator.GetOption();

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

            MessageBox.Show($"Создано {count} вариантов в папке: {optionsPath}",
                       "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}