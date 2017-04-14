namespace AbleSuccess.Commissions.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Txn_PO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PoId { get; set; }

        [Column(Order = 1)]
        [StringLength(50)]
        public string PoNumber { get; set; }

        [Column(Order = 2)]
        public DateTime PoDate { get; set; }

        public string PoFilePath { get; set; }

        public string InvoiceFilePath { get; set; }

        [Column(Order = 3)]
        [StringLength(255)]
        public string CustomerName { get; set; }

        [Column(Order = 4)]
        public int Status { get; set; }

        [Column(Order = 5)]
        public int? SalesProfileId { get; set; }
    }
}
