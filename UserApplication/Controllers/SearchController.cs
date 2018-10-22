using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserApplication.Models;

namespace UserApplication.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: SearchRecord
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchRecord()
        {
            //Creating object of UserViewModel
            SearchRecord model = new SearchRecord();

            List<Country> countryList = new List<Country>();
            List<State> stateList = new List<State>();
            List<City> cityList = new List<City>();
            List<Course> courseList = new List<Course>();
            List<Role> roleList = new List<Role>();

            var tempcountryList = db.Countries.ToList();
            var tempstateList = db.States.ToList();
            var tempcityList = db.Cities.ToList();
            var tempCourseList = db.Courses.ToList();
            var tempRoleList = db.Roles.ToList();

            model.Countries = tempcountryList;
            model.States = tempstateList;
            model.Cities = tempcityList;
            model.Courses = tempCourseList;
            model.Roles = tempRoleList;
            return View(model);
        }
        [HttpPost]
        public ActionResult SearchRecord(SearchRecord objSearchRecord)
        {

            if (!ModelState.IsValid)
            {
                return View(objSearchRecord);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {//Raw data sent to address table.
                    Address objAddress = new Address
                    {
                        AddressLine1 = objSearchRecord.AddressLine1,
                        AddressLine2 = objSearchRecord.AddressLine2,
                        CityId = objSearchRecord.CityId,
                        CountryId = objSearchRecord.CountryId,
                        Zipcode = objSearchRecord.Zipcode,
                        StateId = objSearchRecord.StateId,

                    };

                    db.Addresses.Add(objAddress);
                    db.SaveChanges();
                    //Raw data sent for IsEmailVerified property through ViewModel object.
                    objSearchRecord.IsEmailVerified = "Yes";
                    //try to insert user details of registration form in User table of database.
                    User objUser = new User
                    {
                        UserId = objSearchRecord.UserId,
                        FirstName = objSearchRecord.FirstName,
                        LastName = objSearchRecord.LastName,
                        Gender = objSearchRecord.Gender,
                        DOB = objSearchRecord.DOB,
                        Hobbies = objSearchRecord.Hobbies,
                        Email = objSearchRecord.Email,
                        IsEmailVerified = objSearchRecord.IsEmailVerified,
                        Password = objSearchRecord.Password,
                        ConfirmPassword = objSearchRecord.ConfirmPassword,
                        //AddressLine1 = objUserViewModel.AddressLine1,
                        //AddressLine2 = objUserViewModel.AddressLine2,
                        IsActive = objSearchRecord.IsActive,
                        CourseId = objSearchRecord.CourseId,
                        RoleId = objSearchRecord.RoleId,
                        // Adding addresId 
                        AddressId = objAddress.AddressId,
                        DateCreated = DateTime.Now,
                        //Done for testing purpose.
                        DateModified = DateTime.Now
                    };

                    db.Users.Add(objUser);
                    db.SaveChanges();

                    //RoleId for the respective UserId gets saved in database.
                    UserInRole objUserInRole = new UserInRole
                    {
                        RoleId = objSearchRecord.RoleId,
                        UserId = objUser.UserId
                    };
                    db.UserInRoles.Add(objUserInRole);
                    db.SaveChanges();
                    //Everything looks fine,so save the data permanently.
                    transaction.Commit();

                    ViewBag.ResultMessage = objSearchRecord.FirstName + "" + objSearchRecord.LastName + "" + "is successfully registered.";
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    //throw ex;
                    //roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";

                }
            }
            return RedirectToAction("Login", "User");

        }
        public JsonResult getState(int Id)
        {
            var states = db.States.Where(x => x.CountryId == Id).ToList();
            List<SelectListItem> stateList = new List<SelectListItem>();

            stateList.Add(new SelectListItem { Text = "", Value = "0" });
            if (states != null)
            {
                foreach (var x in states)
                {
                    stateList.Add(new SelectListItem { Text = x.StateName, Value = x.StateId.ToString() });

                }
            }
            return Json(new SelectList(stateList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getCity(int id)
        {
            var cities = db.Cities.Where(x => x.StateId == id).ToList();
            List<SelectListItem> cityList = new List<SelectListItem>();
            cityList.Add(new SelectListItem { Text = "", Value = "0" });
            if (cities != null)
            {
                foreach (var x in cities)
                {
                    cityList.Add(new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() });
                }
            }
            return Json(new SelectList(cityList, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
    }
}