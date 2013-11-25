using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace GenericsCovarianceContravariance
{
    public class EmployeeDb : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }


    /// <summary>
    /// T is covariant here. This means that we can retrieve (OUT) a base class type if the repository is made up of child class types
    /// 
    /// as in if this is a IReadOnlyRepository of Employee then I can cast this to IReadOnlyRepository of Person
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyRepository<out T> : IDisposable
    {
        T Find(int id);
        IQueryable<T> FindAll();
    }

    /// <summary>
    /// T is contrvariant here. This means that we can add (IN) a child class type to a repository of parent class types
    /// 
    /// as we can add Manager types to a repository of Employee objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWriteOnlyRepository<in T> : IDisposable
    {
        void Add(T newEntity);
        void Delete(T entity);
        int Commit();
    }

    /// <summary>
    /// Though IReadOnlyRepository is covariant and IWriteOnlyRepository is contravariant
    /// IRepository is not variant. If IRepository is made up of Employee object then you
    /// can only work with Employee objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IWriteOnlyRepository<T>, IReadOnlyRepository<T>
    {
    }

    public class SqlRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet; 

        public SqlRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Add(T newEntity)
        {
            if (newEntity.IsValid())
            {
                _dbSet.Add(newEntity);
            }
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> FindAll()
        {
            return _dbSet;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}
