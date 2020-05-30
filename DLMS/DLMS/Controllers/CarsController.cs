using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DLMS.Models;

namespace DLMS.Controllers
{
    public class CarsController : Controller
    {
        private DLMSDb db = new DLMSDb();

        // GET: Cars
        public ActionResult Index()
        {
            List<SelectListItem> manufacturer = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="Tata Motors",Value="Tata Motors"},
                new SelectListItem{Text="Maruti Suzuki",Value="Maruti Suzuki"},
                new SelectListItem{Text="Mahindra",Value="Mahindra"},
                new SelectListItem{Text="Ford",Value="Ford"},
                new SelectListItem{Text="Nissan",Value="Nissan"},
                new SelectListItem{Text="Renault",Value="Renault"},
                new SelectListItem{Text="Honda",Value="Honda"},
                new SelectListItem{Text="Volkswagen",Value="Volkswagen"},
                new SelectListItem{Text="Toyota",Value="Toyota"},
                new SelectListItem{Text="Hyundai",Value="Hyundai"},
                new SelectListItem{Text="Mercedes",Value="Mercedes"},
                new SelectListItem{Text="BMW",Value="BMV"},
                new SelectListItem{Text="Audi",Value="Audi"},
                new SelectListItem{Text="Porsche",Value="Porsche"},
                new SelectListItem{Text="Chevrolet",Value="Chevrolet"},
                new SelectListItem{Text="Jeep",Value="Jeep"},
                new SelectListItem{Text="MG",Value="MG"},
                new SelectListItem{Text="Kia",Value="Kia"},
            };
            ViewBag.car_manufacturer = manufacturer;
            List<SelectListItem> Airbag = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="1",Value="1"},
                new SelectListItem{Text="2",Value="2"},
                new SelectListItem{Text="3",Value="3"},
                new SelectListItem{Text="4",Value="4"},
                new SelectListItem{Text="5",Value="5"},
                new SelectListItem{Text="6",Value="6"},
            };
            ViewBag.airbag = Airbag;
            List<SelectListItem> Gear = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="Automatic",Value="Automatic"},
                new SelectListItem{Text="Manual",Value="Manual"},
                new SelectListItem{Text="Hybrid",Value="Hybrid"},
            };
            ViewBag.gear = Gear;
            List<SelectListItem> Car_type = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="New",Value="New"},
                new SelectListItem{Text="Used",Value="Used"},
            };
            ViewBag.car_type = Car_type;
            var car = db.Car.Include(c => c.Dealer);
            return View(car.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(String car_manufacturer,String car_type,String airbag,String gear)
        {
            List<SelectListItem> manufacturer = new List<SelectListItem>()
            {
                new SelectListItem{Text="Tata Motors",Value="Tata Motors"},
                new SelectListItem{Text="Maruti Suzuki",Value="Maruti Suzuki"},
                new SelectListItem{Text="Mahindra",Value="Mahindra"},
                new SelectListItem{Text="Ford",Value="Ford"},
                new SelectListItem{Text="Nissan",Value="Nissan"},
                new SelectListItem{Text="Renault",Value="Renault"},
                new SelectListItem{Text="Honda",Value="Honda"},
                new SelectListItem{Text="Volkswagen",Value="Volkswagen"},
                new SelectListItem{Text="Toyota",Value="Toyota"},
                new SelectListItem{Text="Hyundai",Value="Hyundai"},
                new SelectListItem{Text="Mercedes",Value="Mercedes"},
                new SelectListItem{Text="BMW",Value="BMV"},
                new SelectListItem{Text="Audi",Value="Audi"},
                new SelectListItem{Text="Porsche",Value="Porsche"},
                new SelectListItem{Text="Chevrolet",Value="Chevrolet"},
                new SelectListItem{Text="Jeep",Value="Jeep"},
                new SelectListItem{Text="MG",Value="MG"},
                new SelectListItem{Text="Kia",Value="Kia"},
            };
            ViewBag.car_manufacturer = manufacturer;
            List<SelectListItem> Airbag = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="1",Value="1"},
                new SelectListItem{Text="2",Value="2"},
                new SelectListItem{Text="3",Value="3"},
                new SelectListItem{Text="4",Value="4"},
                new SelectListItem{Text="5",Value="5"},
                new SelectListItem{Text="6",Value="6"},
            };
            ViewBag.airbag = Airbag;
            List<SelectListItem> Gear = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="Automatic",Value="Automatic"},
                new SelectListItem{Text="Manual",Value="Manual"},
                new SelectListItem{Text="Hybrid",Value="Hybrid"},
            };
            ViewBag.gear = Gear;
            List<SelectListItem> Car_type = new List<SelectListItem>()
            {
                new SelectListItem{Text="None",Value="None"},
                new SelectListItem{Text="New",Value="New"},
                new SelectListItem{Text="Used",Value="Used"},
            };
            ViewBag.car_type = Car_type;
            var car = db.Car.Include(c => c.Dealer);
            var car1 = car.Where(c1 => c1.car_manufacturer == car_manufacturer && c1.car_type == car_type && c1.airbag == airbag && c1.gear == gear);
            return View(car1);
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name");
            List<SelectListItem> manufacturer = new List<SelectListItem>()
            {
                new SelectListItem{Text="Car Manufacturer"},
                new SelectListItem{Text="Tata Motors",Value="Tata Motors"},
                new SelectListItem{Text="Maruti Suzuki",Value="Maruti Suzuki"},
                new SelectListItem{Text="Mahindra",Value="Mahindra"},
                new SelectListItem{Text="Ford",Value="Ford"},
                new SelectListItem{Text="Nissan",Value="Nissan"},
                new SelectListItem{Text="Renault",Value="Renault"},
                new SelectListItem{Text="Honda",Value="Honda"},
                new SelectListItem{Text="Volkswagen",Value="Volkswagen"},
                new SelectListItem{Text="Toyota",Value="Toyota"},
                new SelectListItem{Text="Hyundai",Value="Hyundai"},
                new SelectListItem{Text="Mercedes",Value="Mercedes"},
                new SelectListItem{Text="BMW",Value="BMV"},
                new SelectListItem{Text="Audi",Value="Audi"},
                new SelectListItem{Text="Porsche",Value="Porsche"},
                new SelectListItem{Text="Chevrolet",Value="Chevrolet"},
                new SelectListItem{Text="Jeep",Value="Jeep"},
                new SelectListItem{Text="MG",Value="MG"},
                new SelectListItem{Text="Kia",Value="Kia"},
            };
            ViewBag.car_manufacturer = manufacturer;
            List<SelectListItem> Airbag = new List<SelectListItem>()
            {
                new SelectListItem{Text="Airbag"},
                new SelectListItem{Text="1",Value="1"},
                new SelectListItem{Text="2",Value="2"},
                new SelectListItem{Text="3",Value="3"},
                new SelectListItem{Text="4",Value="4"},
                new SelectListItem{Text="5",Value="5"},
                new SelectListItem{Text="6",Value="6"},
            };
            ViewBag.airbag = Airbag;
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "c_id,car_manufacturer,car_model,car_type,price,power,fuel_tank,airbag,gear,mileage,ImageFile")] Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var car1 = new Car() {
                    d_id = Convert.ToInt32(Session["d_id"]),
                    car_manufacturer = car.car_manufacturer,
                    car_model = car.car_model,
                    car_type = car.car_type,
                    price = car.price,
                    power = car.power,
                    fuel_tank = car.fuel_tank,
                    airbag = car.airbag,
                    gear = car.gear,
                    mileage = car.mileage,
                    img = SaveToPhysicalLocation(car.ImageFile),
                };
                db.Car.Add(car1);
                
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting  
                            // the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                return RedirectToAction("Index");
            }
            List<SelectListItem> manufacturer = new List<SelectListItem>()
            {
                new SelectListItem{Text="Car Manufacturer"},
                new SelectListItem{Text="Tata Motors",Value="Tata Motors"},
                new SelectListItem{Text="Maruti Suzuki",Value="Maruti Suzuki"},
                new SelectListItem{Text="Mahindra",Value="Mahindra"},
                new SelectListItem{Text="Ford",Value="Ford"},
                new SelectListItem{Text="Nissan",Value="Nissan"},
                new SelectListItem{Text="Renault",Value="Renault"},
                new SelectListItem{Text="Honda",Value="Honda"},
                new SelectListItem{Text="Volkswagen",Value="Volkswagen"},
                new SelectListItem{Text="Toyota",Value="Toyota"},
                new SelectListItem{Text="Hyundai",Value="Hyundai"},
                new SelectListItem{Text="Mercedes",Value="Mercedes"},
                new SelectListItem{Text="BMW",Value="BMV"},
                new SelectListItem{Text="Audi",Value="Audi"},
                new SelectListItem{Text="Porsche",Value="Porsche"},
                new SelectListItem{Text="Chevrolet",Value="Chevrolet"},
                new SelectListItem{Text="Jeep",Value="Jeep"},
                new SelectListItem{Text="MG",Value="MG"},
                new SelectListItem{Text="Kia",Value="Kia"},
            };
            ViewBag.car_manufacturer = manufacturer;
            List<SelectListItem> Airbag = new List<SelectListItem>()
            {
                new SelectListItem{Text="Airbag"},
                new SelectListItem{Text="1",Value="1"},
                new SelectListItem{Text="2",Value="2"},
                new SelectListItem{Text="3",Value="3"},
                new SelectListItem{Text="4",Value="4"},
                new SelectListItem{Text="5",Value="5"},
                new SelectListItem{Text="6",Value="6"},
            };
            ViewBag.airbag = Airbag;
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", car.d_id);
            return View(car);
        }
        private string SaveToPhysicalLocation(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var newFileName = DateTime.Now.ToString("yyyy_MM_dd_mm_ss") + "_" + fileName;
                var path = Path.Combine(Server.MapPath("~/Car_Images"), newFileName);
                var relativePath = "~/Car_Images/"+newFileName;
                file.SaveAs(path);
                return relativePath;
            }
            return string.Empty;
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", car.d_id);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "c_id,d_id,car_manufacturer,car_model,car_type,price,power,fuel_tank,airbag,gear,mileage,img")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.d_id = new SelectList(db.Dealer, "d_id", "company_name", car.d_id);
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Car.Find(id);
            db.Car.Remove(car);
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

        
    }
}
