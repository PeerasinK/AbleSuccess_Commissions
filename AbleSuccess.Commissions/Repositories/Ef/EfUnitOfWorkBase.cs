using Repositories.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;

namespace Repositories
{
    /// <summary>
    /// Abstract class EfUnitOfWorkBase implementing <see cref="Bndb.Kyc.Common.Repositories.Interfaces.IUnitOfWork"/> interface
    /// is intended for any unit of work component that use
    /// <a href="https://msdn.microsoft.com/en-us/library/vstudio/bb399567(v=vs.100).aspx" target="blank">MS Entity Framwork</a>
    /// following <a href="http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application">Repository and Unit of Work pattern</a> for providing necessary methods of
    /// all entity framework unit of work components.
    /// This component has to use with <see cref="Bndb.Kyc.Common.Repositories.EfRepository&lt;TPocoEntity&gt;"/> class.
    /// </summary>
    /// <example>
    /// The following example demonstrates how to use the
    /// <see cref="Bndb.Kyc.Common.Repositories.EfUnitOfWorkBase"/> abstract class to create a unit of work class.
    /// 
    /// <code>
    /// using System.Data.Entity;
    /// using Bndb.Kyc.Common.Repositories;
    /// 
    /// // Creates unit of work class extended from EfUnitOfWorkBase abstract class by using
    /// // database context representing database that inherited from <see cref="System.Data.Entity.Dbcontext"/> class.
    /// public class SampleUnitOfWork : EfUnitOfWorkBase
    /// {
    ///     // Creates constructor then pass database context, entity framework data provider and checkDbExists,
    ///     // we use sql server entity framework data provider (EntityFramework.SqlServer.dll) in this example.
    ///     public SampleUnitOfWork(string connectionString, bool checkDbExists = true)
    ///         : base(new SampleContext(new SqlConnection(connectionString)),
    ///                SqlProviderServices.Instance,
    ///                checkDbExists)
    ///     { }
    /// }
    /// </code>
    /// </example>
    public abstract class EfUnitOfWorkBase : IUnitOfWork
    {
        #region [Fields]

        private bool _disposed = false;
        /// <summary>
        /// The database context that retrieved from entity framework for using in inherited class to modify.
        /// </summary>
        protected DbContext _context;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWorkBase"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="databaseProvider">The database provider that inherited from
        /// <see cref="System.Data.Entity.Core.Common.DbProviderServices"/> abstract class.</param>
        /// <param name="checkDbExists">if set to <c>true</c> check database exists, it will throw exception if not exists.</param>
        /// <exception cref="System.ArgumentNullException">When context parameter is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// When database does not exist. This exception will occur when set checkDbExists to true.
        /// </exception>
        public EfUnitOfWorkBase(DbContext context, DbProviderServices databaseProvider, bool checkDbExists = true)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (checkDbExists && !context.Database.Exists())
            {
                throw new ArgumentException("The database does not exist. Please check your connection string.", "context");
            }

            _context = context;
            // Sets to false for high performance but has to use with ChangeTracker.DetectChanges method to detect later when you want.
            _context.Configuration.AutoDetectChangesEnabled = false;
        }

        #endregion


        #region [Methods]

        /// <summary>
        /// Gets a repository by passing type of poco entity.
        /// </summary>
        /// <typeparam name="TPocoEntity">The type of the poco entity.</typeparam>
        /// <returns>A repository as <see cref="Bndb.Kyc.Common.Repositories.Interfaces.IRepository&lt;TPocoEntity&gt;"/> interface.</returns>
        /// <exception cref="System.ObjectDisposedException">When object has been disposed.</exception>
        public IRepository<TPocoEntity> GetRepository<TPocoEntity>()
            where TPocoEntity : class
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return new EfRepository<TPocoEntity>(_context);
        }

        /// <summary>
        /// Save all changes by using repository to database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <exception cref="System.ObjectDisposedException">When object has been disposed.</exception>
        public virtual int Execute()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            _context.ChangeTracker.DetectChanges();
            return _context.SaveChanges();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _context = null;
            }

            _disposed = true;
        }

        #endregion
    }
}
