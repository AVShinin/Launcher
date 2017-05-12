using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo
{
    /// <summary>
    /// Логика взаимодействия для MainWindowDemo.xaml
    /// </summary>
    public partial class MainWindowDemo : Window
    {
        private bool IsChanging = false;
        private string __filename;
        public MainWindowDemo(string[] args)
        {
            InitializeComponent();
            this.Closing += MainWindowDemo_Closing;

            if (args.Length > 0)
                OpenFile(args[0]);
        }

        private void MainWindowDemo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsChanging && MessageBox.Show("Сохранить перед закрытием?", "Выход из программы", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (!string.IsNullOrWhiteSpace(__filename)) save_file(this, null);
                else saveas_file(this, null);
            }
        }

        private void open_about(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Название: Текстовый редактор.");
            builder.AppendLine($"Тип издания: Демонстрация возможностей");
            builder.AppendLine($"Тип проекта: Плагин");
            builder.AppendLine($"Автор: Шинин Александр");
            builder.AppendLine($"Сайт: Софтодел.рф");

            MessageBox.Show(builder.ToString(), "О Плагине", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void open_file(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Открыть файл";
            dlg.Filter = "Text files(*.txt)|*.txt|All files|*.*"; ;
            dlg.Multiselect = false;

            if(dlg.ShowDialog()==true)
            {
                OpenFile(dlg.FileName);
            }
        }
        private void OpenFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                __filename = fileName;
                editor.Text = System.IO.File.ReadAllText(fileName);

                if (this.Title.EndsWith("*")) this.Title = this.Title.TrimEnd('*');
                IsChanging = false;
            }
        }

        private void save_file(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists(__filename))
            {
                System.IO.File.WriteAllText(__filename, editor.Text);
            }
        }

        private void saveas_file(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Title = "Сохранить файл как";
            dlg.Filter = "Text files(*.txt)|*.txt|All files|*.*";
            dlg.FileName = "document.txt";
            
            if (dlg.ShowDialog() == true)
            {
                __filename = dlg.FileName;
                System.IO.File.WriteAllText(__filename, editor.Text);
            }
        }

        private void close_app(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void changed_textbox(object sender, TextChangedEventArgs e)
        {
            if (!this.Title.EndsWith("*")) this.Title += "*";
            IsChanging = true;
        }
    }
}
