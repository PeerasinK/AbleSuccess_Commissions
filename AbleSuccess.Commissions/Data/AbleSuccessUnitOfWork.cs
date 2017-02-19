using Repositories;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;

namespace AbleSuccess.Commissions.Data
{
    public class AbleSuccessUnitOfWork : EfUnitOfWorkBase
    {
        public AbleSuccessUnitOfWork(string connectionString, bool checkDbExists = true)
            : base(new AbleSuccessDbContext(new SqlConnection(connectionString)),
                   SqlProviderServices.Instance,
                   checkDbExists)
        { }
    }
}
