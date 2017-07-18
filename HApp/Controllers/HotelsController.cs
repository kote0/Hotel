using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HApp;

namespace HApp.Controllers
{
    public class HotelsController : Controller
    {
        private hotelEntities db = new hotelEntities();
        
        // GET: Hotels
        public ActionResult Index()
        {
            return View(db.Hotels.ToList());
        }
        public ActionResult Check1()
        {
            return View(db.Hotels.ToList());
        }

        public ActionResult Index2()
        {
            return View(db.Hotels.ToList());
        }
        public ActionResult Details2(string name)  
        {
            Hotel hotel;
            hotel = db.Hotels.Find(name);
            return View(hotel);
        }

        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "catalogHotelsID,nameH,country,town,type,countRooms,star,distance,deck")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "catalogHotelsID,nameH,country,town,type,countRooms,star,distance,deck")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
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
        DateTime da;
       [HttpPost]
        public ActionResult Index2(string hotel ="",string country="", string town="", string pr1="", string pr2="", string distance = "", 
            int? star1=0, int? star2 = 0, int? star3= 0, int? star4 = 0, int? star5 = 0, 
            string e="", string t ="", string b= "", string z= "", string d="")
        {
            if (d!= "") da = Convert.ToDateTime(d);
            string s = "";
                s += (e != "") ? " or dbo.Comfort.nameComfort='Экскурсии'" : "";
                s += (t != "") ? " or dbo.Comfort.nameComfort='Такси'" : "";
                s += (b != "") ? " or dbo.Comfort.nameComfort='Оплата'" : "";
                s += (z != "") ? " or dbo.Comfort.nameComfort='Завтрак'" : "";
                s += (pr1 != "") ? " or dbo.CatalogRooms.price>=" + Convert.ToDecimal(pr1) : "";
                s += (pr2 != "") ? " or dbo.CatalogRooms.price<=" + Convert.ToDecimal(pr2) : "";
                s += (distance != "") ? " or  dbo.Hotel.distance<=" + Convert.ToDouble(distance) : "";
                s += (hotel != "") ? " or  dbo.Hotel.nameH like '%" + hotel + "%'" : "";
                s += (country != "Выберите ...") ? " or dbo.Hotel.country='" + country + "'" : "";
                s += (town != "Выберите ...") ? " or dbo.Hotel.town='" + town + "'" : "";
                s += (star1 != 0) ? " or dbo.Hotel.star=" + star1 : "";
                s += (star2 != 0) ? " or dbo.Hotel.star=" + star2 : "";
                s += (star3 != 0) ? " or dbo.Hotel.star=" + star3 : "";
                s += (star4 != 0) ? " or dbo.Hotel.star=" + star4 : "";
                s += (star5 != 0) ? " or dbo.Hotel.star=" + star5 : "";
            
                //проверка
                s = (s!="") ? " and ("+s.Substring(3)+")" : "";

            var res = db.Hotels.SqlQuery("SELECT distinct dbo.Hotel.nameH, dbo.Hotel.catalogHotelsID, dbo.Hotel.country," +
                " dbo.Hotel.town, dbo.Hotel.star, dbo.Hotel.deck, dbo.Hotel.distance," +
                " dbo.Hotel.type, dbo.Hotel.countRooms " +
                " FROM  dbo.CatalogRooms,  dbo.Hotel, dbo.Comfort " +
                " WHERE dbo.CatalogRooms.catalogHotelsID = dbo.Hotel.catalogHotelsID and"+
                " dbo.Comfort.catalogHotelsID=dbo.Hotel.catalogHotelsID "+
                 s).ToList();

            return View(res);
        }
        public ActionResult Description(int i=1) {
            Hotel hotel = db.Hotels.Find(i);
            ViewBag.index = i;
            return View(hotel);
        }
        [ChildActionOnly]
        public ActionResult ParticialDescription(int id)
        {
            return PartialView("_ParticialDescription", db.CatalogRooms.Where(i => i.catalogHotelsID == id).ToList());
        }
        public ActionResult ParticialIndex2(int id)
        {
            return PartialView("_ParticialIndex2", db.Comforts.Where(i => i.catalogHotelsID == id).ToList());
        }
        public ActionResult Reservation(int roomsID = 1)
        {
            ViewBag.dateRoom = da;
            CatalogRoom Room = db.CatalogRooms.Find(roomsID);
            return View(Room);
        }
        [HttpPost]
        public ActionResult Reservation()
        {
            return RedirectToAction("Index2");
        }
        public ActionResult Help()
        {
            return View();
        }

    }
}
