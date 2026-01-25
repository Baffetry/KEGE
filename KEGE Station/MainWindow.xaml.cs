using Option_Generator;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Testing_Option;

namespace KEGE_Station
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string taskBasePath = @"D:\\Data\\Files\\Задания КЕГЭ";
        private string savePath = @"D:\Temp\Options";
        private GridFacade facade = new GridFacade();

        public MainWindow()
        {
            InitializeComponent();
            SetButtonsBehavior();
            SetGrids();
        }

        private void SetGrids()
        {
            GridFacade.SetLogo(Logo);
            GridFacade.SetGOP(GenerateOptionsPanel);
            GridFacade.SetEOP(EditOptionPanel);
            GridFacade.SetCRP(CheckResultPanel);
        }

        private void SetButtonsBehavior()
        {
            // Green behavior
            ButtonBehavior.Apply(Logo_btn);
            ButtonBehavior.Apply(OpenGenerator_btn);
            ButtonBehavior.Apply(OpenEdit_btn);
            ButtonBehavior.Apply(OpenResults_btn);
            ButtonBehavior.Apply(GenerateOptionsPanel_Generate_btn);
            ButtonBehavior.Apply(EditOptionPanel_AddTask_btn);
            ButtonBehavior.Apply(EditOptionPanel_Save_btn);
            ButtonBehavior.Apply(ChoiceOption);
            ButtonBehavior.Apply(ChoiceRepository);

            // Reg behavior
            ButtonBehavior.Apply(GenerateOptionsPanel_Back_btn, true);
            ButtonBehavior.Apply(EditOptionPanel_Erase_btn, true);
            ButtonBehavior.Apply(Exit_btn, true);
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
                TestingOption option = generator.GetOption();

                string fileName = $"Вариант_{i + 1:000}_{DateTime.Now:dd-MM-yyyy}.json";
                string filePath = Path.Combine(savePath, fileName);

                string json = JsonSerializer.Serialize(option, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

                File.WriteAllText(filePath, json);
            }

            MessageBox.Show($"Создано {count} вариантов в папке: {savePath}",
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

        }
        #region Edit optional buttons
        private void EditOptionPanel_Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditOptionPanel_AddTask_btn_Click(object sender, RoutedEventArgs e)
        {
            Border border = new Border();
            Grid.SetRow(border, 0);
            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            border.BorderThickness = new Thickness(3);
            border.CornerRadius = new CornerRadius(15);
            border.Height = 90;
            border.Margin = new Thickness(10, 10, 10, 10);

            Grid innerGrid = new Grid();
            innerGrid.Margin = new Thickness(5);

            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                if (i < 4)
                    colDef.Width = new GridLength(1, GridUnitType.Star);
                else
                    colDef.Width = new GridLength(80);
                innerGrid.ColumnDefinitions.Add(colDef);
            }

            TextBox textBox1 = new TextBox();
            textBox1.BorderThickness = new Thickness(0, 0, 0, 1);
            textBox1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            textBox1.Width = 70;
            textBox1.Height = 30;
            Grid.SetColumn(textBox1, 0);
            textBox1.HorizontalAlignment = HorizontalAlignment.Center;
            textBox1.HorizontalContentAlignment = HorizontalAlignment.Center;
            textBox1.FontFamily = new FontFamily("/Fonts/#Inter");
            textBox1.FontSize = 30;
            textBox1.FontWeight = FontWeights.SemiBold;
            textBox1.MaxLength = 2;

            TextBox textBox2 = new TextBox();
            textBox2.BorderThickness = new Thickness(0, 0, 0, 1);
            textBox2.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            textBox2.Width = 70;
            textBox2.Height = 30;
            Grid.SetColumn(textBox2, 1);
            textBox2.HorizontalAlignment = HorizontalAlignment.Center;
            textBox2.HorizontalContentAlignment = HorizontalAlignment.Center;
            textBox2.FontFamily = new FontFamily("/Fonts/#Inter");
            textBox2.FontSize = 30;
            textBox2.FontWeight = FontWeights.SemiBold;
            textBox2.MaxLength = 2;

            Button addFile1 = new Button();
            addFile1.Name = "AddFile1";
            Grid.SetColumn(addFile1, 2);
            addFile1.Width = 65;
            addFile1.Height = 65;
            addFile1.Template = (ControlTemplate)FindResource("NoMouseOverButtonTemplate");

            Image addImage = new Image();
            addImage.Source = new BitmapImage(new Uri("/Resources/plus96x96.png", UriKind.RelativeOrAbsolute));
            addImage.Width = 40;
            addImage.Height = 40;
            addFile1.Content = addImage;

            TextBox textBox3 = new TextBox();
            textBox3.BorderThickness = new Thickness(0, 0, 0, 1);
            textBox3.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            textBox3.Width = 70;
            textBox3.Height = 30;
            Grid.SetColumn(textBox3, 3);
            textBox3.HorizontalAlignment = HorizontalAlignment.Center;
            textBox3.HorizontalContentAlignment = HorizontalAlignment.Center;
            textBox3.FontFamily = new FontFamily("/Fonts/#Inter");
            textBox3.FontSize = 30;
            textBox3.FontWeight = FontWeights.SemiBold;
            textBox3.MaxLength = 2;

            Button deleteTask1 = new Button();
            deleteTask1.Name = "DeleteTask1";
            Grid.SetColumn(deleteTask1, 4);
            deleteTask1.Width = 65;
            deleteTask1.Height = 65;
            deleteTask1.Template = (ControlTemplate)FindResource("RedButton");
            deleteTask1.Click += DeletePanel1_Click;

            Image deleteImage = new Image();
            deleteImage.Source = new BitmapImage(new Uri("/Resources/erase96x96.png", UriKind.RelativeOrAbsolute));
            deleteImage.Width = 40;
            deleteImage.Height = 40;
            deleteTask1.Content = deleteImage;

            innerGrid.Children.Add(textBox1);
            innerGrid.Children.Add(textBox2);
            innerGrid.Children.Add(addFile1);
            innerGrid.Children.Add(textBox3);
            innerGrid.Children.Add(deleteTask1);

            border.Child = innerGrid;

            EditOptionPanel_TaskPanel.Children.Add(border);
        }

        private void EditOptionPanel_Erase_btn_Click(object sender, RoutedEventArgs e)
        {
            EditOptionPanel_TaskPanel.Children.Clear();
        }

        private void DeletePanel1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Parent is Grid ingrid && ingrid.Parent is Border bord)
            {
                EditOptionPanel_TaskPanel.Children.Remove(bord);
            }
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

        }

        private void CheckResultPanel_ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }

        private void EditOptionPanel_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        private void EditOptionPanel_ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

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