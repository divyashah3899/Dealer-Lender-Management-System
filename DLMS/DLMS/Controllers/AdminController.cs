using DLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DLMS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(String email, String password)
        {
            using (var context = new DLMSDb())
            {
                if (email == "dlms@gmail.com" && password == "dlms@123")
                {
                    Session["adminemail"] = email;
                    Session["adminname"] = "ADMIN";
                    Session["usertype"] = "admin";
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
        }
    }
}