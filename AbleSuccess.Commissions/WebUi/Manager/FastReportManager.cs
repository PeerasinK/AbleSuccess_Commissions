using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace AbleSuccess.Commissions.WebUi.Manager
{
    public class FastReportManager
    {
        private EmployeeManager _employeeManager;
        public FastReportManager()
        {
            _employeeManager = new EmployeeManager();
        }
        public List<CommissionMasterReportModel> MapperCommission(CommissionCollectionViewModel commissionModel)
        {
            EmployeeDetailViewModel employeeDetailViewModel = _employeeManager.GetEmployeeDetail(commissionModel.SearchProfileId);
            List<CommissionMasterReportModel> commissionMasterReports = new List<CommissionMasterReportModel>();
            List<CommissionReportModel> commissionReportModels = new List<CommissionReportModel>();
            CommissionMasterReportModel commissionMasterReport = new CommissionMasterReportModel();
            commissionMasterReport.Fullname = employeeDetailViewModel.FirstName + " " + employeeDetailViewModel.LastName;
            commissionMasterReport.PositionName = "(" + employeeDetailViewModel.PositionName + ")";
            commissionMasterReport.SearchDateFrom = commissionModel.SearchDateFrom;
            commissionMasterReport.SearchDateTo = commissionModel.SearchDateTo;

            foreach (CommissionViewModel commodel in commissionModel.CommissionViewCollection)
            {
                commissionReportModels.Add(new CommissionReportModel
                {
                    Date = DateTime.Now,
                    PO = commodel.PoDetail.PoNumber,
                    ProductName = commodel.PoDetail.ProductName,

                    ProductTypeName = "(" + commodel.PoDetail.ProductTypeName + ")",
                    CustomerName = commodel.PoDetail.CustomerName,
                    SalesName = commodel.CommissionDetail.SalesName,

                    ComRole = Helper.GetNameFromLookup(commodel.Role, Helper.LookupPosition),
                    Revenue = Math.Round((Decimal)commodel.PoDetail.TotalPrice, 2),
                    Tax = Math.Round((Decimal)commodel.CommissionDetail.Tax, 2),
                    TotalCom = Math.Round((Decimal)commodel.CommissionDetail.Total, 2),
                    NetTotal = Math.Round((Decimal)commodel.CommissionDetail.NetTotal, 2),
                });
            }
            commissionMasterReport.CommissionsReport = commissionReportModels;

            commissionMasterReport.SumRevenue = Math.Round((Decimal)commissionModel.SumAllTotalPrice, 2);
            commissionMasterReport.SumCOM = Math.Round((Decimal)commissionModel.SumAllCommission, 2);
            commissionMasterReport.SumTax = Math.Round((Decimal)commissionModel.SumAllTax, 2);
            commissionMasterReport.SumNetTotal = Math.Round((Decimal)commissionModel.SumAllNetTotal, 2);

            commissionMasterReports.Add(commissionMasterReport);
            return commissionMasterReports;
        }

        public MemoryStream GenPDFFile(string reportname, CommissionCollectionViewModel commissionCollectionViewModel)
        {
            List<CommissionMasterReportModel> commissionMasterReports = MapperCommission(commissionCollectionViewModel);
            return FastReportHelper.ShowData(reportname, commissionMasterReports);
        }
    }
}