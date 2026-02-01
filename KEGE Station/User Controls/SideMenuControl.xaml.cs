using System.Windows;
using System.Windows.Controls;

namespace KEGE_Station.User_Controls
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        private GridFacade facade;

        public SideMenuControl()
        {
            InitializeComponent();
            SetButtonProperties();

            facade = GridFacade.Instance();
        }

        private void SetButtonProperties()
        {
            // Green behavior
            ButtonBehavior.Apply(Home_btn);
            ButtonBehavior.Apply(Settings_btn);
            ButtonBehavior.Apply(OpenGenerator_btn);
            ButtonBehavior.Apply(OpenEdit_btn);
            ButtonBehavior.Apply(OpenResults_btn);

            // Red behavior
            ButtonBehavior.Apply(Exit_btn, true);
        }

        #region Buttons
        private void Exit_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void Settings_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            facade.OpenSettings();
        }

        private void Home_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            facade.OpenLogo();
        }

        private void OpenGenerator_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            facade.OpenGenerator();
        }

        private void OpenEdit_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            facade.OpenEditorOption();
        }

        private void OpenResults_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            facade.OpenResults();
        }
        #endregion
    }
}
