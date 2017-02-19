using System.Data.Entity;
using System.Data.Common;

namespace AbleSuccess.Commissions.Data
{
    /// <summary>
    /// Class AbleSuccessDbContext is an entity framework database context that inherited from
    /// <see cref="System.Data.Entity.DbContext"/> class.
    /// </summary>
    /// <remarks>
    /// This class is internal class you cannot use outside.
    /// </remarks>
    public partial class AbleSuccessDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbleSuccessDbContext"/> class.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        public AbleSuccessDbContext(DbConnection connection)
            : base(connection, true)
        { }

        public virtual DbSet<Mst_CommissionRate> Mst_CommissionRate { get; set; }
        public virtual DbSet<Mst_Credential> Mst_Credential { get; set; }
        public virtual DbSet<Mst_Profile> Mst_Profile { get; set; }
        public virtual DbSet<Txn_Commission> Txn_Commission { get; set; }
        public virtual DbSet<Txn_CommissionDetail> Txn_CommissionDetail { get; set; }
        public virtual DbSet<Txn_PO> Txn_PO { get; set; }
        public virtual DbSet<Txn_PODetail> Txn_PODetail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mst_CommissionRate>()
                .Property(e => e.Percentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Mst_Credential>()
                .Property(e => e.UserCode)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Credential>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Credential>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.NickName)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Mst_Profile>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<Txn_PO>()
                .Property(e => e.PoNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Txn_PO>()
                .Property(e => e.PoFilePath)
                .IsUnicode(false);

            modelBuilder.Entity<Txn_PO>()
                .Property(e => e.InvoiceFilePath)
                .IsUnicode(false);

            modelBuilder.Entity<Txn_PO>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<Txn_PODetail>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<Txn_PODetail>()
                .Property(e => e.ProfitPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Txn_PODetail>()
                .Property(e => e.Remark)
                .IsUnicode(false);
        }
    }
}
