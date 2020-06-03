using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppTilausDB.Models;

namespace WebAppTilausDB.Controllers
{
    public class TilausrivitController : Controller
    {
        TilausDBEntities db = new TilausDBEntities();

        // GET: Asiakkaat
        public ActionResult Index()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            List<Tilausrivit> model = db.Tilausrivit.ToList();
            db.Dispose();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tilausrivit tilaus = db.Tilausrivit.Find(id);

            if (tilaus == null) return HttpNotFound();

            var tuotteet = db.Tilausrivit.Include(s => s.Tuotteet);

            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi", tilaus.TuoteID);
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID", tilaus.TilausID);

            return View(tilaus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausriviID,TilausID,TuoteID,Maara,Ahinta")] Tilausrivit tilaus)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Entry(tilaus).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Tuote = new SelectList(db.Tuotteet, "TuoteID", "Nimi", tilaus.TuoteID);
                ViewBag.Tilaus = new SelectList(db.Tilaukset, "TilausID", "TilausID", tilaus.TilausID);
                return RedirectToAction("Index");
            }

            return View(tilaus);
        }

        public ActionResult Create()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            var tuotteet = db.Tilausrivit.Include(s => s.Tuotteet);

            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi");
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausriviID,TilausID,TuoteID,Maara,Ahinta")] Tilausrivit tilaus) 
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Tilausrivit.Add(tilaus);
                db.SaveChanges();
                ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi", tilaus.TuoteID);
                ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID", tilaus.TilausID);
                return RedirectToAction("Index");
            }

            return View(tilaus);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tilausrivit tilaus = db.Tilausrivit.Find(id);

            if (tilaus == null) return HttpNotFound();

            return View(tilaus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            Tilausrivit tilaus = db.Tilausrivit.Find(id);

            db.Tilausrivit.Remove(tilaus);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}