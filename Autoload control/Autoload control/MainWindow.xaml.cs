using System.Windows;
using Autoload_control.Classes;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace Autoload_control
{
    public partial class MainWindow : Window
    {
        private List<ApplicationStruck> _autoLoadStrucks;
        private Autoload_controller _autoloadController;
        private ParseActiveProcess _activeProcess;

        public MainWindow()
        {
            InitializeComponent();
            _autoLoadStrucks = new ParseAutoload().GetStartupApplications();
            // foreach (var VARIABLE in _autoLoadStrucks)
            // {
            //     Console.WriteLine($"Name: {VARIABLE.Name}");
            //     Console.WriteLine($"Comand: {VARIABLE.Command}");
            //     Console.WriteLine($"Location: {VARIABLE.Location}");
            //     Console.WriteLine($"User: {VARIABLE.User}");
            //     Console.WriteLine($"Icon: {VARIABLE.IconPath}");
            //     Console.WriteLine("-------------------------------------");
            // }
            DGProcess.ItemsSource = _autoLoadStrucks;
            _autoloadController = new Autoload_controller(_autoLoadStrucks);

            _activeProcess = new ParseActiveProcess();
            var a =_activeProcess.GetActiveApplication();
        }

        private void DataGridCell_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var a = DGProcess.SelectedCells[1].Item;
            Process.Start("explorer.exe", ((ApplicationStruck)a).Command);
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var a = DGProcess.SelectedCells[1].Item;
                string command = ((ApplicationStruck)a).Command;
                command = command.LastIndexOf('\\') >= 0 ? command.Substring(0, command.LastIndexOf('\\')) : command;
                Process.Start("explorer.exe", command);
            }
            catch (Exception exception)
            { }
        }

        private void LBProcesses_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var control in GetAllControls(this))
            {
                if (control is Grid grid && grid.Tag != null && Convert.ToInt32(grid.Tag) < 10)
                {
                    grid.Visibility = Visibility.Collapsed;

                    if (sender is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        if (frameworkElement.Tag.ToString() == (Convert.ToInt32(grid.Tag)+10).ToString())
                        {
                            grid.Visibility = Visibility.Visible;
                        }
                    }
                }
            }

        }
        private IEnumerable<DependencyObject> GetAllControls(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                foreach (var descendant in GetAllControls(child))
                {
                    yield return descendant;
                }
                yield return child;
            }
        }
    }
}