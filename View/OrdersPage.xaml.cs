using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PhoneShop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhoneShop.View;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OrdersPage : Page
{
    public OrdersViewModel ViewModel { get; set; } = new();
    public OrdersPage()
    {
        InitializeComponent();
    }
    private void Search_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as OrdersViewModel)?.Search();
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Xử lý selection
    }

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as OrdersViewModel)?.Confirm();
    }

    private void CancelOrder_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as OrdersViewModel)?.Cancel();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as OrdersViewModel)?.Delete();
    }
}
