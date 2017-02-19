namespace AbleSuccess.Commissions.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Txn_Commission
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommissionId { get; set; }

        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PoDetailId { get; set; }

        [Column(Order = 2)]
        public decimal TotalCommission { get; set; }

        public decimal? CommissionSales { get; set; }

        public decimal? CommissionPM { get; set; }

        public decimal? CommissionApp { get; set; }

        public decimal? CommissionInstall { get; set; }

        public decimal? CommissionAdmin { get; set; }

        public decimal? CommissionOutside { get; set; }

        public decimal? ConclusionCommission { get; set; }

        public decimal? AuditCommission { get; set; }
    }
}
