using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess
{
    public class PagedResult <TData> where TData : class
    {
        public List<TData>? Item { get; init; }
        public PagingMetadata Pagination { get; init; }=new ();
    }

    public class PagingMetadata
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int TotalItems { get; set; } = 0;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasNext => PageNumber < TotalPages;
        public bool HasPrevious => PageNumber > 1;
    }
}
