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
    public class DealersController : Controller
    {
        private DLMSDb db = new DLMSDb();

        // GET: Dealers
        public ActionResult Index()
        {
            return View(db.Dealer.ToList());
        }

        // GET: Dealers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealer.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // GET: Dealers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dealers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "d_id,company_name,d_address,email,contact_no,password")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Dealer.Add(dealer);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(dealer);
        }

        // GET: Dealers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealer.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "d_id,company_name,d_address,email,contact_no,password")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealer);
        }

        // GET: Dealers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealer.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dealer dealer = db.Dealer.Find(id);
            db.Dealer.Remove(dealer);
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
        public ActionResult Login(Dealer model)
        {
            using (var context = new DLMSDb())
            {
                bool isValid = context.Dealer.Any(x => x.email == model.email && x.password == model.password);
                if (isValid)
                {
                    Session["dealeremail"] = model.email;
                    var e = Session["dealeremail"].ToString();
                    var firstname = context.Dealer.FirstOrDefault(d => d.email == e);
                    ViewBag.name = firstname.company_name;
                    Session["dealername"] = firstname.company_name.ToString();
                    Session["d_id"] = firstname.d_id.ToString();
                    Session["usertype"] = "dealer";
                    
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session["email"] = null;
            Session["firstname"] = null;
            Session["d_id"] = null;
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
