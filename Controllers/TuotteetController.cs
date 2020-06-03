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
    public class TuotteetController : Controller
    {
        // Luodaan tietokantaolio db
        TilausDBEntities db = new TilausDBEntities();

        // GET: Tuotteet
        public ActionResult Index()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            // Luodaan malli, joka tuottaa Tuotteet taulun datasta listan
            List<Tuotteet> model = db.Tuotteet.ToList();

            // Vapautetaan resursseja poistamalla tietokantaolio
            db.Dispose();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tuotteet tuotteet = db.Tuotteet.Find(id);

            if (tuotteet == null) return HttpNotFound();

            return View(tuotteet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TuoteID, Nimi, Ahinta, Kuvalinkki")] Tuotteet tuotteet)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Entry(tuotteet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tuotteet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TuoteID, Nimi, Ahinta, Kuvalinkki")] Tuotteet tuotteet)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Tuotteet.Add(tuotteet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tuotteet);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tuotteet tuotteet = db.Tuotteet.Find(id);

            if (tuotteet == null) return HttpNotFound();

            return View(tuotteet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            Tuotteet tuotteet = db.Tuotteet.Find(id);

            db.Tuotteet.Remove(tuotteet);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}