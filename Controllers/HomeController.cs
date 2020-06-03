using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTilausDB.Models;

namespace WebAppTilausDB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Helppoa hallinnointia, tehokasta työskentelyä - parasta palvelua!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ota yhteyttä";

            return View();
        }

        public ActionResult Map()
        {
            ViewBag.Message = "Saapumisohjeet";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Kayttajat LoginModel)
        {
            TilausDBEntities db = new TilausDBEntities();

            //Haetaan käyttäjän / Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ-kyselyllä
            var LoggedUser = db.Kayttajat.SingleOrDefault(x => x.Kayttajatunnus == LoginModel.Kayttajatunnus && x.Salasana == LoginModel.Salasana);

            if(LoggedUser != null)
            {
                ViewBag.LoginMessage = "Sisäänkirjautuminen onnistui";
                ViewBag.LoggedStatus = "In";
                Session["Kayttajatunnus"] = LoggedUser.Kayttajatunnus;

                return RedirectToAction("Index", "Home"); // Tässä määritellään, mihin onnistunut kirjautuminen johtaa --> Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Sisäänkirjautuminen epäonnistui";
                ViewBag.LoggedStatus = "Out";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana";

                return View("Login", LoginModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";

            return RedirectToAction("Index", "Home"); // Uloskirjautuminen ohjaa pääsivulle
        }
    }

}