using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DLMS.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DLMS.Controllers
{
    public class FullPaymentsController : Controller
    {
        private DLMSDb db = new DLMSDb();

        public int x { get; private set; }

        // GET: FullPayments
        public ActionResult Index()
        {
            var id = Convert.ToInt32(Session["u_id"]);
            var fullPayment = db.FullPayment.Include(f => f.Car).Include(f => f.Dealer).Include(f => f.User);
            var fp = fullPayment.Where(f => f.u_id == id);
            return View(fp.ToList());
        }
        public ActionResult Index1()
        {
            var id = Convert.ToInt32(Session["d_id"]);
            var fullPayment = db.FullPayment.Include(f => f.Car).Include(f => f.Dealer).Include(f => f.User);
            var fp = fullPayment.Where(f => f.d_id == id);
            return View(fp.ToList());
        }


        // GET: FullPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FullPayment fullPayment = db.FullPayment.Find(id);
            if (fullPayment == null)
            {
                return HttpNotFound();
            }
            return View(fullPayment);
        }

        // GET: FullPayments/Create
        public ActionResult Create(int? id)
        {
            if(Session["u_id"] == null) {
                return RedirectToAction("Login", "Users");
            }
            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer");
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name");
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname");
            Session["c_id"] = id;
            var car = db.Car.Find(id);
            Session["d_id"] = car.Dealer.d_id;
            return View();
        }

        // POST: FullPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fp_id,downpayment,user_address,mode")] FullPayment fullPayment)
        {
            if (ModelState.IsValid)
            {
                var payment = new FullPayment()
                {
                    u_id = Convert.ToInt32(Session["u_id"]),
                    c_id = Convert.ToInt32(Session["c_id"]),
                    d_id = Convert.ToInt32(Session["d_id"]),
                    downpayment = Convert.ToInt32(fullPayment.downpayment),
                    user_address = fullPayment.user_address,
                    mode = fullPayment.mode,
                };

                db.FullPayment.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer", fullPayment.c_id);
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", fullPayment.d_id);
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname", fullPayment.u_id);
            return View(fullPayment);
        }

        // GET: FullPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FullPayment fullPayment = db.FullPayment.Find(id);
            if (fullPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer", fullPayment.c_id);
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", fullPayment.d_id);
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname", fullPayment.u_id);
            return View(fullPayment);
        }

        // POST: FullPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fp_id,u_id,d_id,c_id,downpayment,user_address,mode")] FullPayment fullPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fullPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.c_id = new SelectList(db.Car, "c_id", "car_manufacturer", fullPayment.c_id);
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", fullPayment.d_id);
            ViewBag.u_id = new SelectList(db.User, "u_id", "firstname", fullPayment.u_id);
            return View(fullPayment);
        }

        // GET: FullPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FullPayment fullPayment = db.FullPayment.Find(id);
            if (fullPayment == null)
            {
                return HttpNotFound();
            }
            return View(fullPayment);
        }

        // POST: FullPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FullPayment fullPayment = db.FullPayment.Find(id);
            db.FullPayment.Remove(fullPayment);
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

        public ActionResult CreatePdf(int? id)
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
            FullPayment fullPayment = db.FullPayment.Find(id);
            var img = fullPayment.Car.img;
            Image image = Image.GetInstance(Server.MapPath(img));
            image.ScaleAbsolute(200, 150);
            cell.AddElement(image);
            table.AddCell(cell);

            //Cell no 2
            chunk = new Chunk("USER DETAILS:\nName:" + fullPayment.User.firstname + "\nAddress:" + fullPayment.user_address +"\nContact No:"+ fullPayment.User.mobile_no+"\nEmail:"+fullPayment.User.email+ "\n\n\n\nDEALER DETAILS:\nCompany:" + fullPayment.Dealer.company_name + "\nAddress:" + fullPayment.Dealer.d_address + "\nContact No:" + fullPayment.Dealer.contact_no + "\nEmail:" + fullPayment.Dealer.email,  FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
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
            table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            chunk = new Chunk("CAR DETAILS:\nCar Manufacturer:" + fullPayment.Car.car_manufacturer +"\nModel:"+fullPayment.Car.car_model+"\nCar Type:"+fullPayment.Car.car_type+ "\nPrice:₹" + fullPayment.Car.price, FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
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
            var rem = fullPayment.Car.price - fullPayment.downpayment;
            chunk = new Chunk("PAYMENT DETAILS:\nAdvance Token:" + fullPayment.downpayment + "\nPayment Type:" + fullPayment.mode + "\nRemaining Amount:"+rem , FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
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

            return RedirectToAction("Index");
        }
    }
}
