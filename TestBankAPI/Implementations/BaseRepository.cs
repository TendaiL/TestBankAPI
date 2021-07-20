using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestBankAPI.Interfaces;
using TestBankAPI.Models;
using TestBankAPI.Models.Data;

namespace TestBankAPI.Implementations
{
   public abstract class BaseRepository<T> : IRepository<T>
       where T : class ,IEntity
    {
        private BankContext context;
        private readonly DbSet<T> entities;
        public BaseRepository(BankContext dbContext)
        {
            context = dbContext;
            entities = dbContext.Set<T>();
        }
      
        public virtual T Get(long id)
        {
            return entities.AsNoTracking().SingleOrDefault(s => s.Id == id);
        }

        public async Task <T> Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var update =  context.Add(entity);
            SaveChanges();
            return update.Entity;
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var update =  context.Update(entity);
            SaveChanges();
            return update.Entity;
        }

        public virtual void Delete(T entity)
        {
            SaveChanges();
        }

        public virtual void Delete(long id)
        {
            var original = entities.First(dbEntity => dbEntity.Id == id);
            Delete(original);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return entities.FirstOrDefault(predicate);//used for updates. must be tracked
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().AsQueryable().AsNoTracking().Where(predicate);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                AddToContext(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                Delete(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public bool Any<TSource>(Expression<Func<T, bool>> predicate)
        {
            return entities.Any(predicate);
        }

        public void AddToContext(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Add(entity);
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable().AsNoTracking();
        }
    }
}
