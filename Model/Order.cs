
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
        public int OrderId { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal FinalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public string StatusText => Status switch
        {
            OrderStatus.New => "Mới tạo",
            OrderStatus.Paid => "Đã thanh toán",
            OrderStatus.Cancelled => "Đã hủy",
            _ => "Không xác định"
        };
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
