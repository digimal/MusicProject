using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MvcProject.Dal;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MvcProject.Bll.Services.Concrete
{
    public abstract class BaseService<T> where T : class, new()
    {
        protected readonly DbSet<T> dbSet;
        protected readonly MusicContext dbContext;
        protected readonly IMapper mapper;

        public BaseService(MusicContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
            this.mapper = mapper;
        }

        protected bool AlreadyExists(Predicate<T> predicate)
        {
            return dbSet.Any(predicate.Invoke);
        }

        protected void Delete(T entity) 
        {
            if(entity != null)
            {
                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        protected void DeleteOneIf(Expression<Func<T, bool>> predicate)
        {
            T entity = dbSet.FirstOrDefault(predicate);
            if (entity != null)
            {
                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        protected void DeleteManyIf(Expression<Func<T, bool>> predicate)
        {
            var entities = dbSet.Where(predicate);
            foreach(var entity in entities)
            {
                dbSet.Remove(entity);
            }
            dbContext.SaveChanges();
        }

        protected T Create(T entity)
        {
            var result = dbSet.Add(entity);
            dbContext.SaveChanges();
            return result.Entity;
        }

        protected void Update(T entity)
        {
            dbSet.Update(entity);
            dbContext.SaveChanges();
        }

        protected virtual T GetOneIf(Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefault(predicate);
        }

        protected virtual T GetOneIf(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> options)
        {
            return options(dbSet).FirstOrDefault(predicate);
        }

        protected virtual Task<T> GetOneIfAsync(Expression<Func<T, bool>> predicate)
        {
            return dbSet.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetMany()
        {
            return dbSet;
        }

        protected IQueryable<T> GetManyIf(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        protected Task DeleteAsyncOneIf(Expression<Func<T, bool>> predicate)
        {
            T entity = dbSet.FirstOrDefault(predicate);
            if (entity != null)
            {
                dbSet.Remove(entity);
                return dbContext.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }
    }


    public abstract class BaseCachedService<T> : BaseService<T> where T : class, new()
    {
        static T _lastEntity;

        public BaseCachedService(MusicContext dbContext, IMapper mapper): base(dbContext, mapper) { }

        protected override T GetOneIf(Expression<Func<T, bool>> predicate)
        {
            if (!CheckIfStores(predicate))
            {
                var entity = base.GetOneIf(predicate);
                _lastEntity = entity ?? _lastEntity;
                return entity;
            }
            return _lastEntity;
        }

        private bool CheckIfStores(Expression<Func<T, bool>> predicate)
        {
            return _lastEntity != null ? predicate.Compile()(_lastEntity) : false;
        }

    }
}
