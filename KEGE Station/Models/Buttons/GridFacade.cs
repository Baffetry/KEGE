using System.Windows.Controls;
using System.Windows;

namespace KEGE_Station
{
    public class GridFacade
    {
        private static Grid Logo;
        private static Grid GOP;
        private static Grid EOP;
        private static Grid CRP;

        public void OpenLogo()
        {
            GOP.Visibility = Visibility.Collapsed;
            EOP.Visibility = Visibility.Collapsed;
            CRP.Visibility = Visibility.Collapsed;

            Logo.Visibility = Visibility.Visible;
        }

        public void OpenGenerator()
        {
            Logo.Visibility = Visibility.Collapsed;
            EOP.Visibility = Visibility.Collapsed;
            CRP.Visibility = Visibility.Collapsed;

            GOP.Visibility = Visibility.Visible;
        }

        public void OpenEditorOption()
        {
            Logo.Visibility = Visibility.Collapsed;
            GOP.Visibility = Visibility.Collapsed;
            CRP.Visibility = Visibility.Collapsed;

            EOP.Visibility = Visibility.Visible;
        }

        public void OpenResults()
        {
            Logo.Visibility = Visibility.Collapsed;
            GOP.Visibility = Visibility.Collapsed;
            EOP.Visibility = Visibility.Collapsed;

            CRP.Visibility = Visibility.Visible;
        }

        public static void SetLogo(Grid grid)
        {
            Logo = grid;
        }
        public static void SetGOP(Grid grid)
        {
            GOP = grid;
        }
        public static void SetEOP(Grid grid)
        {
            EOP = grid;
        }
        public static void SetCRP(Grid grid)
        {
            CRP = grid;
        }
    }
}
