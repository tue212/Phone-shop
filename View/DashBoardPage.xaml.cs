using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhoneShop.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoardPage : Page
    {
        public DashBoardPage()
        {
            InitializeComponent();

            // Hiển thị ngày hiện tại
            todayTextBlock.Text = "Hôm nay: " + DateTime.Now.ToString("dd/MM/yyyy");

            // TODO: Load dữ liệu từ database
            LoadDashboardData();
        }
        private void LoadDashboardData()
        {
            // TODO: Thay thế bằng dữ liệu thật từ CSDL
            revenueTodayText.Text = "150.000.000 đ";
            ordersTodayText.Text = "12";
            inventoryText.Text = "85";
            pendingOrdersText.Text = "3";
        }
    }
}
