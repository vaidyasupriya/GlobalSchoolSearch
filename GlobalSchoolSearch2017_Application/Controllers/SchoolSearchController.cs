using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlobalSchoolSearch2017_Application.Models;
using PagedList;

namespace GlobalSchoolSearch2017_Application.Controllers
{
    public class SchoolSearchController : Controller
    {
        private GlobalSchoolSearch2017_DatabaseEntities db = new GlobalSchoolSearch2017_DatabaseEntities();

        // GET: SchoolSearch
        public ActionResult SchoolSearch()
        {
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName");
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName");
            return View();
        }

        public ActionResult Search(string cityName, string countryName, string sortOrder, string searchString, string schoolSearchString, string currentFilter, int? page)
        {
            ViewBag.countryName = countryName;
            ViewBag.cityName = cityName;
            ViewBag.searchString = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SchoolSortParm = String.IsNullOrEmpty(sortOrder) ? "school_desc" : "";
            ViewBag.CitySortParm = sortOrder == "City" ? "city_desc" : "City";
            ViewBag.CountrySortParm = sortOrder == "Country" ? "country_desc" : "Country";
            ViewBag.ExamSortParm = sortOrder == "Exam" ? "exam_desc" : "Exam";
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "rating_desc" : "Rating";


            if (schoolSearchString != null)
            {
                page = 1;
            }
            else
            {
                schoolSearchString = currentFilter;
            }

            ViewBag.CurrentFilter = schoolSearchString;

            var schools = db.Schools.Include(s => s.City).Include(s => s.Country);

            if (!String.IsNullOrEmpty(searchString))
            {
                schools = schools.Where(s => s.CityName.Contains(searchString));
            }
            else
            {
                schools = schools.Where(s => s.CountryName.Equals(countryName)).Where(s => s.CityName.Equals(cityName));
            }

            ViewBag.SchoolsNotFound = false;
            if (schools.Count() < 1)
            {
                //ViewBag.DisplayMessage = " No schools found!! Enter the name of big city nearby !!";
                ViewBag.SchoolsNotFound = true;
            }

            //used for Marquee in view
            if (schools.Count() >= 1)
            {
                ViewBag.currentCity = schools.FirstOrDefault().CityName;
            }

            if (!String.IsNullOrEmpty(schoolSearchString))
            {
                schools = schools.Where(s => s.SchoolName.Contains(schoolSearchString)
                                             || s.CityName.Contains(schoolSearchString)
                                             || s.CountryName.Contains(schoolSearchString)
                                             || s.Address.Contains(schoolSearchString)
                                             || s.Postcode.Contains(schoolSearchString)
                                             || s.Examination_Board.Contains(schoolSearchString)
                                             || s.Type_of_School.Contains(schoolSearchString)
                                             || s.Additional_Information_.Contains(schoolSearchString)
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

            List<School> TopSchools = schools.OrderByDescending(s => s.Rating).ToList();
            ViewBag.TopSchools = TopSchools.Take(5);

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(schools.ToPagedList(pageNumber, pageSize));

        }

        // GET: SchoolSearch/Details/5
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



        // GET: SchoolSearch/Edit/5
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
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CountryName", school.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", school.CountryName);
            return View(school);
        }

        // POST: SchoolSearch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CountryName", school.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", school.CountryName);
            return View(school);
        }



        public JsonResult getCities(string Country)
        {
            string selectedCountryName = Convert.ToString(Country);
            List<string> CityList = new List<string>();

            var tempCityList = from c in db.Cities where c.CountryName.Equals(selectedCountryName) select c;
            foreach (var i in tempCityList)
            {
                CityList.Add(i.CityName);
            }
            return Json(CityList);
        }
    }
}
