using AbleSuccess.Commissions.Data;
using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AbleSuccess.Commissions.WebUi.Manager
{
    public class CommissionManager
    {
        private readonly IUnitOfWork _unitOfWork;

        #region [CONSTRUCTOR]

        public CommissionManager()
        {
            _unitOfWork = new AbleSuccessUnitOfWork(Helper.ConnectionString);
        }

        #endregion

        #region [PUBLIC METHODS]

        public CommissionCollectionViewModel GetCommissionList(CommissionCollectionViewModel model = null, int userId = -1)
        {
            if (model == null) model = new CommissionCollectionViewModel();

            // Get Commissions
            var query = (from po in _unitOfWork.GetRepository<Txn_PO>().GetQueryable(o => o.Status == 1 && model.SearchDateFrom <= o.PoDate && o.PoDate <= model.SearchDateTo)
                         join pod in _unitOfWork.GetRepository<Txn_PODetail>().GetQueryable(o => o.Status == 1) on po.PoId equals pod.PoId
                         join cd in _unitOfWork.GetRepository<Txn_CommissionDetail>().GetQueryable(o => o.Amount > 0) on pod.PoDetailId equals cd.PoDetailId
                         join p in _unitOfWork.GetRepository<Mst_Profile>().GetQueryable() on cd.ProfileId equals p.ProfileId
                         orderby po.PoDate descending

                         select new CommissionViewModel
                         {
                             UserId = p.UserId,
                             Role = cd.Position,
                             PoDetail = new PoDetailModel
                             {
                                 PoId = po.PoId,
                                 PoNumber = po.PoNumber,
                                 PoDate = po.PoDate,
                                 //PoFilePath = p.PoFilePath,
                                 //InvoiceFilePath = p.InvoiceFilePath,
                                 CustomerName = po.CustomerName,
                                 PoDetailId = pod.PoDetailId,
                                 ProductName = pod.ProductName,
                                 ProductBrand = pod.ProductBrand,
                                 ProductType = pod.ProductType,
                                 //PricePerUnit = pd.PricePerUnit,
                                 //ActualPricePerUnit = pd.ActualPricePerUnit,
                                 //Amount = pd.Amount,
                                 TotalPrice = pod.TotalPrice,
                                 //ActualTotalPrice = pd.ActualTotalPrice,
                                 //Tax = pd.Tax,
                                 //TransportLocation = pd.TransportLocation,
                                 //TransportFee = pd.TransportFee,
                                 //ParcelFee = pd.ParcelFee,
                                 //ServiceFee = pd.ServiceFee,
                                 //OtherFee = pd.OtherFee,
                                 //CustomerLead = pd.CustomerLead,
                                 //CustomerLeadPercentage = pd.CustomerLeadPercentage,
                                 //TotalCost = pd.TotalCost,
                                 //Profit = pd.Profit,
                                 //ProfitPercentage = pd.ProfitPercentage,
                                 //Remark = pd.Remark,
                                 //Status = pd.Status,
                             },
                             CommissionDetail = new CommissionDetailModel
                             {
                                 CommissionDetailId = cd.CommissionDetailId,
                                 PoDetailId = cd.PoDetailId,
                                 ProfileId = cd.ProfileId,
                                 Position = cd.Position,
                                 Rate = cd.Rate,
                                 Amount = cd.Amount,
                                 WdTax = 0,
                                 Total = 0,
                                 Tax = 0,
                                 NetTotal = 0
                             }
                         });

            // Set Where cause
            // -- UserId
            if (userId > 0) query = query.Where(o => o.UserId == userId);
            else if (model.SearchProfileId > 0) query = query.Where(o => o.CommissionDetail.ProfileId == model.SearchProfileId);
            // -- RoleId
            if (model.SearchRoleId >= 0) query = query.Where(o => o.Role == model.SearchRoleId);

            model.CommissionViewCollection = query.ToList();

            // Mapping Id with lookup name
            for (int i = 0; i < model.CommissionViewCollection.Count; i++)
            {
                CommissionViewModel commission = model.CommissionViewCollection[i];

                commission.RoleName = Helper.GetNameFromLookup(commission.Role, Helper.LookupPosition);

                model.CommissionViewCollection[i] = commission;
            }

            // Get Commission Detail for find the sale
            var listCommissionDetail = (from cd in _unitOfWork.GetRepository<Txn_CommissionDetail>().GetQueryable(o => o.Position == 1)
                                        join p in _unitOfWork.GetRepository<Mst_Profile>().GetQueryable() on cd.ProfileId equals p.ProfileId
                                        select new 
                                        {
                                            cd.PoDetailId, p.FirstName, p.LastName
                                        }).ToList();

            // Calculate tax and sum tootal
            decimal sumAllTotalPrice = decimal.Zero;
            decimal sumAllCommission = decimal.Zero;
            decimal sumAllWdTax = decimal.Zero;
            decimal sumAllTotal = decimal.Zero;
            decimal sumAllTax = decimal.Zero;
            decimal sumAllNetTotal = decimal.Zero;

            List<LookupModel> lookupProductType = Helper.LookupProductType;
            decimal tax = 0.03m;
            foreach (CommissionViewModel commission in model.CommissionViewCollection)
            {
                string productTypeValue = lookupProductType.FirstOrDefault(o => o.Key == commission.PoDetail.ProductType.ToString()).Value;
                string[] productTypeNameWithTax = productTypeValue.Split(new string[] { "__" }, StringSplitOptions.None);

                decimal taxPercentage = decimal.Parse(productTypeNameWithTax[1].Replace("%", ""));
                decimal wdtaxRate = taxPercentage / 100;

                // Calculate Date
                commission.PoDetail.ProductTypeName = productTypeNameWithTax[0];
                commission.CommissionDetail.WdTax = commission.CommissionDetail.Amount * wdtaxRate;
                commission.CommissionDetail.Total = commission.CommissionDetail.Amount - commission.CommissionDetail.WdTax;
                commission.CommissionDetail.Tax = commission.CommissionDetail.Total * tax;
                commission.CommissionDetail.NetTotal = commission.CommissionDetail.Total - commission.CommissionDetail.Tax;

                // Sum Data
                sumAllTotalPrice += commission.PoDetail.TotalPrice;
                sumAllCommission += commission.CommissionDetail.Amount;
                sumAllWdTax += commission.CommissionDetail.WdTax;
                sumAllTotal += commission.CommissionDetail.Total;
                sumAllTax += commission.CommissionDetail.Tax;
                sumAllNetTotal += commission.CommissionDetail.NetTotal;

                // Set sales
                var temp = listCommissionDetail.FirstOrDefault(o => o.PoDetailId == commission.CommissionDetail.PoDetailId);
                commission.CommissionDetail.SalesName = temp.FirstName + " " + temp.LastName;
            }

            // Set Sum Data
            model.SumAllTotalPrice = sumAllTotalPrice;
            model.SumAllCommission = sumAllCommission;
            model.SumAllWdTax = sumAllWdTax;
            model.SumAllTotal = sumAllTotal;
            model.SumAllTax = sumAllTax;
            model.SumAllNetTotal = sumAllNetTotal;

            if (userId > 0 && model.CommissionViewCollection.Count > 0)
            {
                model.SearchProfileId = model.CommissionViewCollection[0].CommissionDetail.ProfileId;
            }

            return model;
        }

        public CommissionRateViewModel GetCommissionRate(bool isRateview = true)
        {
            CommissionRateViewModel model = new CommissionRateViewModel();

            // Get data
            var query = _unitOfWork.GetRepository<Mst_CommissionRate>().GetQueryable();

            // Mapping dto with model
            model.CommissionRateCollection = query.Select(o => new CommissionRateModel()
            {
                RateId = o.RateId,
                PositionId = o.PositionId,
                Percentage = o.Percentage,
                PercentageOf = o.PercentageOf,
                Year = o.Year
            }).OrderBy(o => o.PositionId).ToList();

            // Set Lookup
            model.LookupCommissionPercentageOf = Helper.LookupCommissionPercentageOf;
            //List<LookupModel> lookupPosition = Helper.LookupPosition;

            //// Mapping Id with lookup name
            //for (int i = 0; i < model.CommissionRateCollection.Count; i++)
            //{
            //    CommissionRateModel rate = model.CommissionRateCollection[i];

            //    rate.PositionName = Helper.GetNameFromLookup(rate.PositionId, lookupPosition);
            //    rate.PercentageOfDesciption = Helper.GetNameFromLookup(rate.PercentageOf, model.LookupCommissionPercentageOf);

            //    model.CommissionRateCollection[i] = rate;
            //}

            // Set commission rate by position
            model.SalesPercentage = model.CommissionRateCollection[0].Percentage;
            model.SalesPercentageOf = model.CommissionRateCollection[0].PercentageOf;

            model.PmPercentage = model.CommissionRateCollection[1].Percentage;
            model.PmPercentageOf = model.CommissionRateCollection[1].PercentageOf;

            model.AppSupportPercentage = model.CommissionRateCollection[2].Percentage;
            model.AppSupportPercentageOf = model.CommissionRateCollection[2].PercentageOf;

            model.InstallDeliveryPercentage = model.CommissionRateCollection[3].Percentage;
            model.InstallDeliveryPercentageOf = model.CommissionRateCollection[3].PercentageOf;

            model.AdminPercentage = model.CommissionRateCollection[4].Percentage;
            model.AdminPercentageOf = model.CommissionRateCollection[4].PercentageOf;

            model.OsPercentage = isRateview ? model.CommissionRateCollection[5].Percentage : 0;
            model.OsPercentageOf = model.CommissionRateCollection[5].PercentageOf;

            return model;
        }

        public void UpdateCommissionRate(CommissionRateViewModel model)
        {
            // Update CommissionRate
            IRepository<Mst_CommissionRate> repoCommissionRate = _unitOfWork.GetRepository<Mst_CommissionRate>();
            IEnumerable<Mst_CommissionRate> rateList = repoCommissionRate.GetQueryable();
            foreach (Mst_CommissionRate rate in rateList)
            {
                repoCommissionRate.Update(MappingModelToRate(rate, model));
            }

            _unitOfWork.Execute();
        }

        public List<LookupModel> GetCommissionRateYear()
        {
            // Get data
            var query = _unitOfWork.GetRepository<Mst_CommissionRate>().GetQueryable();

            // Return year
            return query.Select(o => new LookupModel()
            {
                Key = o.Year.ToString(),
                Value = o.Year.ToString()
            }).Distinct().OrderBy(o => o.Key).ToList();
        }

        #endregion

        #region [PRIVATE METHOD]

        private Mst_CommissionRate MappingModelToRate(Mst_CommissionRate rate, CommissionRateViewModel model)
        {
            switch (rate.PositionId)
            {
                // Sales
                case 1:
                    rate.Percentage = model.SalesPercentage;
                    rate.PercentageOf = model.SalesPercentageOf;
                    break;
                case 2:
                    rate.Percentage = model.PmPercentage;
                    rate.PercentageOf = model.PmPercentageOf;
                    break;
                case 3:
                    rate.Percentage = model.AppSupportPercentage;
                    rate.PercentageOf = model.AppSupportPercentageOf;
                    break;
                case 4:
                    rate.Percentage = model.InstallDeliveryPercentage;
                    rate.PercentageOf = model.InstallDeliveryPercentageOf;
                    break;
                case 5:
                    rate.Percentage = model.AdminPercentage;
                    rate.PercentageOf = model.AdminPercentageOf;
                    break;
                case 6:
                    rate.Percentage = model.OsPercentage;
                    rate.PercentageOf = model.OsPercentageOf;
                    break;
            }

            return rate;
        }

        #endregion
    }
}