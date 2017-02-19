using AbleSuccess.Commissions.WebUi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace AbleSuccess.Commissions.WebUi.Common
{
    public static class Helper
    {
        #region [Properties]

        public static string DefaultPassword { get { return "1234"; } }

        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["CommissionsDbConnection"].ConnectionString; } }

        public static string BaseUri { get { return ConfigurationManager.AppSettings["BaseUri"]; } }

        public static string Username
        {
            get
            {
                return (string)HttpContext.Current.Session["_Username"];
            }
            set
            {
                HttpContext.Current.Session["_Username"] = value;
            }
        }

        public static string UserId
        {
            get
            {
                return (string)HttpContext.Current.Session["_UserId"];
            }
            set
            {
                HttpContext.Current.Session["_UserId"] = value;
            }
        }

        public static string UserRole
        {
            get
            {
                return (string)HttpContext.Current.Session["_UserRole"];
            }
            set
            {
                HttpContext.Current.Session["_UserRole"] = value;
            }
        }

        public static decimal CommissionPayRate { get { return Decimal.Parse(ConfigurationManager.AppSettings["CommissionPayRate"]); } }

        public static decimal CommissionForSale { get { return Decimal.Parse(ConfigurationManager.AppSettings["CommissionForSale"]); } }

        public static string LookupPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LookupPath"];
            }
        }

        #endregion

        #region [Lookup]

        public static List<LookupModel> LookupUserStatus { get { return GetXmlLookupConfig("UserStatus"); } }

        public static List<LookupModel> LookupRole { get { return GetXmlLookupConfig("Role"); } }

        public static List<LookupModel> LookupPosition { get { return GetXmlLookupConfig("Position"); } }

        public static List<LookupModel> LookupDevision { get { return GetXmlLookupConfig("Devision"); } }

        public static List<LookupModel> LookupCommissionPercentageOf { get { return GetXmlLookupConfig("CommissionPercentageOf"); } }

        public static List<LookupModel> LookupProductType { get { return GetXmlLookupConfig("ProductType"); } }

        public static List<LookupModel> LookupProductBrand { get { return GetXmlLookupConfig("ProductBrand"); } }

        public static List<LookupModel> LookupTransportationProvince { get { return GetXmlLookupConfig("TransportationProvince"); } }

        public static List<LookupModel> LookupReportType { get { return GetXmlLookupConfig("ReportType"); } }

        public static List<LookupModel> LookupReportSubType { get { return GetXmlLookupConfig("ReportSubType"); } }

        public static List<LookupModel> LookupQuarter { get { return GetXmlLookupConfig("Quarter"); } }

        #endregion

        #region [Methods]

        public static void KillSession()
        {
            HttpContext.Current.Session.Abandon();
        }

        public static string GetHash(string value)
        {
            string hash = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                var data = Encoding.ASCII.GetBytes(value);
                var hashData = new SHA1Managed().ComputeHash(data);

                foreach (var b in hashData)
                    hash += b.ToString("X2");
            }

            return hash;
        }

        public static XmlDocument LookupConfig(string type)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(LookupPath + "Lookup" + type + ".xml");
            return xml;
        }

        public static List<LookupModel> GetXmlLookupConfig(string type)
        {
            List<LookupModel> list = new List<LookupModel>();
            list.Add(new LookupModel
            {
                Key = "-1",
                Value = "---- " + type + " ----"
            });

            try
            {
                XmlDocument lookupConfig = LookupConfig(type);

                XmlNodeList xnList = lookupConfig.SelectNodes("/Lookups/" + type + "/Config");
                foreach (XmlNode xn in xnList)
                {
                    string key = xn["key"].InnerText;
                    string value = xn["value"].InnerText;
                    list.Add(new LookupModel
                    {
                        Key = key,
                        Value = value
                    });
                }
            }
            catch (Exception x)
            {
                string xx = x.Message;
            }

            return list;
        }

        public static string GetNameFromLookup(int id, List<LookupModel> lookup)
        {
            string name = "";
            foreach (LookupModel obj in lookup)
            {
                if (obj.Key == id.ToString()) name = obj.Value;
            }
            return name;
        }

        #endregion
    }
}