using System.Windows.Controls;
using System.Windows;
using KEGE_Station.User_Controls;

namespace KEGE_Station
{
    public class GridFacade
    {
        private static GridFacade instance;

        private static MainLogo Logo;
        private static Grid GOP;
        private static Grid EOP;
        private static Grid CRP;

        private GridFacade() { }

        public static GridFacade Instance()
        {
            if (instance is null) 
                instance = new GridFacade();
            return instance;
        }

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

        public static void SetLogo(MainLogo logo)
        {
            Logo = logo;
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
