using System.Windows;
using Autoload_control.Classes;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace Autoload_control
{
    public partial class MainWindow : Window
    {
        private List<AutoLoadStruck> _autoLoadStrucks;

        public MainWindow()
        {
            InitializeComponent();
            _autoLoadStrucks = new ParseAutoload().GetStartupApplications();
            foreach (var VARIABLE in _autoLoadStrucks)
            {
                Console.WriteLine($"Name: {VARIABLE.Name}");
                Console.WriteLine($"Enabled: {VARIABLE.IsEnabled}");
                Console.WriteLine($"Comand: {VARIABLE.Command}");
                Console.WriteLine($"Location: {VARIABLE.Location}");
                Console.WriteLine($"User: {VARIABLE.User}");
                Console.WriteLine($"Icon: {VARIABLE.IconPath}");
                Console.WriteLine("-------------------------------------");
            }

            DGProcess.ItemsSource = _autoLoadStrucks;
        }

        private void DataGridCell_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var a = DGProcess.SelectedCells[1].Item;
            Process.Start("explorer.exe", ((AutoLoadStruck)a).Command);
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var a = DGProcess.SelectedCells[1].Item;
                string command = ((AutoLoadStruck)a).Command;
                command = command.LastIndexOf('\\') >= 0 ? command.Substring(0, command.LastIndexOf('\\')) : command;
                Process.Start("explorer.exe", command);
            }
            catch (Exception exception)
            { }
        }

        private void LBProcesses_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DGProcess.Visibility = Visibility.Visible;
        }
    }
}