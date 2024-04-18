using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {            
        }
        
        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach(var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "Shailendra";  //TODO: To be replaced by Identity Server
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "Shailendra";  //TODO: To be replaced by Identity Server
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
