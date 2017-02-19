using System;

namespace Repositories.Interfaces
{
    /// <summary>
    /// Interface IUnitOfWork represents unit of work component following <a href="http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application">Repository and Unit of Work pattern</a>.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository by passing type of poco entity.
        /// </summary>
        /// <typeparam name="TPocoEntity">The type of the poco entity.</typeparam>
        /// <returns>A repository as <see cref="Repositories.Interfaces.IRepository{TPocoEntity}"/> interface.</returns>
        IRepository<TPocoEntity> GetRepository<TPocoEntity>() where TPocoEntity : class;
        /// <summary>
        /// Save all changes by using repository to database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        int Execute();
    }
}
