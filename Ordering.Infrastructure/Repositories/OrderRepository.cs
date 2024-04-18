﻿using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {        
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
        }
        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            var orderList = await dbContext.Orders
                .Where(order => order.UserName == userName)
                .ToListAsync();

            return orderList;
        }
    }
}
