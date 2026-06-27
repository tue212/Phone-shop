using PhoneShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PhoneShop.DataAccess
{
    public class MockProductRepo: IRepo<Product>
    {

        private readonly string _connectionString =
        "Host=aws-1-ap-southeast-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.jvgzngbznlwsweakmexr;Password=tuepxt01@SB;SSL Mode=Require;Trust Server Certificate=true;";

        private NpgsqlConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await using var conn = CreateConnection();
                await conn.OpenAsync();
                return conn.State == System.Data.ConnectionState.Open;
            }
            catch (Exception ex)
            {
                throw new Exception($"TestConnectionAsync failed: {ex.Message}", ex);
            }
        }
       
        public async Task<PagedResult<Product>> GetAll(PagingRequest? info = null)
        {
            try
            {

                info ??= new PagingRequest();

                var products = new List<Product>();

                await using var conn = CreateConnection();
                await conn.OpenAsync();

                var sql = @"
            SELECT name, import_price, sale_price, count, description, cat_id, image_path
            FROM product
            ORDER BY id DESC;";

                await using var cmd = new NpgsqlCommand(sql, conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Name = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        ImportPrice = reader.IsDBNull(7) ? 0m : reader.GetDecimal(7),
                        SalePrice = reader.IsDBNull(1) ? 0m : reader.GetDecimal(1),
                        Quantity = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                        Description = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        CatId = reader.GetInt32(9),
                        ImagePath = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    });
                }

                int totalItems = products.Count;

                var items = products
                    .Skip((info.PageNumber - 1) * info.PageSize)
                    .Take(info.PageSize)
                    .ToList();

                return new PagedResult<Product>
                {
                    Item = items,
                    Pagination = new PagingMetadata
                    {
                        TotalItems = totalItems,
                        PageSize = info.PageSize,
                        PageNumber = info.PageNumber,
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"GetAll failed: {ex.Message}", ex);
            }
        }

        public async Task<Product> Insert(Product item)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
            INSERT INTO product
            (name, import_price, sale_price, count, description, cat_id, image_path)
            VALUES
            (@name, @import_price, @sale_price, @quantity, @description, @cat_id, @image_path)
            RETURNING id;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", item.Name);
            cmd.Parameters.AddWithValue("@sale_price", item.SalePrice);
            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
            cmd.Parameters.AddWithValue("@import_price", item.ImportPrice);
            cmd.Parameters.AddWithValue("@image_path", item.ImagePath ?? string.Empty);
            cmd.Parameters.AddWithValue("@cat_id", item.CatId);
            cmd.Parameters.AddWithValue("@description", item.Description ?? string.Empty);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                item.Id = reader.GetInt32(0);
            }

            return item;
        }

        public async Task<Product> Update(Product info)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
            UPDATE product
            SET name = @name,
                sale_price = @sale_price,
                count = @quantity,
                import_price = @import_price,
                image_path = @image_path,
                cat_id = @cat_id,
                description = @description
            WHERE id = @id;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", info.Name);
            cmd.Parameters.AddWithValue("@sale_price", info.SalePrice);
            cmd.Parameters.AddWithValue("@quantity", info.Quantity);
            cmd.Parameters.AddWithValue("@import_price", info.ImportPrice);
            cmd.Parameters.AddWithValue("@image_path", info.ImagePath ?? string.Empty);
            cmd.Parameters.AddWithValue("@description", info.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@cat_id", info.CatId);

            await cmd.ExecuteNonQueryAsync();
            return info;
        }
    }
}
