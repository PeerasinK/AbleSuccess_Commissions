
using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class ReportViewModel : BaseModel
    {
        public string Title { get; set; }

        public List<LookupModel> Data { get; set; }

        public int ReportType { get; set; }

        public int ReportSubType { get; set; }

        public int Year { get; set; }

        public int Quarter { get; set; }

        public List<LookupModel> LookupYear { get; set; }

        public List<LookupModel> LookupQuarter { get; set; }
    }

    public class MonthSummary
    {
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
    }

    public class ChartData
    {
        public string label { get; set; }

        public decimal y { get; set; }
    }
}