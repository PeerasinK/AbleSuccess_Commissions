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
    public class ReportManager
    {
        private readonly IUnitOfWork _unitOfWork;

        #region [CONSTRUCTOR]

        public ReportManager()
        {
            _unitOfWork = new AbleSuccessUnitOfWork(Helper.ConnectionString);
        }

        #endregion

        #region [PUBLIC METHODS]

        public List<LookupModel> GetLookupYear()
        {
            List<DateTime> poDateList = _unitOfWork.GetRepository<Txn_PO>().GetQueryable(o => o.Status == 1).OrderByDescending(o => o.PoDate).Select(o => o.PoDate).ToList();
            List<LookupModel> lookup = new List<LookupModel>();

            foreach (DateTime poDate in poDateList)
            {
                string year = poDate.Year.ToString();

                if (!lookup.Exists(o => o.Value == year)) lookup.Add(new LookupModel { Key = year, Value = year });
            }

            return lookup;
        }

        public List<ChartData> YearlyReport(int type, int subType, int year)
        {
            DateTime dateFrom = new DateTime(year, 1, 1);
            DateTime dateTo = new DateTime(year, 12, 31);

            // Get data
            List<PoDetailModel> poList = (from pd in _unitOfWork.GetRepository<Txn_PODetail>().GetQueryable(o => o.Status == 1)
                                          join p in _unitOfWork.GetRepository<Txn_PO>().GetQueryable(o => o.Status == 1 && dateFrom <= o.PoDate && o.PoDate <= dateTo)
                                          on pd.PoId equals p.PoId

                                          select new PoDetailModel
                                          {
                                              PoDate = p.PoDate,
                                              TotalPrice = pd.TotalPrice,
                                              Profit = pd.Profit,
                                          }).ToList();

            List<ChartData> data = new List<ChartData>();
            MonthSummary summary = new MonthSummary();

            foreach (PoDetailModel po in poList)
            {
                decimal value = decimal.Zero;
                if (type == 0) value = po.TotalPrice;
                else if (type == 1) value = po.Profit;

                if (po.PoDate.Month == 1) summary.Jan += value;
                else if (po.PoDate.Month == 2) summary.Feb += value;
                else if (po.PoDate.Month == 3) summary.Mar += value;
                else if (po.PoDate.Month == 4) summary.Apr += value;
                else if (po.PoDate.Month == 5) summary.May += value;
                else if (po.PoDate.Month == 6) summary.Jun += value;
                else if (po.PoDate.Month == 7) summary.Jul += value;
                else if (po.PoDate.Month == 8) summary.Aug += value;
                else if (po.PoDate.Month == 9) summary.Sep += value;
                else if (po.PoDate.Month == 10) summary.Oct += value;
                else if (po.PoDate.Month == 11) summary.Nov += value;
                else if (po.PoDate.Month == 12) summary.Dec += value;
            }

            if (subType == 0)
            {
                data.Add(new ChartData { label = "Jan", y = summary.Jan });
                data.Add(new ChartData { label = "Feb", y = summary.Feb });
                data.Add(new ChartData { label = "Mar", y = summary.Mar });
                data.Add(new ChartData { label = "Apr", y = summary.Apr });
                data.Add(new ChartData { label = "May", y = summary.May });
                data.Add(new ChartData { label = "Jun", y = summary.Jun });
                data.Add(new ChartData { label = "Jul", y = summary.Jul });
                data.Add(new ChartData { label = "Aug", y = summary.Aug });
                data.Add(new ChartData { label = "Sep", y = summary.Sep });
                data.Add(new ChartData { label = "Oct", y = summary.Oct });
                data.Add(new ChartData { label = "Nov", y = summary.Nov });
                data.Add(new ChartData { label = "Dec", y = summary.Dec });
            }
            else if (subType == 1)
            {
                data.Add(new ChartData { label = "Q1", y = summary.Jan + summary.Feb + summary.Mar });
                data.Add(new ChartData { label = "Q2", y = summary.Apr + summary.May + summary.Jun });
                data.Add(new ChartData { label = "Q3", y = summary.Jul + summary.Aug + summary.Sep });
                data.Add(new ChartData { label = "Q4", y = summary.Oct + summary.Nov + summary.Dec });
            }

            return data;
        }

        #endregion

        #region [PRIVATE METHOD]


        #endregion
    }
}