using System;
using System.Linq;

/// <summary>
/// The Bndb.Kyc.Common.Repositories.Models namespace contains all about models that use in repository component.
/// </summary>
namespace Repositories.Models
{
    /// <summary>
    /// Class Page representing pagination for scoping data when retrieve data from database.
    /// </summary>
    /// <typeparam name="TPocoEntity">The type of the poco entity.</typeparam>
    public class Page<TPocoEntity>
    {
        /// <summary>
        /// Gets or sets the ordering data.
        /// </summary>
        /// <value>The ordering data.</value>
        public Func<IQueryable<TPocoEntity>, IOrderedQueryable<TPocoEntity>> OrderBy { get; set; }
        /// <summary>
        /// Gets or sets the starting record position.
        /// </summary>
        /// <value>The starting record position.</value>
        public int? Offset { get; set; }
        /// <summary>
        /// Gets or sets the amount of data you want to retrieve.
        /// </summary>
        /// <value>The amount of data you want to retrieve.</value>
        public int? Limit { get; set; }
    }
}
