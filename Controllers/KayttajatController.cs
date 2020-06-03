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
    public class KayttajatController : Controller
    {
        TilausDBEntities db = new TilausDBEntities();

        // GET: Kayttajat
        public ActionResult Index()
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            List<Kayttajat> model = db.Kayttajat.ToList();

            db.Dispose();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Kayttajat kayttajat = db.Kayttajat.Find(id);

            if (kayttajat == null) return HttpNotFound();

            return View(kayttajat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KayttajaId, Kayttajatunnus, Salasana")] Kayttajat kayttajat)
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Entry(kayttajat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kayttajat);
        }

        public ActionResult Create()
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Kayttajatunnus, Salasana")] Kayttajat kayttajat)
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Kayttajat.Add(kayttajat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kayttajat);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Kayttajat kayttajat = db.Kayttajat.Find(id);

            if (kayttajat == null) return HttpNotFound();

            return View(kayttajat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (Session["Kayttajatunnus"] == null || Session["Kayttajatunnus"].ToString() != "admin") return RedirectToAction("login", "home");

            Kayttajat kayttajat = db.Kayttajat.Find(id);

            db.Kayttajat.Remove(kayttajat);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}