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
    public class LendersController : Controller
    {
        private DLMSDb db = new DLMSDb();

        // GET: Lenders
        public ActionResult Index()
        {
            return View(db.Lender.ToList());
        }

        // GET: Lenders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lender lender = db.Lender.Find(id);
            if (lender == null)
            {
                return HttpNotFound();
            }
            return View(lender);
        }

        // GET: Lenders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lenders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lender_id,bankname,branchname,ifsc,b_email,contact_no,password")] Lender lender)
        {
            if (ModelState.IsValid)
            {
                db.Lender.Add(lender);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(lender);
        }

        // GET: Lenders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lender lender = db.Lender.Find(id);
            if (lender == null)
            {
                return HttpNotFound();
            }
            return View(lender);
        }

        // POST: Lenders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lender_id,bankname,branchname,ifsc,b_email,contact_no,password")] Lender lender)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lender).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lender);
        }

        // GET: Lenders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lender lender = db.Lender.Find(id);
            if (lender == null)
            {
                return HttpNotFound();
            }
            return View(lender);
        }

        // POST: Lenders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lender lender = db.Lender.Find(id);
            db.Lender.Remove(lender);
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
        public ActionResult Login(Lender model)
        {
            using (var context = new DLMSDb())
            {
                bool isValid = context.Lender.Any(x => x.b_email == model.b_email && x.password == model.password);
                if (isValid)
                {
                    Session["lenderemail"] = model.b_email;
                    var e = Session["lenderemail"].ToString();
                    var firstname = context.Lender.FirstOrDefault(d => d.b_email == e);
                    ViewBag.name = firstname.bankname;
                    Session["lendername"] = firstname.bankname.ToString();
                    Session["lender_id"] = firstname.lender_id.ToString();
                    Session["usertype"] = "lender";
                    FormsAuthentication.SetAuthCookie(model.b_email, false);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            //Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult forgot()
        {
            return View();
        }
        [HttpPost]
        public ActionResult forgot(Dealer model)
        {
            using (var context = new DLMSDb())
            {
                var a = context.Dealer.FirstOrDefault(x => x.email == model.email);
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
                    var body = "Your OTP Is!! " + value;
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
                    return RedirectToAction("forgotone", "Dealers");
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
                return RedirectToAction("changepass", "Dealers");
            }
            return View();
        }
        public ActionResult changepass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changepass(Dealer model)
        {


            string a = Session["forgotmail"].ToString();

            Dealer uname = db.Dealer.FirstOrDefault(x => x.email == a);
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
