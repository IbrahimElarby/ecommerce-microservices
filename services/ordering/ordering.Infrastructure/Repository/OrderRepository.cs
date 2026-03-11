using Microsoft.EntityFrameworkCore;
using ordering.Core.Entities;
using ordering.Core.Repository;
using ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordering.Infrastructure.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string UserName)
        {
            return await _context.Orders.Where(o => o.UserName == UserName).ToListAsync();
        }
    }
}
