using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserApplication.Models;

namespace UserApplication.Controllers
{

    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// GET:show Registration form to New User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registration()
        {

            //Creating object of UserViewModel
            UserViewModel model = new UserViewModel();


            List<Country> countryList = new List<Country>();
            List<State> stateList = new List<State>();
            List<City> cityList = new List<City>();
            List<Course> courseList = new List<Course>();
            List<Role> roleList = new List<Role>();

            var tempCountryList = db.Countries.ToList();
            var tempStateList = db.States.ToList();
            var tempCityList = db.Cities.ToList();
            var tempCourseList = db.Courses.ToList();
            var tempRoleList = db.Roles.Where(u => u.RoleId != 1 && u.RoleId != 2).ToList();

            model.Countries = tempCountryList;
            model.States = tempStateList;
            model.Cities = tempCityList;
            model.Courses = tempCourseList;
            model.Roles = tempRoleList;
            return View(model);
        }

        /// <summary>
        /// To post the values of the registration form to the database.
        /// </summary>
        /// <param name="objUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Registration(UserViewModel objUserViewModel)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Registration");
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {//Raw data sent to address table.
                    Address objAddress = new Address
                    {
                        AddressLine1 = objUserViewModel.AddressLine1,
                        AddressLine2 = objUserViewModel.AddressLine2,
                        CountryId = objUserViewModel.CountryId,
                        StateId = objUserViewModel.StateId,
                        CityId = objUserViewModel.CityId,
                        Zipcode = objUserViewModel.Zipcode,


                    };

                    db.Addresses.Add(objAddress);
                    db.SaveChanges();
                    //Raw data sent for IsEmailVerified property through ViewModel object.
                    objUserViewModel.IsEmailVerified = "Yes";
                    //try to insert user details of registration form in User table of database.
                    User objUser = new User
                    {
                        UserId = objUserViewModel.UserId,
                        FirstName = objUserViewModel.FirstName,
                        LastName = objUserViewModel.LastName,
                        Gender = objUserViewModel.Gender,
                        DOB = objUserViewModel.DOB,
                        Hobbies = objUserViewModel.Hobbies,
                        Email = objUserViewModel.Email,
                        IsEmailVerified = objUserViewModel.IsEmailVerified,
                        Password = objUserViewModel.Password,
                        ConfirmPassword = objUserViewModel.ConfirmPassword,
                        //AddressLine1 = objUserViewModel.AddressLine1,
                        //AddressLine2 = objUserViewModel.AddressLine2,
                        IsActive = objUserViewModel.IsActive,
                        CourseId = objUserViewModel.CourseId,
                        

                        RoleId = objUserViewModel.RoleId,
                        // Adding addresId 
                        AddressId = objAddress.AddressId,
                        DateCreated = DateTime.Now,
                        //Done for testing purpose.
                        DateModified = DateTime.Now
                    };

                    db.User.Add(objUser);
                    db.SaveChanges();

                    //RoleId for the respective UserId gets saved in database.
                    UserInRole objUserInRole = new UserInRole
                    {
                        RoleId = objUserViewModel.RoleId,
                        UserId = objUser.UserId
                    };
                    db.UserInRole.Add(objUserInRole);
                    db.SaveChanges();
                    //Everything looks fine,so save the data permanently.
                    transaction.Commit();

                    ViewBag.ResultMessage = objUserViewModel.FirstName + "" + objUserViewModel.LastName + "" + "is successfully registered.";
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







        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {

            var LoginDetails = db.User.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (LoginDetails != null)
            {
                Session["UserId"] = LoginDetails.UserId.ToString();
                Session["UserName"] = LoginDetails.Email.ToString();
                //Session["UserDetails"] = LoginDetails;
                if (LoginDetails.RoleId == 1)
                {

                    return RedirectToAction("GetAllUsers", "SuperAdmin");
                }
                else if (LoginDetails.RoleId == 2)
                {
                    return RedirectToAction("GetAllUsers", "Admin");
                }
                else if (LoginDetails.RoleId == 3)
                {
                    Session["User"] = LoginDetails;
                    return RedirectToAction("TeacherHomePage1", "Teacher");

                    //return RedirectToAction("TeacherHomePage1", "Teacher", new { id = ((User)Session["UserDetails"]).UserId });
                }
                else
                {
                    Session["User"] = LoginDetails;
                    return RedirectToAction("StudentHomePage1", "Student"/*, new { id = LoginDetails.UserId }*/);
                }
            }
            return View("Login");
        }
        public ActionResult LogOut()
        {
            Session["login"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }



    }
}





