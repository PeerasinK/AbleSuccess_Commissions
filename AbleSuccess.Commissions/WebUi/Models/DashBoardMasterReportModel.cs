using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class DashBoardMasterReportModel
    {
        public string Name { get; set; }
        public string TitleReport { get; set; }
        public string ReportType { get; set; }
        public string ReportSubType { get; set; }
        public string Year { get; set; }
        public List<ChartDataModel> ChartDataModels { get; set; }

    }

    public class ChartDataModel
    {
        public string label { get; set; }

        public decimal y { get; set; }
    }
}