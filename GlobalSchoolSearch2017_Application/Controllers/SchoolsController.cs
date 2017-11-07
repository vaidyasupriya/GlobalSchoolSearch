using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GlobalSchoolSearch2017_Application.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace GlobalSchoolSearch2017_Application.Controllers
{
    public class SchoolsController : Controller
    {
        private GlobalSchoolSearch2017_DatabaseEntities db = new GlobalSchoolSearch2017_DatabaseEntities();

        // GET: Schools
        public ActionResult Index(string cityName, string countryName, string sortOrder, string currentFilter, string searchString, int? page)
        {
            //var schools = db.Schools.Include(s => s.City).Include(s => s.Country);            
            //return View(schools.ToList());

            ViewBag.countryName = countryName;
            ViewBag.cityName = cityName;
            ViewBag.searchString = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SchoolSortParm = String.IsNullOrEmpty(sortOrder) ? "school_desc" : "";
            ViewBag.CitySortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.CountrySortParm = sortOrder == "Country" ? "country-desc" : "Country";
            ViewBag.ExamSortParm = sortOrder == "Exam" ? "exam_desc" : "Exam";
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "rating_desc" : "Rating";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var schools = db.Schools.Include(s => s.City).Include(s => s.Country);

            if (Request.IsAuthenticated)
            {
                string userName = User.Identity.GetUserName().ToString();
                schools = schools.Where(s => s.Authorizer_Email.Equals(userName));
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                schools = schools.Where(s => s.SchoolName.Contains(searchString)
                    || s.CityName.Contains(searchString)
                    || s.CountryName.Contains(searchString)
                    || s.Address.Contains(searchString)
                    || s.Examination_Board.Contains(searchString)
                    || s.Type_of_School.Contains(searchString)
                    || s.Postcode.Contains(searchString)
                    || s.Additional_Information_.Contains(searchString)
                    );
            }

            switch (sortOrder)
            {
                case "school_desc":
                    schools = schools.OrderByDescending(s => s.SchoolName);
                    break;
                case "City":
                    schools = schools.OrderBy(s => s.CityName);
                    break;
                case "city_desc":
                    schools = schools.OrderByDescending(s => s.CityName);
                    break;
                case "Country":
                    schools = schools.OrderBy(s => s.CountryName);
                    break;
                case "country_desc":
                    schools = schools.OrderByDescending(s => s.CountryName);
                    break;
                case "Exam":
                    schools = schools.OrderBy(s => s.Examination_Board);
                    break;
                case "exam_desc":
                    schools = schools.OrderByDescending(s => s.Examination_Board);
                    break;
                case "Rating":
                    schools = schools.OrderByDescending(s => s.Rating);
                    break;
                case "rating_desc":
                    schools = schools.OrderBy(s => s.Rating);
                    break;
                default:
                    schools = schools.OrderBy(s => s.SchoolName);
                    break;

            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(schools.ToPagedList(pageNumber, pageSize));
        }

        // GET: Schools/Details/5
        public ActionResult Details(string schoolName)
        {
            if (schoolName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(schoolName);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        [Authorize]
        // GET: Schools/Create
        public ActionResult Create()
        {
            School newSchool = new School();
            newSchool.Date_of_Updatation = DateTime.Today;
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName");
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName");
            
            return View(newSchool);
        }

        // POST: Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SchoolId,SchoolName,Address,Postcode,CityName,CountryName,Contact_No_,Alternate_Contact_No_,Email,Website,Examination_Board,Type_of_School,Admission_Criteria,Admission_Start_Date,Application_Form,Submission_Date,Additional_Information_,Authorizer_Email,Date_Of_Updation")] School school)
        {
            if (ModelState.IsValid)
            {
                db.Schools.Add(school);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName", school.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", school.CountryName);
            return View(school);
        }


        // GET: Schools/Edit/5
        [Authorize]
        public ActionResult Edit(string schoolName)
        {
            if (schoolName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(schoolName);
            if (school == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName", school.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", school.CountryName);
            return View(school);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolId,SchoolName,Address,Postcode,CityName,CountryName,Contact_No_,Alternate_Contact_No_,Email,Website,Examination_Board,Type_of_School,Admission_Criteria,Admission_Start_Date,Application_Form,Submission_Date,Additional_Information_,Authorizer_Email")] School school)
        {
            if (ModelState.IsValid)
            {
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName", school.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", school.CountryName);
            return View(school);
        }

        // GET: Schools/Delete/5
        [Authorize]
        public ActionResult Delete(string schoolName)
        {
            string userName = User.Identity.GetUserName().ToString();
            if (schoolName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(schoolName);
            if (school == null)
            {
                return HttpNotFound();
            }

            //Only Administrator can edit or delete school entry.
            //Logged in user can edit of delete only those school entries for which he/she is authorized.
            if (school.Authorizer_Email.Equals(userName))
            {
                return View(school);
            }
            else if (User.IsInRole("Administrator"))
            {
                return View(school);
            }
            else
            {

                return HttpNotFound("You are not authorized to delete this school entry");
            }

            //return View(school);
        }

        // POST: Schools/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string schoolName)
        {
            School school = db.Schools.Find(schoolName);
            db.Schools.Remove(school);
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

        public JsonResult GetCities(string Country)
        {
            string selectedCountryName = Convert.ToString(Country);
            List<string> CityList = new List<string>();

           // var tempCityList = from c in db.Cities where c.CountryName.Equals(selectedCountryName) select c;
            IQueryable<City> tempCityList = db.Cities.Where(c => c.CountryName == selectedCountryName);

            CityList.Add("Select City");
            foreach (var i in tempCityList)
            {
                CityList.Add(i.CityName);
            }
            return Json(CityList);
        }
    }
}
