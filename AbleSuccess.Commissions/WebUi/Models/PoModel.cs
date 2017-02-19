
using System;
using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class PoModel : BaseModel
    {
        public int PoId { get; set; }

        public string PoNumber { get; set; }

        public DateTime PoDate { get; set; }

        public string PoFilePath { get; set; }

        public string InvoiceFilePath { get; set; }

        public string CustomerName { get; set; }

    }

    public class PoDetailModel : PoModel
    {
        public int PoDetailId { get; set; }

        public string ProductName { get; set; }

        public int? ProductBrand { get; set; }

        public int? ProductType { get; set; }

        public string ProductTypeName { get; set; }

        public decimal PricePerUnit { get; set; }

        public decimal ActualPricePerUnit { get; set; }

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

    public class PoCollectionViewModel : BaseModel
    {
        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }

        public string SearchCustomer { get; set; }

        public List<PoViewModel> PoCollection { get; set; }

        public decimal SumAllTotalPrice { get; set; }

        public decimal SumAllTotalCost { get; set; }

        public decimal SumAllProfit { get; set; }

    }

    public class PoViewModel : PoModel
    {
        public List<PoDetailModel> PoDetailCollection { get; set; }

        public decimal SumTotalPrice { get; set; }

        public decimal SumTotalCost { get; set; }

        public decimal SumProfit { get; set; }
    }

    public class PoDetailViewModel : PoDetailModel
    {
        public bool IsOwner { get; set; }

        public decimal CommissionPayRate { get; set; }

        public decimal CommissionForSale { get; set; }

        public CommissionModel Commission { get; set; }

        public CommissionRateViewModel CommissionRate { get; set; }

        public int SalesProfileId1 { get; set; }

        public int SalesProfileId2 { get; set; }

        public int SalesProfileId3 { get; set; }

        public int PmProfileId1 { get; set; }

        public int PmProfileId2 { get; set; }

        public int PmProfileId3 { get; set; }

        public int AppSupportProfileId1 { get; set; }

        public int AppSupportProfileId2 { get; set; }

        public int AppSupportProfileId3 { get; set; }

        public int InstallDeliveryProfileId1 { get; set; }

        public int InstallDeliveryProfileId2 { get; set; }

        public int InstallDeliveryProfileId3 { get; set; }

        public int AdminProfileId1 { get; set; }

        public int AdminProfileId2 { get; set; }

        public int AdminProfileId3 { get; set; }

        public int OsProfileId1 { get; set; }

        public int OsProfileId2 { get; set; }

        public int OsProfileId3 { get; set; }

        public List<LookupModel> LookupEmployee { get; set; }

        public List<LookupModel> LookupPosition { get; set; }

        public List<LookupModel> LookupProductType { get; set; }

        public List<LookupModel> LookupProductBrand { get; set; }

        public List<LookupModel> LookupTransportLocation { get; set; }
    }

}