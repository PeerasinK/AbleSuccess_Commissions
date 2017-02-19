using System;
using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class CommissionMasterReportModel
    {
        public string Fullname { get; set; }

        public string PositionName { get; set; }

        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }

        public ICollection<CommissionReportModel> CommissionsReport { get; set; }

        public decimal SumRevenue { get; set; }

        public decimal SumCOM { get; set; }

        public decimal SumTax { get; set; }

        public decimal SumNetTotal { get; set; }

    }
    public class CommissionReportModel
    {
        public DateTime Date { get; set; }

        public string PO { get; set; }

        public string ProductName { get; set; }

        public string ProductTypeName { get; set; }

        public string CustomerName { get; set; }

        public string SalesName { get; set; }

        public string ComRole { get; set; }

        public decimal Revenue { get; set; }

        public decimal TotalCom { get; set; }

        public decimal Tax { get; set; }

        public decimal NetTotal { get; set; }
    }
}