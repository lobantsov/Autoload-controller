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
            _autoLoadStrucks = new ParseAutoload().GetAutostartApps();
            DGProcess.ItemsSource = _autoLoadStrucks;
        }
    }
}