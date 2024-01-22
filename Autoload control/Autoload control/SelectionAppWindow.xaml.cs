using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Autoload_control.Classes;
using System.Windows.Media;

namespace Autoload_control;

public partial class SelectionAppWindow : Window
{
    private List<ApplicationStruck> _applicationStrucks;

    public SelectionAppWindow()
    {
        InitializeComponent();
        _applicationStrucks = new List<ApplicationStruck>();
        SelectedDataGrid.ItemsSource = _applicationStrucks;
        ApplicationForSelectedDataGrid.Items.Clear();
        ApplicationForSelectedDataGrid.ItemsSource = null;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void SelectionAppWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        ApplicationForSelectedDataGrid.ItemsSource = null;
        ApplicationForSelectedDataGrid.Items.Clear();
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.DataContext is ApplicationStruck item)
        {
            _applicationStrucks.Add(item);
        }
    }

    private void PlusButton_OnClick(object sender, RoutedEventArgs e)
    {
        foreach (var item in ApplicationForSelectedDataGrid.Items)
        {
            DataGridRow row =
                (DataGridRow)ApplicationForSelectedDataGrid.ItemContainerGenerator.ContainerFromItem(item);

            if (row != null)
            {
                CheckBox checkBox = FindUIElement.FindVisualChild<CheckBox>(row);

                if (checkBox != null && checkBox.IsChecked == true)
                {
                    ApplicationStruck data = (ApplicationStruck)row.Item;
                    if (!_applicationStrucks.Any(process => process.Name == data.Name))
                        _applicationStrucks.Add(data);
                }
            }
        }
    }
}