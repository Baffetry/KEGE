using System.IO;
using System.Windows;

namespace KEGE_Station
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeConfig();
        }

        public static string GetResourceString(string key)
        {
            return Current.Resources.Contains(key) ? Current.Resources[key] as string : string.Empty;
        }

        public static void SetResourceString(string key, string value)
        {
            if (Current.Resources.Contains(key))
                Current.Resources[key] = value;
            else
                Current.Resources.Add(key, value);
        }

        public static List<string> GetAllResourceKeys()
        {
            // Извлекаем все ключи, где значение является строкой
            return Current.Resources.Keys
                .Cast<object>()
                .Where(k => Current.Resources[k] is string)
                .Select(k => k.ToString())
                .ToList();
        }

        public static void InitializeConfig()
        {
            string cfgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string cfgFile = Path.Combine(cfgDir, "config.txt");

            if (File.Exists(cfgFile))
            {
                var lines = File.ReadAllLines(cfgFile);
                var validKeys = GetAllResourceKeys();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || !line.Contains("-")) continue;

                    var parts = line.Split('-', 2);
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (validKeys.Contains(key))
                        SetResourceString(key, value);
                }
            }
        }

    }
}
