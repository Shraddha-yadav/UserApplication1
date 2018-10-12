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
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            //Country_Bind();
            return View(db.User.ToList());
        }


        // show Registration form to New User
        [HttpGet]
        public ActionResult Registration()
        {
            //Creating object of UserViewModel
            UserViewModel model = new UserViewModel();

            //Query for getting Course dropdown from Database
            var courseList = db.Courses.Select(x => new CourseModel
            {
                CourseName = x.CourseName,
                CourseId = x.CourseId
            }).ToList();

            //query to get the role dropdown from database.
            var roleList = db.Roles.Select(x => new RoleModel
            {
                RoleName = x.RoleName,
                RoleId = x.RoleId
            }).ToList();
            //sending data in roleList and courseList to Roles and Courses properties of ViewModel.  
            model.Roles = roleList;
            model.Courses = courseList;
            //to get country dropdown from database.
            var countryList = db.Countries.Select(x => new CountryModel
            {
                CountryName = x.CountryName,
                CountryId = x.CountryId
            }).ToList();
            //seding countrie's data to ViewModel's property, Countries.
            model.Countries = countryList;


            //to get state dropdown from database.
            var stateList = db.States.Select(x => new StateModel
            {
                StateId = x.StateId,
                StateName = x.StateName
            }
           ).ToList();
            //send state's data to ViewModel's property,States.
            model.States = stateList;


            //to get city dropdown from database.
            var cityList = db.Cities.Select(x => new CityModel
            {
                CityName = x.CityName,
                CityId = x.CityId
            }).ToList();

            //send cities data to ViewModel's property,Cities.
            model.Cities = cityList;


            //return object of ViewModel in the view.
            return View(model);
        }
        //To post the values of the registration form to the database.
        [HttpPost]
        public ActionResult Registration(UserViewModel objUserViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(objUserViewModel);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {//Raw data sent to address table.
                    Address objAddress = new Address
                    {
                        AddressLine1 = objUserViewModel.AddressLine1,
                        AddressLine2 = objUserViewModel.AddressLine2,
                        CityId = objUserViewModel.CityId,
                        CountryId = objUserViewModel.CountryId,
                        Zipcode = objUserViewModel.Zipcode,
                        StateId = objUserViewModel.StateId,

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
                        AddressLine1 = objUserViewModel.AddressLine1,
                        AddressLine2 = objUserViewModel.AddressLine2,
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
                    db.UserInRoles.Add(objUserInRole);
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
                if (LoginDetails.RoleId == 1)
                {
                    return RedirectToAction("DisplayList", "SuperAdmin");
                }
            return View("Registration");
        }
       




    }
}