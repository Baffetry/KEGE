using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KEGE_Station.Models.Custom_brushes;

namespace KEGE_Station.Windows
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>

    public enum NotificationType
    {
        Error,
        Warning,
        Success
    }

    public partial class NotificationWindow : Window
    {
        public MessageBoxResult result { get; set; }
        public Brush Background { get; set; }
        public string IconSource { get; set; }
        public string TitleText { get; set; }
        public string MessageText { get; set; }
        public string MessageTitleText { get; set; }

        public NotificationWindow()
        {
            InitializeComponent();
            SetButtonBehavior();
        }

        public static void QuickShow(string title, string text, NotificationType type, bool buttons = false)
        {
            var win = new NotificationWindow();
            win.ShowNotification(title, text, type);
        }

        public void ShowNotification(string messageTitle, string messageText, NotificationType type, bool buttons = false)
        {
            switch (type)
            {
                case NotificationType.Success:
                    SetProperties(
                        "/Resources/Icons/success96x96.png", "Уведомление",
                        CustomBrusher.Green);
                    SystemSounds.Asterisk.Play();
                    break;

                case NotificationType.Warning:
                    SetProperties(
                        "/Resources/Icons/warning96x96.png", "Предупреждение",
                        CustomBrusher.Yellow);
                    SystemSounds.Exclamation.Play();
                    break;

                case NotificationType.Error:
                    SetProperties("/Resources/Icons/error96x96.png", "Ошибка",
                        CustomBrusher.Red);
                    SystemSounds.Hand.Play();
                    break;
            }

            MessageTitleText = messageTitle;
            MessageText = messageText;
            _YesNoPanel.Visibility = buttons ? Visibility.Visible : Visibility.Collapsed;

            SetToXaml();

            this.ShowDialog();
        }

        public void SetButtonBehavior()
        {
            // Green
            ButtonBehavior.Apply(Yes_btn);

            // Red
            ButtonBehavior.Apply(No_btn, true);
            ButtonBehavior.Apply(_Close_btn, true);
        }

        public void SetProperties(string iconSource, string titleText, Brush background)
        {
            IconSource = iconSource;
            TitleText = titleText;
            Background = background;
        }

        public void SetToXaml()
        {
            _Icon.Source = new BitmapImage(new Uri(IconSource, UriKind.Relative));
            _Title.Text = TitleText;
            _NotificationType.Background = Background;
            _MessageTitle.Text = MessageTitleText;
            _Message.Text = MessageText;
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void _Close_btn_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Cancel;
            this.Close();
        }

        private void No_btn_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.No;
            this.Close();
        }

        private void Yes_btn_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Yes;
            this.Close();
        }
    }
}
