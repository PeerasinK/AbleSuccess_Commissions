namespace AbleSuccess.Commissions.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Mst_CommissionRate
    {
        [Key]
        [Column(Order = 0)]
        public int RateId { get; set; }

        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PositionId { get; set; }

        [Column(Order = 2)]
        public decimal Percentage { get; set; }

        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PercentageOf { get; set; }
    }
}
