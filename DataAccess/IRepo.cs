using PhoneShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess
{
    public class PagingRequest
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
    }
    public interface IRepo<TData> where TData : class
    {
        Task<PagedResult<TData>> GetAll(PagingRequest? info = null);
        Task<TData> Insert(TData item);
        Task<TData> Update(TData info);
        Task<bool> TestConnectionAsync();
    }
}
