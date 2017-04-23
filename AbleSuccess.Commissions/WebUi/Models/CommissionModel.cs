
using AbleSuccess.Commissions.WebUi.Common;
using System;
using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class CommissionModel
    {
        public int CommissionId { get; set; }

        public int PoDetailId { get; set; }

        public decimal TotalCommission { get; set; }

        public decimal CommissionSales { get; set; }

        public decimal CommissionPM { get; set; }

        public decimal CommissionApp { get; set; }

        public decimal CommissionInstall { get; set; }

        public decimal CommissionAdmin { get; set; }

        public decimal CommissionOutside { get; set; }

        public decimal ConclusionCommission { get; set; }

        public decimal AuditCommission { get; set; }

    }

    public class CommissionDetailModel
    {
        public int CommissionDetailId { get; set; }

        public int PoDetailId { get; set; }

        public int ProfileId { get; set; }

        public int Position { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public decimal WdTax { get; set; }

        public decimal Total { get; set; }

        public decimal Tax { get; set; }

        public decimal NetTotal { get; set; }

        public string SalesName { get; set; }
    }

    public class CommissionViewModel
    {
        public int UserId { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }
        public PoDetailModel PoDetail { get; set; }

        public CommissionDetailModel CommissionDetail { get; set; }
    }

    public class CommissionCollectionViewModel : BaseModel
    {
        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }

        public int SearchProfileId { get; set; }

        public int SearchRoleId { get; set; }

        public List<CommissionViewModel> CommissionViewCollection { get; set; }

        public List<LookupModel> LookupEmployee { get; set; }

        public List<LookupModel> LookupPosition { get; set; }

        public decimal SumAllTotalPrice { get; set; }

        public decimal SumAllCommission { get; set; }

        public decimal SumAllWdTax { get; set; }

        public decimal SumAllTotal { get; set; }

        public decimal SumAllTax { get; set; }

        public decimal SumAllNetTotal { get; set; }

    }

    public class CommissionRateModel
    {
        public int RateId { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public decimal Percentage { get; set; }

        public int PercentageOf { get; set; }

        public string PercentageOfDesciption { get; set; }

        public int Year { get; set; }
    }

    public class CommissionRateViewModel : BaseModel
    {
        public List<LookupModel> LookupCommissionRateYear { get { return Helper.LookupCommissionRateYear; } }

        public string SearchYear { get; set; }
    }

    public class CommissionRateDetailViewModel : BaseModel
    {
        public List<CommissionRateModel> CommissionRateCollection { get; set; }

        public List<LookupModel> LookupCommissionPercentageOf { get; set; }

        public int Year { get; set; }

        public decimal SalesPercentage { get; set; }

        public int SalesPercentageOf { get; set; }

        public decimal PmPercentage { get; set; }

        public int PmPercentageOf { get; set; }

        public decimal AppSupportPercentage { get; set; }

        public int AppSupportPercentageOf { get; set; }

        public decimal InstallDeliveryPercentage { get; set; }

        public int InstallDeliveryPercentageOf { get; set; }

        public decimal AdminPercentage { get; set; }

        public int AdminPercentageOf { get; set; }

        public decimal OsPercentage { get; set; }

        public int OsPercentageOf { get; set; }

    }
}
