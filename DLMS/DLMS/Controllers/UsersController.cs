using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DLMS.Models;

namespace DLMS.Controllers
{
    public class UsersController : Controller
    {
        private DLMSDb db = new DLMSDb();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "u_id,firstname,lastname,email,mobile_no,password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "u_id,firstname,lastname,email,mobile_no,password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User model)
        {
            using (var context = new DLMSDb())
            {
                bool isValid = context.User.Any(x => x.email == model.email && x.password == model.password);
                if (isValid)
                {
                    Session["useremail"] = model.email;
                    var e = Session["useremail"].ToString();
                    var firstname = context.User.FirstOrDefault(d => d.email == e);
                    ViewBag.name = firstname.firstname;
                    Session["username"]=firstname.firstname.ToString();
                    Session["u_id"] = firstname.u_id.ToString();
                    Session["usertype"] = "user";
                    //Response.Write("Name" + firstname.ToString());
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    //return Content("Name"+firstname.ToString());
                   return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }
        }
        public ActionResult Logout()
        {
            // Session["email"] = null;
            // Session["firstname"] = null ;
            //Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        public ActionResult forgot()
        {
            return View();
        }
        [HttpPost]
        public ActionResult forgot(User model)
        {
            using (var context = new DLMSDb())
            {
                var a = context.User.FirstOrDefault(x => x.email == model.email);
                if (a != null)
                {
                    Random random = new Random();
                    int value = random.Next(10000);
                    Session["otp"] = value;
                    Session["forgotmail"] = model.email;
                    //  ViewBag["forgot"]= model.email;
                    var senderEmail = new MailAddress("dlms.mvc@gmail.com", "DLMS");
                    var receiverEmail = new MailAddress(model.email, "Receiver");
                    var password = "dlms@123";
                    var sub = "Forgot Password";
                    var body = "Your OTP Is!!" + value;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return RedirectToAction("forgotone", "Users");
                    // Response.Write("<script>alert('Main Sef')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Email Not Found')</script>");
                }

            }
            return View();
        }
        public ActionResult forgotone()
        {
            return View();
        }
        [HttpPost]
        public ActionResult forgotone(string otp)
        {
            string so = Session["otp"].ToString();
            if (otp != so)
            {
                Response.Write("<script>alert('Not Matched')</script>");
            }
            else
            {
                Response.Write("<script>alert('Matche Found')</script>");
                return RedirectToAction("changepass", "Users");
            }
            return View();
        }
        public ActionResult changepass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changepass(User model)
        {


            string a = Session["forgotmail"].ToString();

            User uname = db.User.FirstOrDefault(x => x.email == a);
            if (uname != null)
            {
                uname.password = model.password;
                db.SaveChanges();
                Response.Write("<script>alert('Password Updated Successfully')</script>");
            }

            return RedirectToAction("Login");
        }
    }
}
