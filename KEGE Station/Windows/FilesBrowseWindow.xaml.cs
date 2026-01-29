using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Task_Data;

namespace KEGE_Station.Windows
{
    /// <summary>
    /// Interaction logic for FilesBrowseWindow.xaml
    /// </summary>
    public partial class FilesBrowseWindow : Window
    {
        private List<FileData> _files;

        public FilesBrowseWindow(List<FileData> files)
        {
            _files = files;
            InitializeComponent();
        }

        private void Browse()
        {
            if (_files is null) return;

            for (int row = 0; row < _files.Count; row++)
            {
                string name = _files[row].FileName;
                byte[] data = _files[row].Data;

                string extension = Path.GetExtension(name);
                string iconPath = string.Empty;

                switch (extension)
                {
                    case ".txt":
                        iconPath = "/Resources/Extensions/txt96x96.png";
                        break;

                    case ".doc":
                    case ".docx":
                        iconPath = "/Resources/Extensions/word96x96.png";
                        break;

                    case ".xls":
                    case ".xlsx":
                        iconPath = "/Resources/Extensions/exel96x96.png";
                        break;

                    case ".ods":
                        iconPath = "/Resources/Extensions/ods96x96.png";
                        break;

                    default:
                        iconPath = "/Resources/Extensions/unknownFile96xx96.png";
                        break;
                }

                var label = GetLabel(name);
                var img = GetImage(iconPath);

                FileGrid.RowDefinitions.Add(new RowDefinition());

                Grid.SetColumn(label, 0);
                Grid.SetColumn(img, 1);

                Grid.SetRow(label, row);
                Grid.SetRow(img, row);

                FileGrid.Children.Add(label);
                FileGrid.Children.Add(img);
            }
        }

        private Label GetLabel(string content)
        {
            return new Label()
            {
                Content = content,
                FontFamily = new FontFamily("/Resources/Fonts/#Inter"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 5, 5, 5),
                Width = 160
            };
        }

        private Image GetImage(string source)
        {
            return new Image()
            {
                Source = new BitmapImage(new Uri(source, UriKind.RelativeOrAbsolute)),
                Height = 50,
                Width = 50,
                Margin = new Thickness(5, 5, 5, 5)
            };
        }


        public void CloseWithAnimation()
        {
            ((Storyboard)Resources["ControlSlideOutAnimation"]).Begin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Browse();
            ((Storyboard)Resources["ControlSlideInAnimation"]).Begin();
        }

        private void ControlSlideOutAnimation_Completed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
