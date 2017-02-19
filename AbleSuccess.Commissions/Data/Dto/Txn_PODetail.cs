namespace AbleSuccess.Commissions.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Txn_PODetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PoDetailId { get; set; }

        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PoId { get; set; }

        [Column(Order = 2)]
        [StringLength(255)]
        public string ProductName { get; set; }

        public int? ProductBrand { get; set; }

        public int? ProductType { get; set; }

        [Column(Order = 3)]
        public decimal PricePerUnit { get; set; }

        [Column(Order = 4)]
        public decimal ActualPricePerUnit { get; set; }

        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Amount { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal ActualTotalPrice { get; set; }

        public decimal Tax { get; set; }

        public string TransportLocation { get; set; }

        public decimal TransportFee { get; set; }

        public decimal ParcelFee { get; set; }

        public decimal ServiceFee { get; set; }

        public decimal OtherFee { get; set; }

        public decimal CustomerLead { get; set; }

        public decimal CustomerLeadPercentage { get; set; }

        public decimal TotalCost { get; set; }

        public decimal Profit { get; set; }

        public decimal ProfitPercentage { get; set; }

        public string Remark { get; set; }

        public int Status { get; set; }
    }
}
