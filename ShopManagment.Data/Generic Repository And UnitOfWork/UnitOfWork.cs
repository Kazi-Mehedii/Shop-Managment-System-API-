using ShopManagment.Core.Model;
using ShopManagment.Data.ContextClass;
using ShopManagment.Data.IGeneric_Repository_And_IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Data.Generic_Repository_And_UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Product> Products => new Repository<Product>(_context);

        public IRepository<Supplier> Suppliers => new Repository<Supplier>(_context);

        public IRepository<Purchase> Purchases => new Repository<Purchase>(_context);

        public IRepository<Sale> Sales => new Repository<Sale>(_context);

        public IRepository<SaleItem> SaleItems => new Repository<SaleItem>(_context);

        public IRepository<Stock> Stocks => new Repository<Stock>(_context);

        public IRepository<User> Users => new Repository<User>(_context);


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        //Idisposible  Implementation

        protected virtual void Dispose(bool disposing) 
        {
            if (!_disposed) 
            {
                if (disposing) 
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
