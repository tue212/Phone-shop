using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PhoneShop.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhoneShop;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProductsPage : Page
{
    public ProductViewModel ViewModel { get; set; } = new();
    public ProductsPage()
    {
        this.InitializeComponent();


    }

    private void listViewSwitchBtn_Click(object sender, RoutedEventArgs e)
    {
        var newTemplate = (DataTemplate)container.Resources["ListTemplate"];
        productListView.ItemTemplate = newTemplate;
    }

    private void thumbnailViewSwitchBtn_Click(object sender, RoutedEventArgs e)
    {
        var newTemplate = (DataTemplate)container.Resources["ThumbnailTemplate"];
        productListView.ItemTemplate = newTemplate;
    }

    private T? FindParent<T>(DependencyObject obj) where T: DependencyObject
    {
        var parent = VisualTreeHelper.GetParent(obj);
        if (parent == null) return null;
        if (parent is T typed) return typed;
        return FindParent<T>(parent);
    }

    private void productListView_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var control = sender as ListView;
        if (control == null) return;

        if (e.GetCurrentPoint(control).Properties.IsRightButtonPressed)
        {
            ListViewItem? item = FindParent<ListViewItem>((DependencyObject) e.OriginalSource);
            if (item != null)
            {
                control.SelectedItem = item.Content;
            }    
        }
    }

    private void PaginationControl_PageChanged(int obj)
    {
        ViewModel.ChangedPageCommand.Execute(obj);
    }
}
