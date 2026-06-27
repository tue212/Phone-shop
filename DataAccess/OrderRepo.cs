using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using PhoneShop.Model;

namespace PhoneShop.DataAccess
{
    internal class OrderRepo : IOrderRepo
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
            catch
            {
                return false;
            }
        }

        public async Task<PagedResult<Order>> GetAll(PagingRequest? info = null)
        {
            info ??= new PagingRequest();
            var items = new List<Order>();

            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
                SELECT order_id, create_time, final_price, status
                FROM orders
                ORDER BY create_time DESC, order_id DESC
                LIMIT @limit OFFSET @offset;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@limit", info.PageSize);
            cmd.Parameters.AddWithValue("@offset", (info.PageNumber - 1) * info.PageSize);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(ReadOrder(reader));
            }

            var totalItems = await CountAllAsync(conn);

            return new PagedResult<Order>
            {
                Item = items,
                Pagination = new PagingMetadata
                {
                    TotalItems = totalItems,
                    PageSize = info.PageSize,
                    PageNumber = info.PageNumber
                }
            };
        }

        public async Task<Order> Insert(Order item)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
                INSERT INTO orders (create_time, final_price, status)
                VALUES (@create_time, @final_price, @status)
                RETURNING order_id;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@create_time", item.CreateTime);
            cmd.Parameters.AddWithValue("@final_price", item.FinalPrice);
            cmd.Parameters.AddWithValue("@status", item.Status.ToString().ToLower());

            item.OrderId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return item;
        }

        public async Task<Order> Update(Order info)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
                UPDATE orders
                SET create_time = @create_time,
                    final_price = @final_price,
                    status = @status
                WHERE order_id = @order_id;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@create_time", info.CreateTime);
            cmd.Parameters.AddWithValue("@final_price", info.FinalPrice);
            cmd.Parameters.AddWithValue("@status", info.Status.ToString().ToLower());
            cmd.Parameters.AddWithValue("@order_id", info.OrderId);

            await cmd.ExecuteNonQueryAsync();
            return info;
        }

        public async Task<Order?> GetById(int orderId)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
                SELECT order_id, create_time, final_price, status
                FROM orders
                WHERE order_id = @order_id;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@order_id", orderId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return ReadOrder(reader);

            return null;
        }

        public async Task<bool> Delete(int orderId)
        {
            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"DELETE FROM orders WHERE order_id = @order_id;";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@order_id", orderId);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<PagedResult<Order>> SearchByDate(DateTime from, DateTime to, PagingRequest? info = null)
        {
            info ??= new PagingRequest();
            var items = new List<Order>();

            await using var conn = CreateConnection();
            await conn.OpenAsync();

            var sql = @"
                SELECT order_id, create_time, final_price, status
                FROM orders
                WHERE create_time >= @from_date AND create_time < @to_date
                ORDER BY create_time DESC, order_id DESC
                LIMIT @limit OFFSET @offset;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@from_date", from);
            cmd.Parameters.AddWithValue("@to_date", to);
            cmd.Parameters.AddWithValue("@limit", info.PageSize);
            cmd.Parameters.AddWithValue("@offset", (info.PageNumber - 1) * info.PageSize);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(ReadOrder(reader));
            }

            var totalItems = await CountSearchAsync(conn, from, to);

            return new PagedResult<Order>
            {
                Item = items,
                Pagination = new PagingMetadata
                {
                    TotalItems = totalItems,
                    PageSize = info.PageSize,
                    PageNumber = info.PageNumber
                }
            };
        }

        private static Order ReadOrder(NpgsqlDataReader reader)
        {
            var statusText = reader.GetString(3).ToLower();

            return new Order
            {
                OrderId = reader.GetInt32(0),
                CreateTime = reader.GetDateTime(1),
                FinalPrice = reader.GetDecimal(2),
                Status = statusText switch
                {
                    "paid" => OrderStatus.Paid,
                    "cancelled" => OrderStatus.Cancelled,
                    _ => OrderStatus.New
                }
            };
        }

        private async Task<int> CountAllAsync(NpgsqlConnection conn)
        {
            var sql = @"SELECT COUNT(*) FROM orders;";
            await using var cmd = new NpgsqlCommand(sql, conn);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        private async Task<int> CountSearchAsync(NpgsqlConnection conn, DateTime from, DateTime to)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM orders
                WHERE create_time >= @from_date AND create_time < @to_date;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@from_date", from);
            cmd.Parameters.AddWithValue("@to_date", to);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
    }
}

