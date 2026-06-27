using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneShop.Model;

namespace PhoneShop.DataAccess
{
    internal interface IOrderRepo : IRepo<Order>
    {
        Task<Order?> GetById(int orderId);
        Task<bool> Delete(int orderId);
        Task<PagedResult<Order>> SearchByDate(DateTime from, DateTime to, PagingRequest? info = null);
    }
}
