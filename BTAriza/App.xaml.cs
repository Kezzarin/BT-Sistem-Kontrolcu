using System.Windows;
using BTAriza.Services;

namespace BTAriza
{
    public partial class App : Application
    {
        public static DatabaseService? DB { get; private set; }
        public static WmiService Wmi { get; } = new WmiService();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigService.Yukle();
            DB = new DatabaseService(ConfigService.Config.VeritabaniYolu);
        }
    }
}
