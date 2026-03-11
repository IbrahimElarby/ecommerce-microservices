using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.Infrastructure.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var OptionBuilder = new DbContextOptionsBuilder<OrderContext>();
            OptionBuilder.UseSqlServer("Server=localhost,1433;Database=OrderDb;User Id=sa;Password=p@ssw0rd123;TrustServerCertificate=True");
            return new OrderContext(OptionBuilder.Options);
        }
    }
}
