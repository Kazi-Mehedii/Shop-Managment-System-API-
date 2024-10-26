using ShopManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Data.IGeneric_Repository_And_IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }

        IRepository<Supplier> Suppliers { get; }

        IRepository<Purchase> Purchases { get; }

        IRepository<Sale> Sales { get; }

        IRepository<SaleItem> SaleItems { get; }

        IRepository<Stock> Stocks { get; }

        IRepository<User> Users { get; }

        Task SaveAsync();
    }
}
