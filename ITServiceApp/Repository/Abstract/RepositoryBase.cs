using ITServiceApp.Data;
using ITServiceApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ITServiceApp.Repository.Abstract
{
    public class RepositoryBase<T, TId> : IRepository<T, TId> where T : BaseEntity, new()
    {
        protected MyContext _context;
        private readonly DbSet<T> Table;
        public RepositoryBase(MyContext context)
        {
            _context = context;
            Table=_context.Set<T>();
        }
        public virtual void Add(T entity, bool isSaveLater = false)
        {
            Table.Add(entity);
            if (!isSaveLater)
                 this.Save();
        }

        public virtual void Delete(T entity, bool isSaveLater = false)
        {
            Table.Remove(entity);
            if (!isSaveLater)
                this.Save();
        }

        public virtual IQueryable<T> Get(Func<T, bool> predicate = null)
        {
           return predicate == null ? Table : Table.Where(predicate).AsQueryable();
        }

        public virtual IQueryable<T> Get(string[] includes, Func<T, bool> predicate = null)
        {
            IQueryable<T> query = Table;
            foreach (var include in includes)
            {
                query=Table.Include (include);
            }
            return predicate == null ? query : query.Where(predicate).AsQueryable();

        }

        public virtual T GetById(TId id)
        {
            return Table.Find(id);
        }

        public virtual int Save()
        {
            return _context.SaveChanges();
        }

        public virtual void Update(T entity, bool isSaveLater = false)
        {
            Table.Update(entity);
            if (!isSaveLater)
                this.Save();
        }

        public IQueryable<T> GET(string[] includes, Func<T, bool> predicate = null)
        {
            throw new NotImplementedException();
        }
    }
}
