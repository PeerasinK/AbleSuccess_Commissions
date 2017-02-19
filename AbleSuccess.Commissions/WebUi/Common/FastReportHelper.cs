using FastReport;
using System.Collections;
using System.IO;

namespace AbleSuccess.Commissions.WebUi.Common
{
    public class FastReportHelper
    {
        public static MemoryStream ShowData(string reportname, IEnumerable data)
        {
            using (Report report = GetLoadedReport(reportname))
            {
                if (data != null)
                {
                    RegisterData(report, data);
                }
                //report.Show();

                if (report.Report.Prepare())
                {
                    // Set PDF export props
                    FastReport.Export.Pdf.PDFExport pdfExport = new FastReport.Export.Pdf.PDFExport();
                    pdfExport.ShowProgress = false;
                    pdfExport.Subject = "Subject";
                    pdfExport.Title = "Title";
                    pdfExport.Compressed = true;
                    pdfExport.AllowPrint = true;
                    pdfExport.EmbeddingFonts = true;

                    //MemoryStream strm = new MemoryStream();
                    //report.Report.Export(pdfExport, strm);
                    //report.Dispose();
                    //pdfExport.Dispose();
                    //strm.Position = 0;
                    using (MemoryStream strm = new MemoryStream())
                    {
                        report.Report.Export(pdfExport, strm);
                        report.Dispose();
                        pdfExport.Dispose();
                        strm.Position = 0;
                        //for check data
                        //File.WriteAllBytes(@"C:\Users\job\Desktop\PD1F.pdf", strm.ToArray());
                        return strm;
                    }
                }
                else
                {
                    return new MemoryStream();
                }
            }
        }

        /// <summary>
        /// Gets the loaded report.
        /// </summary>
        /// <returns>Report.</returns>
        private static Report GetLoadedReport(string reportname)
        {
            return Report.FromFile(reportname);
        }

        /// <summary>
        /// Registers the data to report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="data">The data.</param>
        private static void RegisterData(Report report, IEnumerable data)
        {
            report.RegisterData(data, "ReportCommission");
            report.GetDataSource("ReportCommission").Enabled = true;
        }

    }
}