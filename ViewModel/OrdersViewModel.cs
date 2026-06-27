using PhoneShop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.ViewModel
{
    public class OrdersViewModel
    {
        public ObservableCollection<Order> Orders { get; set; } = new();
        public ObservableCollection<Order> FilteredOrders { get; set; } = new();

        public string SearchText { get; set; }
        public string SelectedStatus { get; set; } = "Tất cả";
        public Order? SelectedOrder { get; set; }

        public OrdersViewModel()
        {
            LoadSampleData();
            FilteredOrders = new ObservableCollection<Order>(Orders);
        }

        private void LoadSampleData()
        {
            Orders.Add(new Order
            {
                OrderId = "ORD0001",
                OrderDate = DateTime.Now.AddDays(-2),
                CustomerName = "Nguyễn Văn A",
                CustomerPhone = "0123456789",
                CustomerAddress = "123 Đường ABC, Q.1, TP.HCM",
                Status = OrderStatus.Paid,
                Discount = 0,
                PaidAmount = 25000000,
                Items = new ObservableCollection<OrderItem>
                {
                    new OrderItem { ProductId = 1, ProductName = "iPhone 15 Pro Max", Quantity = 1, UnitPrice = 25000000 }
                }
            });

            Orders.Add(new Order
            {
                OrderId = "ORD0002",
                OrderDate = DateTime.Now.AddDays(-1),
                CustomerName = "Trần Thị B",
                CustomerPhone = "0987654321",
                CustomerAddress = "456 Đường XYZ, Q.3, TP.HCM",
                Status = OrderStatus.New,
                Discount = 500000,
                PaidAmount = 0,
                Items = new ObservableCollection<OrderItem>
                {
                    new OrderItem { ProductId = 9, ProductName = "Samsung S24 Ultra", Quantity = 1, UnitPrice = 28000000 },
                    new OrderItem { ProductId = 2, ProductName = "AirPods Pro 2", Quantity = 2, UnitPrice = 7000000 }
                }
            });

            Orders.Add(new Order
            {
                OrderId = "ORD0003",
                OrderDate = DateTime.Now,
                CustomerName = "Lê Văn C",
                CustomerPhone = "0369852147",
                CustomerAddress = "789 Đường DEF, Q.7, TP.HCM",
                Status = OrderStatus.Cancelled,
                Discount = 0,
                PaidAmount = 0,
                Note = "Khách hủy đơn",
                Items = new ObservableCollection<OrderItem>
                {
                    new OrderItem { ProductId = 23, ProductName = "MacBook Pro 14\" M3", Quantity = 1, UnitPrice = 64000000 }
                }
            });
        }

        public void Search()
        {
            FilteredOrders.Clear();
            foreach (var o in Orders)
            {
                bool match = true;
                if (!string.IsNullOrEmpty(SearchText))
                {
                    match = o.OrderId.Contains(SearchText) || o.CustomerName.Contains(SearchText);
                }
                if (match) FilteredOrders.Add(o);
            }
        }

        public void Delete()
        {
            if (SelectedOrder != null)
            {
                Orders.Remove(SelectedOrder);
                FilteredOrders.Remove(SelectedOrder);
                SelectedOrder = null;
            }
        }

        public void Confirm()
        {
            if (SelectedOrder != null)
                SelectedOrder.Status = OrderStatus.Paid;
        }

        public void Cancel()
        {
            if (SelectedOrder != null)
                SelectedOrder.Status = OrderStatus.Cancelled;
        }

    }
}
