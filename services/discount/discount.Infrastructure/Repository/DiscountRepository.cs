using Dapper;
using discount.Core.Entities;
using discount.Core.Repository;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discount.Infrastructure.Repository
{
    public class DiscountRepository : ICouponRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where ProductName = @productName",
                new
                {
                    productName = productName,
                });
            if (coupon == null)
            {
                return new Coupon { Amount = 0 , Description = "No Discount Available for this product" , ProductName = "NoDiscount" };
            }
            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("insert into Coupon (ProductName,Description,Amount) values (@ProductName,@Description,@Amount)",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount,
                });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("update Coupon set ProductName=@ProductName ,Description=@Description, Amount = @Amount Where Id = @Id ",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount,
                    Id = coupon.Id,
                });
            if(affected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("delete from Coupon  Where ProductName=@ProductName ",
                new
                {
                    ProductName = productName,
                    
                });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }


    }
}
