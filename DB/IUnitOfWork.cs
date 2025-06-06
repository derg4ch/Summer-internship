using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Repo;

namespace Work_with_db
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBrandRepository Brands { get; }
        IClothingItemRepository ClothingItems { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        ISizeRepository Sizes { get; }

        Task<int> SaveAsync();
    }
}
