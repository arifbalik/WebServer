using AMEDYA.TYPES;
using Pdks.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace Student.Controllers
{
    public class StudentController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ClassList()
        {


            return View();
        }

        [ValidateInput(false)]
        public ActionResult ShowPDF()
        {

            if (Request["f"] != null)
            {
                string strFilename = Request["f"].ToString();
                ViewBag.Filename = "/files/" + strFilename;

                return View();

            }
            else
            {

                return RedirectToAction("../Home");


            }

        }

        public ActionResult StudentList()
        {

            DataTable dtList = new DataTable();

            RetCode oRet = Util.DB.RunTable("ClassList");

            dtList = oRet.ToTable;

            ViewBag.BolumListOk = true;
            ViewBag.BolumList = dtList;
            return View();
        }

        public ActionResult Entrance()
        {

            RetCode oRet = Util.DB.RunTable("ClassList");

            DataTable dtList = oRet.ToTable;

            ViewBag.BolumListOk = true;
            ViewBag.BolumList = dtList;


            return View();
        }

        public ActionResult EntranceReport()
        {

            RetCode oRet = Util.DB.RunTable("ClassList");

            DataTable dtList = oRet.ToTable;

            ViewBag.BolumListOk = true;
            ViewBag.BolumList = dtList;


            return View();
        }

        public JsonResult StudentListAll()
        {

            JsonResult m_json = new JsonResult();
            try
            {
                //yt.lstRooms
                //yetkisi olan odalar gelsin

                RetCode oRet = Util.DB.RunTable("StudentList");

                DataTable dtRooms = oRet.ToTable;


                m_json.Data = new
                {
                    recordCount = dtRooms.Rows.Count,
                    success = true,
                    failure = false,
                    error = "",
                    data = Util.GetJsonData(dtRooms)

                };
                m_json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;





            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return m_json;

        }

        public JsonResult ClassListAll()
        {

            JsonResult m_json = new JsonResult();
            try
            {
                //yt.lstRooms
                //yetkisi olan odalar gelsin

                RetCode oRet = Util.DB.RunTable("ClassList");

                DataTable dtRooms = oRet.ToTable;


                m_json.Data = new
                {
                    recordCount = dtRooms.Rows.Count,
                    success = true,
                    failure = false,
                    error = "",
                    data = Util.GetJsonData(dtRooms)

                };
                m_json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;





            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return m_json;

        }

        public JsonResult EntranceListDate()
        {
            JsonResult m_json = new JsonResult();
            try
            {
                //yt.lstRooms
                //yetkisi olan odalar gelsin
                int iClassID = Pv.Pint(Request["ClassID"]);
                DateTime dtmDate = Pv.PDate(Request["Tarih"]);

                RetCode oRet = Util.DB.RunTable("EntranceListDate",
                    iClassID,
                    dtmDate);

                DataTable dtRooms = oRet.ToTable;


                m_json.Data = new
                {
                    recordCount = dtRooms.Rows.Count,
                    success = true,
                    failure = false,
                    error = "",
                    data = Util.GetJsonData(dtRooms)

                };
                m_json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;





            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return m_json;

        }

        //[HttpPost]
        //public HttpResponseMessage PostOrder(HttpRequestMessage request)
        //{
        //    string body = request.Content.ReadAsStringAsync().Result;
        //    var siparisList = JsonConvert.DeserializeObject<List<JsSiparis>>(body);


        //}


        public string SaveClass()
        {

            string strReturn = "";
            try
            {
                int iClassID = Pv.Pint(Request["ClassID"]);
                int iOperation = Pv.Pint(Request["Operation"]);
                string strClassName = Pv.Pstr(Request["ClassName"]);

                int iWorkDay = Pv.Pint(Request["WorkDay"]);
                string strStartTime = Pv.Pstr(Request["StartTime"]);
                string strEndTime = Pv.Pstr(Request["EndTime"]);


                if (iOperation == 1)//yeni
                {

                    RetCode oRet = Util.DB.RunTable("ClassInsert",
                        strClassName,
                        iWorkDay,
                        strStartTime,
                        strEndTime);

                    iClassID = Pv.Pint(oRet.ToRow[0]);
                    strReturn = iClassID.ToString();


                }
                else if (iOperation == 2)//edit
                {
                    RetCode oRet = Util.DB.Run("ClassUpdate",
                        iClassID,
                        strClassName,
                        iWorkDay,
                        strStartTime,
                        strEndTime);

                    strReturn = "1";

                }

            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return strReturn;

        }

        public string SaveStudent()
        {
            string strReturn = "";
            try
            {
                int iOperation = Pv.Pint(Request["Operation"]);
                int iStudentID = Pv.Pint(Request["StudentID"]);
                int iClassID = Pv.Pint(Request["ClassID"]);
                string strCardNumber = Pv.Pstr(Request["CardNumber"]);
                string strStudentName = Pv.Pstr(Request["StudentName"]);


                if (iOperation == 1)//yeni
                {

                    RetCode oRet = Util.DB.RunTable("StudentInsert",
                        iClassID,
                        strCardNumber,
                        strStudentName);

                    iStudentID = Pv.Pint(oRet.ToRow[0]);
                    strReturn = iStudentID.ToString();

                }
                else if (iOperation == 2)//edit
                {
                    RetCode oRet = Util.DB.Run("StudentUpdate",
                        iStudentID,
                        iClassID,
                        strCardNumber,
                        strStudentName);

                    strReturn = "1";

                }

            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return strReturn;

        }

        public string DeleteClass()
        {
            //bir odaya bağlımı kontrol et öyle sil
            string strReturn = "";
            try
            {
                //schedule varsa silme
                int iClassID = Pv.Pint(Request["ClassID"]);

                RetCode oRet = Util.DB.RunTable("ClassDelete",
                    iClassID);

                if (oRet.iRet == Back.Ok)
                {
                    strReturn = "1";


                }


            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }

            return strReturn;

        }

        public string DeleteStudent()
        {
            //bir odaya bağlımı kontrol et öyle sil
            string strReturn = "";
            try
            {
                //schedule varsa silme
                int iStudentID = Pv.Pint(Request["StudentID"]);

                RetCode oRet = Util.DB.RunTable("StudentDelete",
                    iStudentID);


                if (oRet.iRet == Back.Ok)
                {
                    strReturn = "1";


                }

            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }

            return strReturn;

        }

        public string ReadBarkod()
        {
            string strReturn = "";
            try
            {
                int iClassID = Pv.Pint(Request["ClassID"]);
                string strCardNumber = Pv.Pstr(Request["CardNumber"]);

                if (iClassID == 0)
                {
                    return "-4";

                }
                
                
                RetCode oRet = Util.DB.RunTable("ReadBarcode",
                    iClassID,
                    strCardNumber);

                if (oRet.iRet == Back.Ok)
                {
                    strReturn = oRet.ToRow["Done"].ToString();


                }

            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }


            return strReturn;

        }



        public JsonResult EntranceListDateAll()
        {
            JsonResult m_json = new JsonResult();
            try
            {
                int iClassID = Pv.Pint(Request["ClassID"]);
                DateTime dtmDate = Pv.PDate(Request["Tarih"]);

                RetCode oRet = Util.DB.RunTable("EntranceListDateAll",
                    iClassID,
                    dtmDate);

                DataTable dtRooms = oRet.ToTable;


                m_json.Data = new
                {
                    recordCount = dtRooms.Rows.Count,
                    success = true,
                    failure = false,
                    error = "",
                    data = Util.GetJsonData(dtRooms)

                };
                m_json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;





            }
            catch (Exception eX)
            {
                Util.WriteLog(eX);

            }
            return m_json;

        }


        [HttpPost]
        [ValidateInput(false)]
        public string SavePDF()
        {

            string strFilename = DateTime.Now.ToShortDateString().Replace(".", "").Replace(",", "") + DateTime.Now.ToShortDateString().Replace(":", "").Replace(" ", "").Replace(".", "") + ".pdf";

            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                string _virtualPath = "files/" + strFilename;
                string _contentPath = Server.MapPath("~/" + _virtualPath);

                Request.Files[0].SaveAs(_contentPath);
                return strFilename;


            }
            else
            {
                return "";

            }

        }

	}
}