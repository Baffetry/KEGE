using System.Windows;

namespace KEGE_Station
{
    public partial class App : Application
    {
        public static string GetResourceString(string key)
        {
            return Current.Resources.Contains(key) ? Current.Resources[key] as string : string.Empty;
        }
    }
}
