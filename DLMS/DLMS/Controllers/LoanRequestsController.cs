using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DLMS.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DLMS.Controllers
{
    public class LoanRequestsController : Controller
    {
        private DLMSDb db = new DLMSDb();

        // GET: LoanRequests
        public ActionResult Index()
        {
            var loanRequest = db.LoanRequest.Include(l => l.Car).Include(l => l.Dealer).Include(l => l.Lender).Include(l => l.User);
            var id = Convert.ToInt32(Session["u_id"]);
            var loanRequest1 = loanRequest.Where(l => l.User.u_id == id);
            return View(loanRequest1.ToList());
        }
        public ActionResult Index2()
        {
            var loanRequest = db.LoanRequest.Include(l => l.Car).Include(l => l.Dealer).Include(l => l.Lender).Include(l => l.User);
            var id = Convert.ToInt32(Session["d_id"]);
            var loanRequest1 = loanRequest.Where(l => l.User.u_id == id);
            return View(loanRequest1.ToList());
        }

        // GET: LoanRequests/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanRequest loanRequest = db.LoanRequest.Find(id);
            if (loanRequest == null)
            {
                return HttpNotFound();
            }
            var loanApproved = db.LoanApproved.Include(l => l.LoanRequest).Include(l => l.User).Include(l => l.Lender).Include(l => l.Dealer).Include(l => l.Car);
            var loanApproved1 = loanApproved.FirstOrDefault(l => l.LoanRequest.loan_id == id);
            if( loanApproved1 == null )
            {
                ViewBag.print = "none";
            }
            else
            {
                ViewBag.print = loanApproved1.status;
            }
            return View(loanRequest);
        }

        // GET: LoanRequests/Create
        public ActionResult Create(int? id)
        {
            if (Session["u_id"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            Session["c_id"] = id;
            Car car = db.Car.Find(id);

            Session["d_id"] = car.Dealer.d_id;

            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer");
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name");
            ViewBag.lender_id = new SelectList(db.Lender, "lender_id", "bankname");
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname");
            return View();
        }

        // POST: LoanRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "loan_id,lender_id,downpayment,loan_period,emi,user_address,id_proof_file,income_certificate_file,income_tax_return_file,address_proof_file,gnn,gnn_address,gnn_address_file")] LoanRequest loanRequest)
        {
            if (ModelState.IsValid)
            {
                var loanRequest1 = new LoanRequest()
                {
                    u_id = Convert.ToInt32(Session["u_id"]),
                    c_id = Convert.ToInt32(Session["c_id"]),
                    d_id =Convert.ToInt32(Session["d_id"]),
                    lender_id = loanRequest.lender_id,
                    downpayment = loanRequest.downpayment,
                    loan_period = loanRequest.loan_period,
                    emi = loanRequest.emi,
                    user_address = loanRequest.user_address,
                    gnn = loanRequest.gnn,
                    gnn_address = loanRequest.gnn_address,
                    id_proof = SaveToPhysicalLocation(loanRequest.id_proof_file),
                    income_certificate = SaveToPhysicalLocation(loanRequest.income_certificate_file),
                    income_tax_return = SaveToPhysicalLocation(loanRequest.income_tax_return_file),
                    address_proof = SaveToPhysicalLocation(loanRequest.address_proof_file),
                    gnn_address_p = SaveToPhysicalLocation(loanRequest.gnn_address_file),
                };
                db.LoanRequest.Add(loanRequest1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer", loanRequest.c_id);
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", loanRequest.d_id);
            ViewBag.lender_id = new SelectList(db.Lender, "lender_id", "bankname", loanRequest.lender_id);
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname", loanRequest.u_id);
            return View(loanRequest);
        }
        private string SaveToPhysicalLocation(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var newFileName = DateTime.Now.ToString("yyyy_MM_dd_mm_ss") + "_" + fileName;
                var path = Path.Combine(Server.MapPath("~/Documents"), newFileName);
                var relativePath = "~/Documents/" + newFileName;
                file.SaveAs(path);
                return relativePath;
            }
            return string.Empty;
        }

        // GET: LoanRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanRequest loanRequest = db.LoanRequest.Find(id);
            if (loanRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer", loanRequest.c_id);
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", loanRequest.d_id);
            ViewBag.lender_id = new SelectList(db.Lender, "lender_id", "bankname", loanRequest.lender_id);
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname", loanRequest.u_id);
            return View(loanRequest);
        }

        // POST: LoanRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "loan_id,lender_id,downpayment,u_id,c_id,d_id,loan_period,emi,user_address,id_proof,income_certificate,income_tax_return,address_proof,gnn,gnn_address,gnn_address_p")] LoanRequest loanRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer", loanRequest.c_id);
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", loanRequest.d_id);
            ViewBag.lender_id = new SelectList(db.Lender, "lender_id", "bankname", loanRequest.lender_id);
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname", loanRequest.u_id);
            return View(loanRequest);
        }

        // GET: LoanRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanRequest loanRequest = db.LoanRequest.Find(id);
            if (loanRequest == null)
            {
                return HttpNotFound();
            }
            return View(loanRequest);
        }

        // POST: LoanRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanRequest loanRequest = db.LoanRequest.Find(id);
            db.LoanRequest.Remove(loanRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Approve(int? id)
        {
            LoanRequest lr = db.LoanRequest.Find(id);
            var la = new LoanApproved() {
                lender_id = lr.Lender.lender_id,
                u_id = lr.User.u_id,
                loan_id = lr.loan_id,
                c_id = lr.c_id,
                d_id = lr.d_id,
                status = "Approved"
            };
            db.LoanApproved.Add(la);
            db.SaveChanges();
            return RedirectToAction("Details",new {id = id});
        }
        public ActionResult Reject(int? id)
        {
            LoanRequest lr = db.LoanRequest.Find(id);
            var la = new LoanApproved()
            {
                lender_id = lr.Lender.lender_id,
                u_id = lr.User.u_id,
                loan_id = lr.loan_id,
                c_id = lr.c_id,
                d_id = lr.d_id,
                status = "Rejected"
            };
            db.LoanApproved.Add(la);
            db.SaveChanges();
            return RedirectToAction("Details",new {id = id});
        }
        public ActionResult Index1()
        {
            var loanRequest = db.LoanRequest.Include(l => l.Car).Include(l => l.Dealer).Include(l => l.Lender).Include(l => l.User);
            var id = Convert.ToInt32(Session["lender_id"]);
            var loanRequest1 = loanRequest.Where(l => l.Lender.lender_id == id);
            return View(loanRequest1.ToList());
        }

        public ActionResult Pdf(int? id)
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk("Your Car Invoice Details", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            //0=Left, 1=Centre, 2=Right
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell no 1
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            LoanRequest lr = db.LoanRequest.Find(id);
            var img = lr.Car.img;
            Image image = Image.GetInstance(Server.MapPath(img));
            image.ScaleAbsolute(200, 150);
            cell.AddElement(image);
            table.AddCell(cell);

            //Cell no 2
            chunk = new Chunk("USER DETAILS:\nName:" + lr.User.firstname + "\nAddress:" + lr.user_address + "\nContact No:" + lr.User.mobile_no + "\nEmail:" + lr.User.email + "\n\n\n\nDEALER DETAILS:\nCompany:" + lr.Dealer.company_name + "\nAddress:" + lr.Dealer.d_address + "\nContact No:" + lr.Dealer.contact_no + "\nEmail:" + lr.Dealer.email, FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);


            //Add table to document
            pdfDoc.Add(table);

            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            chunk = new Chunk("CAR DETAILS:\nCar Manufacturer:" + lr.Car.car_manufacturer + "\nModel:" + lr.Car.car_model + "\nCar Type:" + lr.Car.car_type + "\nPrice:₹" + lr.Car.price, FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            chunk = new Chunk("LENDER DETAILS:\nBank Name:" + lr.Lender.bankname + "\nIFSC Code:" + lr.Lender.ifsc + "\nBranch Name:" + lr.Lender.branchname + "\nContact No.:" + lr.Lender.contact_no+"\nEmail:"+lr.Lender.b_email, FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);


            pdfDoc.Add(table);


            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            var rem = lr.Car.price - lr.downpayment;
            chunk = new Chunk("PAYMENT DETAILS:\nDownpayment:" + lr.downpayment + "\nLoan Period:" + lr.loan_period + "\nRemaining Amount:" + rem +"\nEMI:"+lr.emi, FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);


            pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            return RedirectToAction("Details",new { id = id});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
