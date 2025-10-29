using Npgsql;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class ProductService
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var list = new List<Product>();
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "SELECT id, name, price, stock, created_at FROM product ORDER BY id",
                conn
            );
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(
                    new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Stock = reader.GetInt32(3),
                        CreatedAt = reader.GetDateTime(4),
                    }
                );
            }
            return list;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "SELECT id, name, price, stock, created_at FROM product WHERE id = @id",
                conn
            );
            cmd.Parameters.AddWithValue("id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Stock = reader.GetInt32(3),
                    CreatedAt = reader.GetDateTime(4),
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Product product)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "INSERT INTO product (name, price, stock, created_at) VALUES (@name, @price, @stock, NOW()) RETURNING id",
                conn
            );
            cmd.Parameters.AddWithValue("name", product.Name);
            cmd.Parameters.AddWithValue("price", product.Price);
            cmd.Parameters.AddWithValue("stock", product.Stock);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<int> UpdateAsync(int id, Product product)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "UPDATE product SET name=@name, price=@price, stock=@stock WHERE id=@id",
                conn
            );
            cmd.Parameters.AddWithValue("name", product.Name);
            cmd.Parameters.AddWithValue("price", product.Price);
            cmd.Parameters.AddWithValue("stock", product.Stock);
            cmd.Parameters.AddWithValue("id", id);

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("DELETE FROM product WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("id", id);

            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
