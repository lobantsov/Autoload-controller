using System.Windows;
using Autoload_control.Classes;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Autoload_control
{
    public partial class MainWindow : Window
    {
        private List<ApplicationStruck> _autoLoadStrucks;
        private List<ApplicationStruck> _activeProcessStrucks;
        private List<ApplicationStruck> _manulSelectStrucks = new();
        private Autoload_controller _autoloadController;//control powermode
        private ParseActiveProcess _activeProcess;//active process
        private SelectionAppWindow selectionAppWindow;//form for select app
        private string ApplicationWorkMode = "";

        public MainWindow()
        {
            InitializeComponent();

            //folder creation
            try
            {
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Console.WriteLine("created");
                }
                else
                {
                    Console.WriteLine("exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erorr {ex.Message}");
            }

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

            _activeProcessStrucks = new ParseActiveProcess().GetActiveApplication();
            _autoloadController = new Autoload_controller(null);
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

        

        private void BTWorkMode_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var control in FindUIElement.GetAllControls(this))
            {
                if (control is Grid grid && Convert.ToInt32(grid?.Tag) < 10)
                {
                    grid.Visibility = Visibility.Collapsed;

                    if (sender is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        if (frameworkElement.Tag.ToString() == (Convert.ToInt32(grid.Tag) + 10).ToString())
                        {
                            grid.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void OpenSelectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            selectionAppWindow = new SelectionAppWindow(_activeProcessStrucks, _manulSelectStrucks);
            selectionAppWindow.ShowDialog();
            if (selectionAppWindow.DialogResult == true)
            {
                
            }
        }

        private void AutoLoadRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (OpenSelectionButton != null)
                OpenSelectionButton.IsEnabled = false;
            switch (((RadioButton)sender).Tag?.ToString())
            {
             case "0":
                 ApplicationWorkMode = "AutoloadApplication";
                 break;
             
             case "1":
                 OpenSelectionButton.IsEnabled = true;
                 ApplicationWorkMode = "ManualSelectApplication";
                 break;
             
             case "2":
                 ApplicationWorkMode = "ActiveAplication";
                 break;
            }
        }
    }
}