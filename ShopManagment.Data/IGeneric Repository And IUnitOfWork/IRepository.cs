using ShopManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Data.IGeneric_Repository_And_IUnitOfWork
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> GetByNameAsync(string name);

        Task AddAsync(T entity);

         void Update(T entity);

        Task DeleteAsync(int id);

        public Task<IQueryable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

         

        // Method to get entity by Id with includes
        public  Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);


        //
        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        
        

    }
}
