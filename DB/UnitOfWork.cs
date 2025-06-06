using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Work_with_db
{
    public class UnitOfWork : IUnitOfWork
    {
        public ClothingStoreDbContext context;

        public IUserRepository Users { get; private set; }
        public IBrandRepository Brands { get; private set; }
        public IClothingItemRepository ClothingItems { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IOrderItemRepository OrderItems { get; private set; }
        public ISizeRepository Sizes { get; private set; }

        public UnitOfWork(ClothingStoreDbContext context)
        {
            this.context = context;

            Users = new UserRepository(context);
            Brands = new BrandRepository(context);
            ClothingItems = new ClothingItemRepository(context);
            Orders = new OrderRepository(context);
            OrderItems = new OrderItemRepository(context);
            Sizes = new SizeRepository(context);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
