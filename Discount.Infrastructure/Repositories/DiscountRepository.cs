using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;

        public DiscountRepository(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection
                .QueryFirstOrDefaultAsync<Coupon>(
                    "SELECT * FROM Coupon WHERE ProductName = @ProductName", 
                    new { ProductName = productName });

            if (coupon == null)
                return new Coupon { ProductName = "No discount", Amount = 0, Description = "No discount available" };
            
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var rowsAffected = await connection.ExecuteAsync(
                    "INSERT INTO Coupon (ProductName, Description, Amount) VALUES(@ProductName,@Description,@Amount)",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (rowsAffected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var rowsAffected = await connection.ExecuteAsync(
                    "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount=@Amount WHERE Id = @Id)",
                    new {Id = coupon.Id, ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (rowsAffected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var rowsAffected = await connection.ExecuteAsync(
                    "DELETE FROM Coupon WHERE ProductName = @ProductName)",
                    new { ProductName = productName });

            if (rowsAffected == 0)
                return false;

            return true;
        }
    }
}
