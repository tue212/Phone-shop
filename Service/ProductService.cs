using PhoneShop.DataAccess;
using PhoneShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.Service
{
    public class ProductService
    {
        private IRepo<Product> _productRepo = new MockProductRepo();
        public ProductService() { 
        }
        public async Task<PagedResult<Product>> GetAll(PagingRequest? info = null)
        {
            try
            {
                return await _productRepo.GetAll(info);
            }
            catch (Exception ex)
            {
                throw new Exception("ProductService.GetAll failed: " + ex.Message, ex);
            }
        }

        public async Task<Product> Add(Product newItem)
        {
            return await _productRepo.Insert(newItem);
        }

        internal async Task<Product> Update(Product editItem)
        {
            return await _productRepo.Update(editItem);
        }
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                return await _productRepo.TestConnectionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("ProductService.TestConnectionAsync failed: " + ex.Message, ex);
            }
        }
    }
}
