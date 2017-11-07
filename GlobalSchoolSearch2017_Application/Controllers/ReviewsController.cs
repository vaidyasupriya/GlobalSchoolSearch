using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GlobalSchoolSearch2017_Application.Models;
using System.Collections;
using PagedList;

namespace GlobalSchoolSearch2017_Application.Controllers
{
    public class ReviewsController : Controller
    {
        private GlobalSchoolSearch2017_DatabaseEntities db = new GlobalSchoolSearch2017_DatabaseEntities();


        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.City).Include(r => r.Country).Include(r => r.School);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            Review newReview = new Review();
            newReview.Date = DateTime.Today;
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName");
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName");
            ViewBag.SchoolName = new SelectList(db.Schools, "SchoolName", "SchoolName");
            return View(newReview);
        }


        public ActionResult CreateForSpecificSchool(string CountryName, string CityName, string SchoolName)
      {
            Review newReview = new Review();
            newReview.Date = DateTime.Today;
            newReview.CityName = CityName;
            newReview.CountryName = CountryName;
            newReview.SchoolName = SchoolName;

            return View(newReview);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForSpecificSchool([Bind(Include = "Id,SchoolName,CityName,CountryName,ReviewText,Rating,Date")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                School school = UpdateRatingForSchool(review.SchoolName, review.Rating);
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName", review.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", review.CountryName);
            ViewBag.SchoolName = new SelectList(db.Schools, "SchoolName", "SchoolName", review.SchoolName);
            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SchoolName,CityName,CountryName,ReviewText,Rating, Date")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                School school = UpdateRatingForSchool(review.SchoolName, review.Rating);
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName", review.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", review.CountryName);
            ViewBag.SchoolName = new SelectList(db.Schools, "SchoolName", "SchoolName", review.SchoolName);
            return View(review);
        }

        [Authorize]
        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CountryName", review.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", review.CountryName);
            ViewBag.SchoolName = new SelectList(db.Schools, "SchoolName", "Address", review.SchoolName);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SchoolName,CityName,CountryName,ReviewText,Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                UpdateRatingForSchool(review.SchoolName, review.Rating);
                return RedirectToAction("Index");
            }

            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CountryName", review.CityName);
            ViewBag.CountryName = new SelectList(db.Countries, "CountryName", "CountryName", review.CountryName);
            ViewBag.SchoolName = new SelectList(db.Schools, "SchoolName", "Address", review.SchoolName);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

        public JsonResult GetSchools(string City)
        {
            string selectedCityName = Convert.ToString(City);
            List<string> SchoolList = new List<string>();

            // var tempCityList = from c in db.Cities where c.CountryName.Equals(selectedCountryName) select c;
            IQueryable<School> tempSchoolList = db.Schools.Where(c => c.CityName == selectedCityName);

            SchoolList.Add("Select School");
            foreach (var i in tempSchoolList)
            {
                SchoolList.Add(i.SchoolName);
            }
            return Json(SchoolList);
        }

        public School UpdateRatingForSchool(string SchoolName, double rating)
        {

            School currentSchool = db.Schools.Where(m => m.SchoolName == SchoolName).SingleOrDefault();
            if (currentSchool.Rating == null)
            {
                currentSchool.Rating = rating;
                currentSchool.Rating = currentSchool.Rating * 0.8;
                if (currentSchool.Authorizer_Email != "vaidya.supriya@gmail.com")
                {
                    currentSchool.Rating += 0.5;
                }

                if ((currentSchool.Date_of_Updatation - DateTime.Today.Date).TotalDays <= 90)
                {
                    currentSchool.Rating += 0.5;
                }
            }
            else
            {
                IList<Review> reviews = db.Reviews.Where(m => m.SchoolName == SchoolName).ToList();
                //Initialize total rating to rating from ew review
                double totalRating = rating;

                //Add all previous review ratings
                foreach (Review review in reviews)
                {
                    totalRating += review.Rating;
                }

                //Take average 
                double averageRating = totalRating / (reviews.Count + 1);

                // Scale original average rating from 5 to 4
                double scaledAverageRating = 0;
                scaledAverageRating = averageRating * 0.8;

                // If school is not created by "vaidya.supriya@gmail.com" / (admin)  but is created by school authorities itself, add 0.5 in rating

                if (currentSchool.Authorizer_Email != "vaidya.supriya@gmail.com")
                {
                    scaledAverageRating += 0.5;
                }

                if ((currentSchool.Date_of_Updatation - DateTime.Today.Date).TotalDays <= 90)
                {
                    scaledAverageRating += 0.5;
                }

                currentSchool.Rating = scaledAverageRating;
            }

            // db.SaveChanges();
            return currentSchool;

        }

        public ActionResult ReviewsPerSchool(string SchoolName)
        {
            ViewBag.SchoolName = SchoolName;
            School school = db.Schools.Where(s => s.SchoolName == SchoolName).SingleOrDefault();
            return View(school);
        }
    }
}
