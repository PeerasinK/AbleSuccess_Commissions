using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// The Repositories namespace contains all relevant classes following <a href="http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application">Repository and Unit of Work pattern</a>.
/// </summary>
namespace Repositories
{
    /// <summary>
    /// Class EfRepository is a generic repository component implementing
    /// <see cref="Repositories.Interfaces.IRepository&lt;TPocoEntity&gt;"/> interface
    /// that use <a href="https://msdn.microsoft.com/en-us/library/vstudio/bb399567(v=vs.100).aspx" target="blank">MS Entity
    /// Framwork</a> following <a href="http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application">Repository and Unit of Work pattern</a>.
    /// This component has been designed for flexibility,
    /// you can override any method and inherit to do something more as well.
    /// </summary>
    /// <remarks>
    /// This component should use with unit of work component that extended from
    /// <see cref="Repositories.EfUnitOfWorkBase"/> abstract class following unit of work pattern.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use the
    /// <see cref="Repositories.EfRepository&lt;TPocoEntity&gt;"/> class
    /// to injecting poco entity for manipulating data in database.
    /// 
    /// <code>
    /// using System.Data.Entity;
    /// using Repositories;
    /// 
    /// class Test
    /// {
    ///     // This is a poco entity representing table in database.
    ///     class PocoEntity
    ///     {
    ///         public int Id { get; set; }
    ///         public string Name { get; set; }
    ///     }
    ///
    ///     static void Main(string[] args)
    ///     {
    ///         // Creates Entity Framework DbContext object for accessing database by passing connection string.
    ///         DbContext context = new DbContext("Database Connection String");
    ///
    ///         // Creates repository by injecting your poco entity that you want to manipulating.
    ///         EfRepository&lt;PocoEntity&gt; repository = new EfRepository&lt;PocoEntity&gt;(context);
    ///
    ///         // This is an example to get all PocoEntity data.
    ///         IEnumerable&lt;PocoEntity&gt; pocoEntities = repository.Get();
    ///
    ///         // This is an example to add a PocoEntity data to database.
    ///         repository.Insert(new PocoEntity { Id = 1, Name = "PocoEntity1" });
    ///
    ///         // Saves to database.
    ///         context.SaveChanges();
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <typeparam name="TPocoEntity">The type of the poco entity.</typeparam>
    public class EfRepository<TPocoEntity> : IRepository<TPocoEntity>
        where TPocoEntity : class
    {
        #region [Fields]

        /// <summary>
        /// The database context that retrieved from entity framework for using in inherited class to modify.
        /// </summary>
        protected DbContext _context;
        /// <summary>
        /// The DbSet that retrieved from entity framework for using in inherited class to modify.
        /// </summary>
        protected DbSet<TPocoEntity> _dbSet;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{TPocoEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EfRepository(DbContext context)
        {
            // Sets all required fields.
            _context = context;
            _dbSet = context.Set<TPocoEntity>();
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Gets poco entity representing data in database.
        /// </summary>
        /// <param name="filter">The filter data.</param>
        /// <param name="page">The pagination for retrieving scoped data.</param>
        /// <param name="orderBy">The ordering data.</param>
        /// <param name="includeProperties">The collection of property names of related poco entity for including data.</param>
        /// <returns>Collection of data as IEnumerable of poco entity.</returns>
        /// <exception cref="System.ArgumentNullException">When use page parameter, page.OrderBy property could not be null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// When use page parameter, must has at least 'Limit' or 'Offset' property.
        /// </exception>
        public virtual IEnumerable<TPocoEntity> Get(
            Expression<Func<TPocoEntity, bool>> filter = null,
            Page<TPocoEntity> page = null,
            Func<IQueryable<TPocoEntity>, IOrderedQueryable<TPocoEntity>> orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<TPocoEntity> result = GetQueryable(filter, page, orderBy, includeProperties);

            // Returns with checking ordering.
            return result.ToList();
        }

        /// <summary>
        /// Gets queryable of poco entity representing data in database.
        /// </summary>
        /// <param name="filter">The filter data.</param>
        /// <param name="page">The pagination for retrieving scoped data.</param>
        /// <param name="orderBy">The ordering data.</param>
        /// <param name="includeProperties">The collection of property names of related poco entity for including data.</param>
        /// <returns>IQueryable of poco entity.</returns>
        /// <exception cref="System.ArgumentNullException">When use page parameter, page.OrderBy property could not be null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// When use page parameter, must has at least 'Limit' or 'Offset' property.
        /// </exception>
        public virtual IQueryable<TPocoEntity> GetQueryable(
            Expression<Func<TPocoEntity, bool>> filter = null,
            Page<TPocoEntity> page = null,
            Func<IQueryable<TPocoEntity>, IOrderedQueryable<TPocoEntity>> orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<TPocoEntity> result = _dbSet;

            // For filter.
            if (filter != null)
            {
                result = result.Where(filter);
            }

            // For pagination.
            if (page != null)
            {
                // Checks all required parameters.
                if (page.OrderBy == null)
                {
                    throw new ArgumentNullException("page.OrderBy");
                }
                if (!page.Limit.HasValue && !page.Offset.HasValue)
                {
                    throw new InvalidOperationException("Must has at least 'Limit' or 'Offset'");
                }

                result = page.OrderBy(result);

                if (page.Offset.HasValue)
                {
                    result = result.Skip(page.Offset.Value);
                }

                if (page.Limit.HasValue)
                {
                    result = result.Take(page.Limit.Value);
                }
            }

            // For include properties.
            result = includeProperties.Aggregate(
                result,
                (current, includeProperty) => current.Include(includeProperty)
            );

            // Returns with checking ordering.
            return (orderBy != null) ? orderBy(result)
                                     : result;
        }

        /// <summary>
        /// Gets poco entity representing data in database by using primary key.
        /// </summary>
        /// <param name="ids">The primary key.</param>
        /// <returns>A data as poco entity. It will be null if could not found data.</returns>
        public virtual TPocoEntity GetById(params object[] ids)
        {
            return _dbSet.Find(ids);
        }

        /// <summary>
        /// Gets the amount of data with filter if any.
        /// </summary>
        /// <param name="filter">The filter data.</param>
        /// <param name="includeProperties">The collection of property names of related poco entity for including data.</param>
        /// <returns>Amount of data.</returns>
        public int GetCount(Expression<Func<TPocoEntity, bool>> filter = null, params string[] includeProperties)
        {
            IQueryable<TPocoEntity> result = _dbSet;

            // For filter.
            if (filter != null)
            {
                result = result.Where(filter);
            }

            // For include properties.
            result = includeProperties.Aggregate(
                result,
                (current, includeProperty) => current.Include(includeProperty)
            );

            return result.Count();
        }

        /// <summary>
        /// Inserts a data as poco entity to database.
        /// </summary>
        /// <param name="entityToInsert">The entity to insert.</param>
        public virtual void Insert(TPocoEntity entityToInsert)
        {
            DbEntityEntry entry = _context.Entry(entityToInsert);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                _dbSet.Add(entityToInsert);
            }
        }

        /// <summary>
        /// Inserts the multiple data as collection of poco entities to database.
        /// </summary>
        /// <param name="entitiesToInsert">The collection of entities to insert.</param>
        public virtual void Insert(IEnumerable<TPocoEntity> entitiesToInsert)
        {
            _dbSet.AddRange(entitiesToInsert);
        }

        /// <summary>
        /// Updates a data as poco entity in database.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        public virtual void Update(TPocoEntity entityToUpdate)
        {
            DbEntityEntry entry = _context.Entry(entityToUpdate);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entityToUpdate);
            }

            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes a data by using primary key.
        /// </summary>
        /// <param name="ids">The primary key.</param>
        public virtual void Delete(params object[] ids)
        {
            TPocoEntity entityToDelete = GetById(ids);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        /// <summary>
        /// Deletes a data as poco entity from database.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public virtual void Delete(TPocoEntity entityToDelete)
        {
            DbEntityEntry entry = _context.Entry(entityToDelete);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entityToDelete);
                _dbSet.Remove(entityToDelete);
            }
        }

        /// <summary>
        /// Deletes the multiple data as collection of poco entities from database.
        /// </summary>
        /// <param name="entitiesToDelete">The collection of entities to delete.</param>
        public virtual void Delete(IEnumerable<TPocoEntity> entitiesToDelete)
        {
            _dbSet.RemoveRange(entitiesToDelete);
        }

        #endregion
    }
}
