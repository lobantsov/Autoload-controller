using System.Windows;
using Autoload_control.Classes;
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
    }
}