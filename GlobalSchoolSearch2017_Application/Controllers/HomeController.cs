using GlobalSchoolSearch2017_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalSchoolSearch2017_Application.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private GlobalSchoolSearch2017_DatabaseEntities db = new GlobalSchoolSearch2017_DatabaseEntities();

        public ActionResult Index()
        {
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName");
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Parents()
        {
            return View();
        }

        public ActionResult SchoolAuthorities()
        {
            return View();
        }
    }
}