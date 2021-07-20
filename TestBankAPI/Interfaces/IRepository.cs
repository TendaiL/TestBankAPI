using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestBankAPI.Models.Data;

namespace TestBankAPI.Interfaces
{
   public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T>GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T Get(long id);
        Task <T> Add(T entity);
        Task <T> Update(T entity);
        void Delete(T entity);
        void Delete(long id);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
