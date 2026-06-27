
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.Model
{

    public class Order
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Note { get; set; }
        public ObservableCollection<OrderItem> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
        public decimal RemainingAmount => TotalAmount - Discount - PaidAmount;
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }

    public enum OrderStatus
    {
        New,
        Paid,
        Cancelled
    }

}
