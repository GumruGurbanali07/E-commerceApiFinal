using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using TContext context = new();
            var addEntity=context.Entry(entity);
            addEntity.State= EntityState.Added;
            context.SaveChanges();
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            //Butun kolleksiyani getirir
            var allItems = GetAll();

            // ve methodu isleden bir data varmi deye yoxlayir
            return allItems.Any(expression.Compile());
        }

        public void Delete(TEntity entity)
        {
            using TContext context = new();
            var deleteEntity = context.Entry(entity);
            deleteEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            using TContext context = new();
            return context.Set<TEntity>().SingleOrDefault(expression);   
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null)
        {
            using TContext context = new();
            return expression==null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(expression).ToList();
        }

        public void Update(TEntity entity)
        {
            using TContext context = new();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
