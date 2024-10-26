using Microsoft.EntityFrameworkCore;
using ShopManagment.Core.DTOs;
using ShopManagment.Core.Model;
using ShopManagment.Data.ContextClass;
using ShopManagment.Data.IGeneric_Repository_And_IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Data.Generic_Repository_And_UnitOfWork
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<T> _dbset;

        public Repository(ApplicationDbContext context)
        {
            _context = context;

            _dbset = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbset.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            T? t = await _dbset.FindAsync(id);
            return t!;
        }

        public async Task<T> GetByNameAsync(string name)
        {
            T? names = await _dbset.FindAsync(name);
            return names!;
        }

        public  void Update(T entity)
        {
            _dbset.Update(entity);
        }


        public async Task<IQueryable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes) 
        {
            IQueryable<T> query = _dbset;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await Task.FromResult(query);
        }

        // Get entity by id with includes
        public async Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbset;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        //
        public async Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbset.SingleOrDefaultAsync(predicate);
        }

       


    }
}
