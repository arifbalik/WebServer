using AMEDYA.CORE;
using AMEDYA.TYPES;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pdks.Classes
{

    public static class Util
    {

        public static CultureInfo CUL_EN = new CultureInfo("en-GB");

        private static IDataCore m_DB = null;

        public static IDataCore DB
        {
            get
            {
                if (m_DB == null)
                {
                    m_DB = new SqlCore();
                    m_DB.ConnStr = ConfigurationManager.ConnectionStrings["sqlconn"].ConnectionString;

                }
                return m_DB;
            }

        }

        public static string UretimYeri()
        {
            return "2200";

        }

        private static string[] GetUserData()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    return ticket.UserData.Split('|');
                }
            }
            return null;
        }

        public static string FasonCompanyName
        {
            get
            {
                string[] data = GetUserData();
                if (data != null && data.Length > 2)
                    return Pv.Pstr(data[3]);
                else
                    return "";
            }
        }

        public static int FasonDepoID
        {
            get
            {
                string[] data = GetUserData();
                if (data != null && data.Length > 4)
                    return Pv.Pint(data[4]);
                else
                    return 0;
            }
        }

        public static int CompanyID
        {
            get
            {
                string[] data = GetUserData();
                if (data != null && data.Length > 1)
                    return Pv.Pint(data[1]);
                else
                    return 0;
            }
        }

        public static JsonResult GetResult(bool success)
        {

            return GetResult(success, "");
        }

        public static JsonResult GetResult(bool success, string message)
        {
            JsonResult _json = new JsonResult();
            _json.Data = new
            {
                success = success,
                message = message
            };
            _json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return _json;
        }

        public static List<Dictionary<string, string>> GetJsonData(DataTable _dt)
        {
            List<Dictionary<string, string>> _lst = new List<Dictionary<string, string>>();
            foreach (DataRow _dr in _dt.Rows)
            {
                Dictionary<string, string> _dic = new Dictionary<string, string>();
                foreach (DataColumn _dc in _dt.Columns)
                {
                    if (_dc.DataType == typeof(DateTime) && _dr[_dc.ColumnName] != DBNull.Value)
                        _dic[_dc.ColumnName] = ((DateTime)_dr[_dc.ColumnName]).ToString(new System.Globalization.CultureInfo("en-GB"));
                    else
                        _dic[_dc.ColumnName] = _dr[_dc.ColumnName].ToString();
                }
                _lst.Add(_dic);
            }

            return _lst;
        }

        public static List<List<string>> GetJsonDataTable(DataTable _dt)
        {

            List<List<string>> _lst = new List<List<string>>();


            foreach (DataRow _dr in _dt.Rows)
            {
                List<string> str = new List<string>();

                foreach (DataColumn _dc in _dt.Columns)
                {

                    if (_dc.DataType == typeof(DateTime) && _dr[_dc.ColumnName] != DBNull.Value)
                        //_dic[_dc.ColumnName] = ((DateTime)_dr[_dc.ColumnName]).ToString(new System.Globalization.CultureInfo("en-GB"));
                        str.Add(((DateTime)_dr[_dc.ColumnName]).ToString(new System.Globalization.CultureInfo("en-GB")));
                    else
                        str.Add(_dr[_dc.ColumnName].ToString());




                }

                _lst.Add(str);

            }

            return _lst;
        }

        public static void WriteLog(Exception eX)
        {

            RetCode oRet =
            DB.RunOne("ErrorInsert",
                 eX.Message + "  " + eX.StackTrace
                 );

            if (oRet.iRet == Back.Ok)
            {
                int i1 = oRet.ToInt;

            }

        }

        public static string ReplaceInjection(string strParam)
        {
            return strParam.Replace("insert", "").Replace("INSERT", "").Replace("UPDATE", "")
                .Replace("update", "").Replace("delete", "").Replace("DELETE", "");

        }

        public static int TaypaMainDepotID
        {
            get { return 2135; }
        }

        public static DateTime GetServerTime()
        {

            DateTime dtDate = DateTime.Now;

            try
            {
                RetCode oRet = DB.RunOne(System.Data.CommandType.Text, "SELECT GETDATE()");

                if (oRet.iRet == Back.Ok)
                {
                    dtDate = Pv.PDate(oRet.oReturn, DateTime.Now);


                }
                //dtDate = Convert.ToDateTime(Database.ExecuteScalar("SELECT GETDATE()").ToString());

            }
            catch (Exception exc)
            {
                WriteLog(exc);
            }

            return dtDate;

        }

        public static string GetTimeTextSpan(TimeSpan ts)
        {
            string strReturn = "";

            if (ts.TotalDays >= 1)
            {
                strReturn = ((int)Math.Floor(ts.TotalDays)).ToString() + " gün ";

            }
            else if (ts.TotalHours >= 1)
            {
                int iSaat = (int)Math.Floor(ts.TotalHours);
                strReturn = iSaat + " saat ";



            }
            else if (ts.TotalMinutes >= 1)
            {
                int iDakika = (int)Math.Floor(ts.TotalMinutes);
                strReturn = iDakika + " dakika ";

            }
            else
            {
                int iSaniye = (int)Math.Floor(ts.TotalSeconds);
                strReturn = iSaniye + " saniye ";

            }

            return strReturn;
        }

        public static string GetTypeOrg
        {
            get
            {
                string[] data = GetUserData();
                if (data != null && data.Length > 1)
                {
                    return Pv.Pstr(data[0]);

                }
                else
                {
                    return "";

                }

            }
        }

        public static int GetUserType
        {
            get
            {
                string[] data = GetUserData();
                if (data != null && data.Length > 1)
                {
                    if (data[0] == "U")
                    {
                        return Pv.Pint(data[2]);
                    }
                    else
                    {
                        return -1;

                    }

                }
                else
                {
                    return -1;

                }

            }

        }

        public static int UserID
        {
            get
            {
                string[] data = GetUserData();
                if (data != null && data.Length > 1)
                {

                    return Pv.Pint(data[1]);


                }
                else
                {
                    return -2;

                }


            }
        }

        public static string castDate(string str)
        {

            string strReturn = str;
            try
            {
                strReturn = str.Substring(0, 2) + "." + str.Substring(2, 2) + "." + str.Substring(4, 4);

            }
            catch (Exception eX)
            {
                WriteLog(eX);
            }

            return strReturn;

        }
       
        public static int GetBarcodeMainID(string strBarcode)
        {

            int iBarcodeMainID = 0;

            try
            {

                RetCode mRet = Util.DB.RunTable("GetBarcodeMainID",
                   strBarcode);

                if (mRet.iRet == Back.Ok)
                {
                    iBarcodeMainID = Pv.Pint(mRet.ToRow["BarcodeMainID"]);

                }

            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return iBarcodeMainID;

        }

        public static DataRow GetBarcodeMain(string strBarcode)
        {


            DataRow drReturn = null;

            try
            {

                RetCode mRet = Util.DB.RunTable("GetBarcodeMain",
                   strBarcode);

                if (mRet.iRet == Back.Ok)
                {

                    drReturn = mRet.ToRow;

                }


            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return drReturn;

        }

        public static DataTable GetKaliteTipList()
        {

            DataTable dt = new DataTable();
            try
            {
                RetCode oRet = Util.DB.RunTable("KaliteList");

                if (oRet.iRet != Back.Err)
                {
                    dt = oRet.ToTable;

                }

            }
            catch (Exception eX)
            {
                WriteLog(eX);

            }


            return dt;
        }


    }

}