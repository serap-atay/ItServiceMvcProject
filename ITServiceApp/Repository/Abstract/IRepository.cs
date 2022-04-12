using ITServiceApp.Models.Entities;
using System;
using System.Linq;

namespace ITServiceApp.Repository.Abstract
{
    public interface IRepository<T , in TId> where T : BaseEntity
    {
        T GetById(TId id);
        IQueryable<T> Get(Func<T, bool> predicate = null);

        IQueryable<T> Get(string [] includes ,Func<T, bool> predicate = null);

        void Add( T entity,bool isSaveLater=false);
        void Update( T entity,bool isSaveLater=false);
        void Delete( T entity,bool isSaveLater=false);

        int Save();
    }
}
