using AMEDYA.TYPES;
using Pdks.Classes;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pdks.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //ViewBag.Result = "1";
            ViewBag.ReturnUrl = returnUrl;
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(LoginUserModel model, string returnUrl)
        {
            model.Password = model.Password;

            RetCode _ret = Util.DB.RunTable("UserLogin", model.Username, model.Password);


            if (_ret.iRet == Back.Ok)
            {
                bool isCookiePersistent = model.RememberMe;
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(11225533, 
                    "BolumUser" + model.Username,
                    DateTime.Now, DateTime.Now.AddMinutes(6000),
                    isCookiePersistent,
                    "U" + "|" + _ret.ToRow["UserID"].ToString() + "|" + _ret.ToRow["UserType"].ToString() + "|" + _ret.ToRow["PersonelName"].ToString());

                String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                //Create a cookie, and then add the encrypted ticket to the cookie as data.
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                if (true == isCookiePersistent)
                    authCookie.Expires = authTicket.Expiration;

                //Add the cookie to the outgoing cookies collection.
                Response.Cookies.Add(authCookie);
                return RedirectToLocal(returnUrl);

            }
            else
            {
                ViewBag.Result = "Please check your user info";

                ModelState.AddModelError("", "Kullanıcı adı ve şifreniz hatalı, Lütfen kontrol edip tekrar deneyiniz");
                return View(model);
            }


        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string LogOff()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            //return RedirectToAction("Index", "Home");
            return "1";

        }




	}
}