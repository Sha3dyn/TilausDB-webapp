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
    public class TilauksetController : Controller
    {
        TilausDBEntities db = new TilausDBEntities();

        // GET: Tilaukset
        public ActionResult Index()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            var tilaukset = db.Tilaukset.Include(s => s.Postitoimipaikat);
            
            return View(tilaukset.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tilaukset tilaukset = db.Tilaukset.Find(id);

            if (tilaukset == null) return HttpNotFound();

            var asiakkaat = db.Tilaukset.Include(s => s.Asiakkaat);

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);

            return View(tilaukset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausID, AsiakasID, Toimitusosoite, Postinumero, Postitoimipaikka, Tilauspvm, Toimituspvm")] Tilaukset tilaukset)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
                ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
                return RedirectToAction("Index");
            }

            return View(tilaukset);
        }

        public ActionResult Create()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            var asiakkaat = db.Tilaukset.Include(s => s.Asiakkaat);

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID, AsiakasID, Toimitusosoite, Postinumero, Postitoimipaikka, Tilauspvm, Toimituspvm")] Tilaukset tilaukset) //huom lisää postinumero (ja tietokanta hakee postitoimipaikan?)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
                ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
                return RedirectToAction("Index");
            }

            return View(tilaukset);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tilaukset tilaukset = db.Tilaukset.Find(id);

            if (tilaukset == null) return HttpNotFound();

            return View(tilaukset);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            Tilaukset tilaukset = db.Tilaukset.Find(id);

            db.Tilaukset.Remove(tilaukset);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}