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
    public class AsiakkaatController : Controller
    {
        TilausDBEntities db = new TilausDBEntities();

        // GET: Asiakkaat
        public ActionResult Index()
        {
            if(Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            //Tämä joinaa postitoimipaikat taulun asiakkaat tauluun
            var asiakkaat = db.Asiakkaat.Include(s => s.Postitoimipaikat);

            return View(asiakkaat.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);

            if (asiakkaat == null) return HttpNotFound();

            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", asiakkaat.Postinumero);

            return View(asiakkaat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AsiakasID,Nimi,Osoite,Postinumero")] Asiakkaat asiakkaat) 
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Entry(asiakkaat).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", asiakkaat.Postinumero);
                return RedirectToAction("Index");
            }

            return View(asiakkaat);
        }

        public ActionResult Create()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsiakasID,Nimi,Osoite,Postinumero")] Asiakkaat asiakkaat) 
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakkaat);
                db.SaveChanges();
                ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
                return RedirectToAction("Index");
            }

            return View(asiakkaat);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);

            if (asiakkaat == null) return HttpNotFound();

            return View(asiakkaat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            Asiakkaat asiakkaat = db.Asiakkaat.Find(id);

            db.Asiakkaat.Remove(asiakkaat);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}