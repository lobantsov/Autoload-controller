using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autoload_control.Classes;
using System.Windows.Media;

namespace Autoload_control;

public partial class SelectionAppWindow : Window
{
    private List<ApplicationStruck> _applicationStrucksForSELECTED;
    private List<ApplicationStruck> _applicationStrucksForSELECT;

    public SelectionAppWindow(List<ApplicationStruck> _applicationStrucksForSELECT,
        List<ApplicationStruck> _applicationStrucksForSELECTED)
    {
        InitializeComponent();
        this._applicationStrucksForSELECTED = _applicationStrucksForSELECTED;
        this._applicationStrucksForSELECT = _applicationStrucksForSELECT;
        ApplicationForSelectedDataGrid.ItemsSource = _applicationStrucksForSELECT;

        SelectedDataGrid.ItemsSource = _applicationStrucksForSELECTED;
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

    private void PlusButton_OnClick(object sender, RoutedEventArgs e)
    {
        DataGridRowContr(ApplicationForSelectedDataGrid, SelectedDataGrid,
            _applicationStrucksForSELECTED, _applicationStrucksForSELECT);
    }

    private void MinusButton_OnClick(object sender, RoutedEventArgs e)
    {
        DataGridRowContr(SelectedDataGrid, ApplicationForSelectedDataGrid,
            _applicationStrucksForSELECT, _applicationStrucksForSELECTED);
    }

    private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is T result)
            {
                return result;
            }
            else
            {
                T descendant = FindVisualChild<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }
        }

        return null;
    }

    private void DataGridRowContr(DataGrid appSelectedDataGrid, DataGrid appSecetDataGrid,
        List<ApplicationStruck> appList,
        List<ApplicationStruck> appList2)
    {
        // for (int i = 0; i < appList2.Count; i++)
        // {
        //     if (appList2[i].IsSelected)
        //     {
        //         if (appList.All(process => process.Name != appList2[i].Name))
        //         {
        //             appList.Add(appList2[i]);
        //         }
        //         appList2.RemoveAt(i);
        //         i--;
        //     }
        // }
        //
        // appSecetDataGrid.Items.Refresh();
        // appSelectedDataGrid.Items.Refresh();

    }
}