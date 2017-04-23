using AbleSuccess.Commissions.Data;
using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AbleSuccess.Commissions.WebUi.Manager
{
    public class PoManager
    {
        private readonly IUnitOfWork _unitOfWork;

        #region [CONSTRUCTOR]

        public PoManager()
        {
            _unitOfWork = new AbleSuccessUnitOfWork(Helper.ConnectionString);
        }

        #endregion

        #region [PUBLIC METHODS]

        public PoCollectionViewModel GetPoList(PoCollectionViewModel model = null)
        {
            if (model == null) model = new PoCollectionViewModel();

            var queryDetail = _unitOfWork.GetRepository<Txn_PODetail>().GetQueryable(o => o.Status == 1);

            // Get data
            var query = (from p in _unitOfWork.GetRepository<Txn_PO>().GetQueryable(o => o.Status == 1 && model.SearchDateFrom <= o.PoDate && o.PoDate <= model.SearchDateTo)
                         orderby p.PoDate descending

                         select new PoViewModel
                         {
                             // Txn_PO
                             PoId = p.PoId,
                             PoNumber = p.PoNumber,
                             PoDate = p.PoDate,
                             //PoFilePath = p.PoFilePath,
                             //InvoiceFilePath = p.InvoiceFilePath,
                             CustomerName = p.CustomerName,

                             PoDetailCollection = (
                                 from pd in queryDetail
                                 where pd.PoId == p.PoId
                                 select new PoDetailModel
                                 {
                                     // Txn_PODetail
                                     PoDetailId = pd.PoDetailId,
                                     ProductName = pd.ProductName,
                                     //ProductBrand = pd.ProductBrand,
                                     //ProductType = pd.ProductType,
                                     //PricePerUnit = pd.PricePerUnit,
                                     //ActualPricePerUnit = pd.ActualPricePerUnit,
                                     //Amount = pd.Amount,
                                     TotalPrice = pd.TotalPrice,
                                     //ActualTotalPrice = pd.ActualTotalPrice,
                                     //Tax = pd.Tax,
                                     //TransportLocation = pd.TransportLocation,
                                     //TransportFee = pd.TransportFee,
                                     //ParcelFee = pd.ParcelFee,
                                     //ServiceFee = pd.ServiceFee,
                                     //OtherFee = pd.OtherFee,
                                     //CustomerLead = pd.CustomerLead,
                                     //CustomerLeadPercentage = pd.CustomerLeadPercentage,
                                     TotalCost = pd.TotalCost,
                                     Profit = pd.Profit,
                                     //ProfitPercentage = pd.ProfitPercentage,
                                     //Remark = pd.Remark,
                                     //Status = pd.Status,
                                 }).ToList()
                         });

            // Set Where cause
            if (!string.IsNullOrWhiteSpace(model.SearchCustomer))
                query = query.Where(o => o.CustomerName.StartsWith(model.SearchCustomer));

            model.PoCollection = query.ToList();

            // Sum data
            decimal sumAllTotalPrice = decimal.Zero;
            decimal sumAllTotalCost = decimal.Zero;
            decimal sumAllProfit = decimal.Zero;

            foreach (PoViewModel po in model.PoCollection)
            {
                decimal sumTotalPrice = decimal.Zero;
                decimal sumTotalCost = decimal.Zero;
                decimal sumProfit = decimal.Zero;

                foreach (PoDetailModel det in po.PoDetailCollection)
                {
                    sumTotalPrice += det.TotalPrice;
                    sumTotalCost += det.TotalCost;
                    sumProfit += det.Profit;
                }

                po.SumTotalPrice = sumTotalPrice;
                po.SumTotalCost = sumTotalCost;
                po.SumProfit = sumProfit;

                // Calculate all
                sumAllTotalPrice += sumTotalPrice;
                sumAllTotalCost += sumTotalCost;
                sumAllProfit += sumProfit;
            }

            model.SumAllTotalPrice = sumAllTotalPrice;
            model.SumAllTotalCost = sumAllTotalCost;
            model.SumAllProfit = sumAllProfit;

            return model;
        }

        public PoDetailViewModel GetPoDetail(int id)
        {
            PoDetailViewModel model;

            // Get Po data
            model = (from p in _unitOfWork.GetRepository<Txn_PO>().GetQueryable()
                     join pd in _unitOfWork.GetRepository<Txn_PODetail>().GetQueryable(o => o.PoDetailId == id) on p.PoId equals pd.PoId
                     select new PoDetailViewModel
                     {
                         // Txn_PO
                         PoId = p.PoId,
                         PoNumber = p.PoNumber,
                         PoDate = p.PoDate,
                         PoFilePath = p.PoFilePath,
                         InvoiceFilePath = p.InvoiceFilePath,
                         CustomerName = p.CustomerName,

                         // Txn_PODetail
                         PoDetailId = pd.PoDetailId,
                         ProductName = pd.ProductName,
                         ProductBrand = pd.ProductBrand,
                         ProductType = pd.ProductType,
                         PricePerUnit = pd.PricePerUnit,
                         ActualPricePerUnit = pd.ActualPricePerUnit,
                         Amount = pd.Amount,
                         TotalPrice = pd.TotalPrice,
                         ActualTotalPrice = pd.ActualTotalPrice,
                         Tax = pd.Tax,
                         TransportLocation = pd.TransportLocation,
                         TransportFee = pd.TransportFee,
                         ParcelFee = pd.ParcelFee,
                         ServiceFee = pd.ServiceFee,
                         OtherFee = pd.OtherFee,
                         CustomerLead = pd.CustomerLead,
                         CustomerLeadPercentage = pd.CustomerLeadPercentage,
                         TotalCost = pd.TotalCost,
                         Profit = pd.Profit,
                         ProfitPercentage = pd.ProfitPercentage,
                         Remark = pd.Remark,
                         Status = pd.Status,
                     }).FirstOrDefault();

            // Get commission
            model.Commission = (from c in _unitOfWork.GetRepository<Txn_Commission>().GetQueryable(o => o.PoDetailId == id)
                                select new CommissionModel
                                {
                                    // Txn_Commission
                                    CommissionId = c.CommissionId,
                                    PoDetailId = c.PoDetailId,
                                    TotalCommission = c.TotalCommission,
                                    CommissionSales = c.CommissionSales.Value,
                                    CommissionPM = c.CommissionPM.Value,
                                    CommissionApp = c.CommissionApp.Value,
                                    CommissionInstall = c.CommissionInstall.Value,
                                    CommissionAdmin = c.CommissionAdmin.Value,
                                    CommissionOutside = c.CommissionOutside.Value,
                                    ConclusionCommission = c.ConclusionCommission.Value,
                                    AuditCommission = c.AuditCommission.Value
                                }).FirstOrDefault();


            model = MappingCommissionDetailToPoDetailViewModel(_unitOfWork.GetRepository<Txn_CommissionDetail>().GetQueryable(o => o.PoDetailId == id).ToList(), model);

            return model;
        }

        public void CreatePo(PoDetailViewModel model)
        {
            // If Po number already exists then do not insert 
            Txn_PO existPo = _unitOfWork.GetRepository<Txn_PO>().GetQueryable(o => o.PoNumber == model.PoNumber).FirstOrDefault();
            if (existPo != null)
            {
                existPo.CustomerName = model.CustomerName;
                existPo.PoDate = model.PoDate;
                existPo.Status = 1;
                _unitOfWork.GetRepository<Txn_PO>().Update(existPo);

                model.PoId = existPo.PoId;
            }
            else
            {
                // Insert Po
                Txn_PO po = MappingPoDetailViewModelToPo(model);
                po.Status = 1;
                _unitOfWork.GetRepository<Txn_PO>().Insert(po);
                _unitOfWork.Execute();

                model.PoId = po.PoId;
            }

            // Insert PoDetail
            Txn_PODetail poDetail = MappingPoDetailViewModelToPoDetail(model);
            poDetail.Status = 1;
            _unitOfWork.GetRepository<Txn_PODetail>().Insert(poDetail);
            _unitOfWork.Execute();

            model.PoDetailId = poDetail.PoDetailId;

            // Insert Commission
            Txn_Commission commission = MappingPoDetailViewModelToCommission(model);
            _unitOfWork.GetRepository<Txn_Commission>().Insert(commission);

            // Insert CommissionDetail
            List<Txn_CommissionDetail> commissionDetail = MappingPoDetailViewModelToCommissionDetail(model);
            _unitOfWork.GetRepository<Txn_CommissionDetail>().Insert(commissionDetail);

            // Execute commission 
            _unitOfWork.Execute();
        }

        public void UpdatePo(PoDetailViewModel model)
        {
            // PO
            IRepository<Txn_PO> repoPo = _unitOfWork.GetRepository<Txn_PO>();
            Txn_PO po = repoPo.GetQueryable(o => o.PoId == model.PoId).FirstOrDefault();
            if (po != null)
            {
                po.PoNumber = model.PoNumber;
                po.PoDate = model.PoDate;
                po.PoFilePath = model.PoFilePath;
                po.InvoiceFilePath = model.InvoiceFilePath;
                po.CustomerName = model.CustomerName;
                repoPo.Update(po);
            }

            // PO Detail
            IRepository<Txn_PODetail> repoPoDetail = _unitOfWork.GetRepository<Txn_PODetail>();
            Txn_PODetail poDetail = repoPoDetail.GetQueryable(o => o.PoDetailId == model.PoDetailId).FirstOrDefault();
            if (poDetail != null)
            {
                //poDetail.PoId = model.PoId;
                poDetail.ProductName = model.ProductName;
                poDetail.ProductBrand = model.ProductBrand;
                poDetail.ProductType = model.ProductType;
                poDetail.PricePerUnit = model.PricePerUnit;
                poDetail.ActualPricePerUnit = model.ActualPricePerUnit;
                poDetail.Amount = model.Amount;
                poDetail.TotalPrice = model.TotalPrice;
                poDetail.ActualTotalPrice = model.ActualTotalPrice;
                poDetail.Tax = model.Tax;
                poDetail.TransportLocation = model.TransportLocation;
                poDetail.TransportFee = model.TransportFee;
                poDetail.ParcelFee = model.ParcelFee;
                poDetail.ServiceFee = model.ServiceFee;
                poDetail.OtherFee = model.OtherFee;
                poDetail.CustomerLead = model.CustomerLead;
                poDetail.CustomerLeadPercentage = model.CustomerLeadPercentage;
                poDetail.TotalCost = model.TotalCost;
                poDetail.Profit = model.Profit;
                poDetail.ProfitPercentage = model.ProfitPercentage;
                poDetail.Remark = model.Remark;
                repoPoDetail.Update(poDetail);
            }

            // Commission
            IRepository<Txn_Commission> repoCommission = _unitOfWork.GetRepository<Txn_Commission>();
            Txn_Commission commission = repoCommission.GetQueryable(o => o.PoDetailId == model.PoDetailId).FirstOrDefault();
            if (commission != null)
            {
                commission.TotalCommission = model.Commission.TotalCommission;
                commission.CommissionSales = model.Commission.CommissionSales;
                commission.CommissionPM = model.Commission.CommissionPM;
                commission.CommissionApp = model.Commission.CommissionApp;
                commission.CommissionInstall = model.Commission.CommissionInstall;
                commission.CommissionAdmin = model.Commission.CommissionAdmin;
                commission.CommissionOutside = model.Commission.CommissionOutside;
                commission.ConclusionCommission = model.Commission.ConclusionCommission;
                commission.AuditCommission = model.Commission.AuditCommission;
                repoCommission.Update(commission);
            }

            // CommissionDetail
            IRepository<Txn_CommissionDetail> repoCommissionDetail = _unitOfWork.GetRepository<Txn_CommissionDetail>();
            List<Txn_CommissionDetail> commissionDetailList = repoCommissionDetail.GetQueryable(o => o.PoDetailId == model.PoDetailId).ToList();
            foreach (Txn_CommissionDetail record in commissionDetailList)
            {
                repoCommissionDetail.Delete(record);
            }
            List<Txn_CommissionDetail> commissionDetail = MappingPoDetailViewModelToCommissionDetail(model);
            _unitOfWork.GetRepository<Txn_CommissionDetail>().Insert(commissionDetail);


            // Commit transaction
            _unitOfWork.Execute();
        }

        public void DeletePo(int id)
        {
            // Set status to remove Txn_PoDetail
            IRepository<Txn_PODetail> repoPoDetail = _unitOfWork.GetRepository<Txn_PODetail>();
            Txn_PODetail poDetail = repoPoDetail.GetQueryable(o => o.PoDetailId == id).FirstOrDefault();
            if (poDetail != null)
            {
                poDetail.Status = 0;
                repoPoDetail.Update(poDetail);
                _unitOfWork.Execute();
            }

            // Check If all detail that belong to PoDetail is status remove then remove Po
            int countStatus = repoPoDetail.GetQueryable(o => o.PoId == poDetail.PoId && o.Status == 1).Count();
            if (countStatus == 0)
            {
                IRepository<Txn_PO> repoPo = _unitOfWork.GetRepository<Txn_PO>();
                Txn_PO po = repoPo.GetQueryable(o => o.PoId == poDetail.PoId).FirstOrDefault();
                if (poDetail != null)
                {
                    po.Status = 0;
                    repoPo.Update(po);
                    _unitOfWork.Execute();
                }
            }
        }

        public PoDetailViewModel MappingCommissionDetail(PoDetailViewModel model)
        {
            UserManager userManager = new UserManager();
            List<UserDetailViewModel> userList = userManager.GetUserDetailList();

            int profileIdOfCurrentUser = userList.Where(o => o.UserId == int.Parse(Helper.UserId)).Select(o => o.ProfileId).FirstOrDefault();
            model.SalesProfileId1 = profileIdOfCurrentUser;

            foreach (UserDetailViewModel user in userList)
            {
                if (user.ProfileId != profileIdOfCurrentUser)
                {
                    // Sales
                    //if (user.Position == 1)
                    //{
                    //    if (model.SalesProfileId1 <= 0) model.SalesProfileId1 = user.ProfileId;
                    //    else if (model.SalesProfileId2 <= 0) model.SalesProfileId2 = user.ProfileId;
                    //    else if (model.SalesProfileId3 <= 0) model.SalesProfileId3 = user.ProfileId;
                    //}

                    // Product Manager
                    if (user.Position == 2)
                    {
                        if (model.PmProfileId1 <= 0) model.PmProfileId1 = user.ProfileId;
                        else if (model.PmProfileId2 <= 0) model.PmProfileId2 = user.ProfileId;
                        else if (model.PmProfileId3 <= 0) model.PmProfileId3 = user.ProfileId;
                    }

                    // Application/Support
                    if (user.Position == 3)
                    {
                        if (model.AppSupportProfileId1 <= 0) model.AppSupportProfileId1 = user.ProfileId;
                        else if (model.AppSupportProfileId2 <= 0) model.AppSupportProfileId2 = user.ProfileId;
                        else if (model.AppSupportProfileId3 <= 0) model.AppSupportProfileId3 = user.ProfileId;
                    }

                    // Installation/Delivery
                    if (user.Position == 4)
                    {
                        if (model.InstallDeliveryProfileId1 <= 0) model.InstallDeliveryProfileId1 = user.ProfileId;
                        else if (model.InstallDeliveryProfileId2 <= 0) model.InstallDeliveryProfileId2 = user.ProfileId;
                        else if (model.InstallDeliveryProfileId3 <= 0) model.InstallDeliveryProfileId3 = user.ProfileId;
                    }

                    // Admin
                    if (user.Position == 5)
                    {
                        if (model.AdminProfileId1 <= 0) model.AdminProfileId1 = user.ProfileId;
                        else if (model.AdminProfileId2 <= 0) model.AdminProfileId2 = user.ProfileId;
                        else if (model.AdminProfileId3 <= 0) model.AdminProfileId3 = user.ProfileId;
                    }

                    // Outsite Lead
                    if (user.Position == 6)
                    {
                        if (model.OsProfileId1 <= 0) model.OsProfileId1 = user.ProfileId;
                        else if (model.OsProfileId2 <= 0) model.OsProfileId2 = user.ProfileId;
                        else if (model.OsProfileId3 <= 0) model.OsProfileId3 = user.ProfileId;
                    }
                }
            }

            return model;
        }

        #endregion

        #region [PRIVATE METHOD]

        private Txn_PO MappingPoDetailViewModelToPo(PoDetailViewModel model)
        {
            return new Txn_PO
            {
                PoNumber = model.PoNumber,
                PoDate = model.PoDate,
                PoFilePath = model.PoFilePath,
                InvoiceFilePath = model.InvoiceFilePath,
                CustomerName = model.CustomerName,
                Status = model.Status,
                SalesProfileId = model.SalesProfileId1
            };
        }

        private Txn_PODetail MappingPoDetailViewModelToPoDetail(PoDetailViewModel model)
        {
            return new Txn_PODetail
            {
                PoId = model.PoId,
                ProductName = model.ProductName,
                ProductBrand = model.ProductBrand,
                ProductType = model.ProductType,
                PricePerUnit = model.PricePerUnit,
                ActualPricePerUnit = model.ActualPricePerUnit,
                Amount = model.Amount,
                TotalPrice = model.TotalPrice,
                ActualTotalPrice = model.ActualTotalPrice,
                Tax = model.Tax,
                TransportLocation = model.TransportLocation,
                TransportFee = model.TransportFee,
                ParcelFee = model.ParcelFee,
                ServiceFee = model.ServiceFee,
                OtherFee = model.OtherFee,
                CustomerLead = model.CustomerLead,
                CustomerLeadPercentage = model.CustomerLeadPercentage,
                TotalCost = model.TotalCost,
                Profit = model.Profit,
                ProfitPercentage = model.ProfitPercentage,
                Remark = model.Remark
            };
        }

        private Txn_Commission MappingPoDetailViewModelToCommission(PoDetailViewModel model)
        {
            return new Txn_Commission
            {
                PoDetailId = model.PoDetailId,
                TotalCommission = model.Commission.TotalCommission,
                CommissionSales = model.Commission.CommissionSales,
                CommissionPM = model.Commission.CommissionPM,
                CommissionApp = model.Commission.CommissionApp,
                CommissionInstall = model.Commission.CommissionInstall,
                CommissionAdmin = model.Commission.CommissionAdmin,
                CommissionOutside = model.Commission.CommissionOutside,
                ConclusionCommission = model.Commission.ConclusionCommission,
                AuditCommission = model.Commission.AuditCommission,
            };
        }

        private List<Txn_CommissionDetail> MappingPoDetailViewModelToCommissionDetail(PoDetailViewModel model)
        {
            List<Txn_CommissionDetail> listCommissionDetail = new List<Txn_CommissionDetail>();

            // +++++++++++++++++ Sales +++++++++++++++++ //
            if (model.CommissionRate.SalesPercentage > decimal.Zero)
            {
                List<Txn_CommissionDetail> listSales = new List<Txn_CommissionDetail>();
                int countSales = 0;
                if (model.SalesProfileId1 > 0)
                {
                    countSales++;
                    listSales.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 1, ProfileId = model.SalesProfileId1 });
                }
                if (model.SalesProfileId2 > 0)
                {
                    countSales++;
                    listSales.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 1, ProfileId = model.SalesProfileId2 });
                }
                if (model.SalesProfileId3 > 0)
                {
                    countSales++;
                    listSales.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 1, ProfileId = model.SalesProfileId3 });
                }

                // Set rate
                decimal rateSales = model.CommissionRate.SalesPercentage / countSales;
                decimal amountSales = model.Commission.CommissionSales / countSales;
                for (int i = 0; i < listSales.Count; i++)
                {
                    listSales[i].Rate = rateSales;
                    listSales[i].Amount = amountSales;
                }

                // Add to list
                listCommissionDetail.AddRange(listSales);
            }

            // +++++++++++++++++ Product Manager +++++++++++++++++ //
            if (model.CommissionRate.PmPercentage > decimal.Zero)
            {
                List<Txn_CommissionDetail> listPm = new List<Txn_CommissionDetail>();
                int countPm = 0;
                if (model.PmProfileId1 > 0)
                {
                    countPm++;
                    listPm.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 2, ProfileId = model.PmProfileId1 });
                }
                if (model.PmProfileId2 > 0)
                {
                    countPm++;
                    listPm.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 2, ProfileId = model.PmProfileId2 });
                }
                if (model.PmProfileId3 > 0)
                {
                    countPm++;
                    listPm.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 2, ProfileId = model.PmProfileId3 });
                }

                // Set rate
                decimal sharePm = model.CommissionRate.PmPercentage / countPm;
                decimal amountPm = model.Commission.CommissionPM / countPm;
                for (int i = 0; i < listPm.Count; i++)
                {
                    listPm[i].Rate = sharePm;
                    listPm[i].Amount = amountPm;
                }

                // Add to list
                listCommissionDetail.AddRange(listPm);
            }

            // +++++++++++++++++ Application/Support +++++++++++++++++ //
            if (model.CommissionRate.AppSupportPercentage > decimal.Zero)
            {
                List<Txn_CommissionDetail> listAppSupport = new List<Txn_CommissionDetail>();
                int countAppSupport = 0;
                if (model.AppSupportProfileId1 > 0)
                {
                    countAppSupport++;
                    listAppSupport.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 3, ProfileId = model.AppSupportProfileId1 });
                }
                if (model.AppSupportProfileId2 > 0)
                {
                    countAppSupport++;
                    listAppSupport.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 3, ProfileId = model.AppSupportProfileId2 });
                }
                if (model.AppSupportProfileId3 > 0)
                {
                    countAppSupport++;
                    listAppSupport.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 3, ProfileId = model.AppSupportProfileId3 });
                }

                // Set rate
                decimal shareAppSupport = model.CommissionRate.AppSupportPercentage / countAppSupport;
                decimal amountAppSupport = model.Commission.CommissionApp / countAppSupport;
                for (int i = 0; i < listAppSupport.Count; i++)
                {
                    listAppSupport[i].Rate = shareAppSupport;
                    listAppSupport[i].Amount = amountAppSupport;
                }

                // Add to list
                listCommissionDetail.AddRange(listAppSupport);
            }

            // +++++++++++++++++ Installation/Delivery +++++++++++++++++ //
            if (model.CommissionRate.InstallDeliveryPercentage > decimal.Zero)
            {
                List<Txn_CommissionDetail> listInstallDelivery = new List<Txn_CommissionDetail>();
                int countInstallDelivery = 0;
                if (model.InstallDeliveryProfileId1 > 0)
                {
                    countInstallDelivery++;
                    listInstallDelivery.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 4, ProfileId = model.InstallDeliveryProfileId1 });
                }
                if (model.InstallDeliveryProfileId2 > 0)
                {
                    countInstallDelivery++;
                    listInstallDelivery.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 4, ProfileId = model.InstallDeliveryProfileId2 });
                }
                if (model.InstallDeliveryProfileId3 > 0)
                {
                    countInstallDelivery++;
                    listInstallDelivery.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 4, ProfileId = model.InstallDeliveryProfileId3 });
                }

                // Set rate
                decimal shareInstallDelivery = model.CommissionRate.InstallDeliveryPercentage / countInstallDelivery;
                decimal amountInstallDelivery = model.Commission.CommissionInstall / countInstallDelivery;
                for (int i = 0; i < listInstallDelivery.Count; i++)
                {
                    listInstallDelivery[i].Rate = shareInstallDelivery;
                    listInstallDelivery[i].Amount = amountInstallDelivery;
                }

                // Add to list
                listCommissionDetail.AddRange(listInstallDelivery);
            }

            // +++++++++++++++++ Admin +++++++++++++++++ //
            if (model.CommissionRate.AdminPercentage > decimal.Zero)
            {
                List<Txn_CommissionDetail> listAdmin = new List<Txn_CommissionDetail>();
                int countAdmin = 0;
                if (model.AdminProfileId1 > 0)
                {
                    countAdmin++;
                    listAdmin.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 5, ProfileId = model.AdminProfileId1 });
                }
                if (model.AdminProfileId2 > 0)
                {
                    countAdmin++;
                    listAdmin.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 5, ProfileId = model.AdminProfileId2 });
                }
                if (model.AdminProfileId3 > 0)
                {
                    countAdmin++;
                    listAdmin.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 5, ProfileId = model.AdminProfileId3 });
                }

                // Set rate
                decimal shareAdmin = model.CommissionRate.AdminPercentage / countAdmin;
                decimal amountAdmin = model.Commission.CommissionAdmin / countAdmin;
                for (int i = 0; i < listAdmin.Count; i++)
                {
                    listAdmin[i].Rate = shareAdmin;
                    listAdmin[i].Amount = amountAdmin;
                }

                // Add to list
                listCommissionDetail.AddRange(listAdmin);
            }

            // +++++++++++++++++ Outsite +++++++++++++++++ //
            if (model.CommissionRate.OsPercentage > decimal.Zero)
            {
                List<Txn_CommissionDetail> listOs = new List<Txn_CommissionDetail>();
                int countOs = 0;
                if (model.OsProfileId1 > 0)
                {
                    countOs++;
                    listOs.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 6, ProfileId = model.OsProfileId1 });
                }
                if (model.OsProfileId2 > 0)
                {
                    countOs++;
                    listOs.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 6, ProfileId = model.OsProfileId2 });
                }
                if (model.OsProfileId3 > 0)
                {
                    countOs++;
                    listOs.Add(new Txn_CommissionDetail { PoDetailId = model.PoDetailId, Position = 6, ProfileId = model.OsProfileId3 });
                }

                // Set rate
                decimal shareOs = model.CommissionRate.OsPercentage / countOs;
                decimal amountOs = model.Commission.CommissionOutside / countOs;
                for (int i = 0; i < listOs.Count; i++)
                {
                    listOs[i].Rate = shareOs;
                    listOs[i].Amount = amountOs;
                }

                // Add to list
                listCommissionDetail.AddRange(listOs);
            }

            return listCommissionDetail;
        }

        private PoDetailViewModel MappingCommissionDetailToPoDetailViewModel(List<Txn_CommissionDetail> list, PoDetailViewModel model)
        {
            decimal rateSales, ratePm, rateAppSupport, rateInstallDelivery, rateAdmin, rateOs;
            rateSales = ratePm = rateAppSupport = rateInstallDelivery = rateAdmin = rateOs = decimal.Zero;

            foreach (Txn_CommissionDetail com in list)
            {
                // +++++++++++++++++ Sales +++++++++++++++++ //
                if (com.Position == 1)
                {
                    if (model.SalesProfileId1 <= 0)
                    {
                        rateSales += com.Rate;
                        model.SalesProfileId1 = com.ProfileId;
                    }
                    else if (model.SalesProfileId2 <= 0)
                    {
                        rateSales += com.Rate;
                        model.SalesProfileId2 = com.ProfileId;
                    }
                    else if (model.SalesProfileId3 <= 0)
                    {
                        rateSales += com.Rate;
                        model.SalesProfileId3 = com.ProfileId;
                    }
                }

                // +++++++++++++++++ Product Manager +++++++++++++++++ //
                else if (com.Position == 2)
                {
                    if (model.PmProfileId1 <= 0)
                    {
                        ratePm += com.Rate;
                        model.PmProfileId1 = com.ProfileId;
                    }
                    else if (model.PmProfileId2 <= 0)
                    {
                        ratePm += com.Rate;
                        model.PmProfileId2 = com.ProfileId;
                    }
                    else if (model.PmProfileId3 <= 0)
                    {
                        ratePm += com.Rate;
                        model.PmProfileId3 = com.ProfileId;
                    }
                }

                // +++++++++++++++++ Application/Support +++++++++++++++++ //
                else if (com.Position == 3)
                {
                    if (model.AppSupportProfileId1 <= 0)
                    {
                        rateAppSupport += com.Rate;
                        model.AppSupportProfileId1 = com.ProfileId;
                    }
                    else if (model.AppSupportProfileId2 <= 0)
                    {
                        rateAppSupport += com.Rate;
                        model.AppSupportProfileId2 = com.ProfileId;
                    }
                    else if (model.AppSupportProfileId3 <= 0)
                    {
                        rateAppSupport += com.Rate;
                        model.AppSupportProfileId3 = com.ProfileId;
                    }
                }

                // +++++++++++++++++ Installation/Delivery +++++++++++++++++ //
                else if (com.Position == 4)
                {
                    if (model.InstallDeliveryProfileId1 <= 0)
                    {
                        rateInstallDelivery += com.Rate;
                        model.InstallDeliveryProfileId1 = com.ProfileId;
                    }
                    else if (model.InstallDeliveryProfileId2 <= 0)
                    {
                        rateInstallDelivery += com.Rate;
                        model.InstallDeliveryProfileId2 = com.ProfileId;
                    }
                    else if (model.InstallDeliveryProfileId3 <= 0)
                    {
                        rateInstallDelivery += com.Rate;
                        model.InstallDeliveryProfileId3 = com.ProfileId;
                    }
                }

                // +++++++++++++++++ Admin +++++++++++++++++ //
                else if (com.Position == 5)
                {
                    if (model.AdminProfileId1 <= 0)
                    {
                        rateAdmin += com.Rate;
                        model.AdminProfileId1 = com.ProfileId;
                    }
                    else if (model.AdminProfileId2 <= 0)
                    {
                        rateAdmin += com.Rate;
                        model.AdminProfileId2 = com.ProfileId;
                    }
                    else if (model.AdminProfileId3 <= 0)
                    {
                        rateAdmin += com.Rate;
                        model.AdminProfileId3 = com.ProfileId;
                    }
                }

                // +++++++++++++++++ Outsite +++++++++++++++++ //
                else if (com.Position == 6)
                {
                    if (model.OsProfileId1 <= 0)
                    {
                        rateOs += com.Rate;
                        model.OsProfileId1 = com.ProfileId;
                    }
                    else if (model.OsProfileId2 <= 0)
                    {
                        rateOs += com.Rate;
                        model.OsProfileId2 = com.ProfileId;
                    }
                    else if (model.OsProfileId3 <= 0)
                    {
                        rateOs += com.Rate;
                        model.OsProfileId3 = com.ProfileId;
                    }
                }
            }

            // Add commission rate
            model.CommissionRate = new CommissionRateDetailViewModel();
            model.CommissionRate.SalesPercentage = rateSales;
            model.CommissionRate.PmPercentage = ratePm;
            model.CommissionRate.AppSupportPercentage = rateAppSupport;
            model.CommissionRate.InstallDeliveryPercentage = rateInstallDelivery;
            model.CommissionRate.AdminPercentage = rateAdmin;
            model.CommissionRate.OsPercentage = rateOs;

            return model;
        }

        #endregion
    }
}