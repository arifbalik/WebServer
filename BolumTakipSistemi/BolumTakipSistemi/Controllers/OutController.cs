using AMEDYA.TYPES;
using Pdks.Classes;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Pdks.Controllers
{

    public class OutController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

        public string Entrance()
        {

            string strReturn = "";

            try
            {
                //https://attendance.tayeks.com.tr/Out/Entrance?SectionNumber=17&CardNumber=001
                //her bölümün cihaz no su var
                int iClassID = Pv.Pint(Request["SectionNumber"]);
                string strCardNumber = Pv.Pstr(Request["CardNumber"]);
                if (iClassID <= 0 || strCardNumber.Length == 0)
                {

                    return "-4";
                }

                RetCode oRet = Util.DB.RunTable("ReadBarcode",
                   iClassID,
                   strCardNumber);

                if (oRet.iRet == Back.Ok)
                {
                    strReturn = oRet.ToRow["Done"].ToString();
                    //-1 ise bölüme ait kart değil veya kart no hatalı
                    //-2 ise bu güne ait kayıt var
                    //-3 ise zaman bölüm sıkıntısı var bölüm de tanımlı zaman içinde değil giriş yapılan zaman

                    if (Pv.Pint(strReturn) > 0)
                    {
                        strReturn = "1";

                    }

                }

            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return strReturn;

        }


	}
}