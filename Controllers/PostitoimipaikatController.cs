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
    public class PostitoimipaikatController : Controller
    {
        TilausDBEntities db = new TilausDBEntities();

        // GET: Postitoimipaikat
        public ActionResult Index()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            List<Postitoimipaikat> model = db.Postitoimipaikat.ToList();

            db.Dispose();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Postitoimipaikat pstmpk = db.Postitoimipaikat.Find((object)id);

            if (pstmpk == null) return HttpNotFound();

            return View(pstmpk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Postinumero, Postitoimipaikka")] Postitoimipaikat pstmpk)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Entry(pstmpk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pstmpk);
        }

        public ActionResult Create()
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Postinumero, Postitoimipaikka")] Postitoimipaikat pstmpk)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (ModelState.IsValid)
            {
                db.Postitoimipaikat.Add(pstmpk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pstmpk);
        }

        public ActionResult Delete(string id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Postitoimipaikat pstmpk = db.Postitoimipaikat.Find((object)id);

            if (pstmpk == null) return HttpNotFound();

            return View(pstmpk);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["Kayttajatunnus"] == null) return RedirectToAction("login", "home");

            Postitoimipaikat pstmpk = db.Postitoimipaikat.Find((object)id);

            db.Postitoimipaikat.Remove(pstmpk);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}