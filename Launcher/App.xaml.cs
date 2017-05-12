using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Launcher
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    { 
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main(string [] args)
        {
            try
            {
                Config.config.Load();
                Story.story.Add("Launcher").AddOrUpdate("Args", args);
                Story.story.Add("Launcher").AddOrUpdate("MainWindow", new MainWindow());
                var con = new Connector();
                con.InitialPlugins();
                App app = new App();
                app.Run(Story.story.Get("Launcher").Get<Window>("MainWindow"));
            }
            catch (Exception e)
            {
                var _path_base = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                var _path_exceptions = System.IO.Path.Combine(_path_base.FullName, "Exceptions.log");

                System.IO.File.AppendAllLines(_path_exceptions,
                    new[]
                    {
                        $"-О-Ш-И-Б-К-А-{DateTime.Now.ToString("dd-MM-yyyy-HH:mm:ss")}-",
                        $"{e.Message}"
                    });
                if (e.InnerException != null)
                {
                    System.IO.File.AppendAllLines(_path_exceptions,
                    new[]
                    {
                        $"{e.InnerException.Message}"
                    });
                }

                MessageBox.Show($"В приложении возникла ошибка.\nПодробнее в файле \"{_path_exceptions}\".", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
        }
    }
}
