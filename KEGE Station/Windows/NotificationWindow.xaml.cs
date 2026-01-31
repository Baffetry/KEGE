using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace KEGE_Station.Windows
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>

    public partial class NotificationWindow : Window
    {
        public MessageBoxResult resutl {  get; set; }
        public string IconSource { get; set; }
        public string TitleText { get; set; }
        public string MessageText { get; set; }

        public NotificationWindow(string messageText, bool isError = false)
        {
            InitializeComponent();

            IconSource = isError 
                ? "/Resources/Icons/error96x96.png"
                : "/Resources/Icons/warning96x96.png";

            TitleText = isError
                ? "Ошибка"
                : "Предупреждение";

            MessageText = messageText;
            
            ButtonBehavior.Apply(_Close_btn, true);

            SetProps();
        }

        public void Show(string messageText, bool isError = false)
        {
            var mb = new NotificationWindow(messageText, isError);
            mb.Show();
        }

        public void SetProps()
        {
            _Icon.Source = new BitmapImage(new Uri(IconSource, UriKind.Relative));
            _Title.Text = TitleText;
            _Message.Text = MessageText;
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
