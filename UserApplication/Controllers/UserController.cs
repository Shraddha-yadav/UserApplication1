﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

            objUserViewModel.Countries = tempCountryList;
            objUserViewModel.States = tempStateList;
            objUserViewModel.Cities = tempCityList;
            objUserViewModel.Courses = tempCourseList;
            objUserViewModel.Roles = tempRoleList;

            if (!ModelState.IsValid)
            {
                return View(objUserViewModel);
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //Raw data sent to address table.
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
                    objUser.IsActive = true;

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
                    ModelState.AddModelError(string.Empty, ex.Message);
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";

                }
            }
            return RedirectToAction("Login", "User");

        }
        public JsonResult getState(int id)
        {
            var states = db.States.Where(x => x.CountryId == id).ToList();
            List<SelectListItem> stateList = new List<SelectListItem>();

            stateList.Add(new SelectListItem { Text = "--select State--", Value = "0" });
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

            Session["login"] = LoginDetails;

            //if (Session["login"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            if (LoginDetails != null)
            {
                Session["UserId"] = LoginDetails.UserId.ToString();
                Session["UserName"] = LoginDetails.Email.ToString();
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

                }
                else if(LoginDetails.RoleId == 4)

                {
                    Session["User"] = LoginDetails;
                    return RedirectToAction("StudentHomePage1", "Student"/*, new { id = LoginDetails.UserId }*/);
                }
                else
            {
                    ModelState.AddModelError("", "Email and Password do not match");
                }

            }

            //else
            //{
            //    ModelState.AddModelError("", "Invalid username or password");
            //    return View("Login");
            //}
            return View("Login");
        }
        [AllowAnonymous]
        public ActionResult LogOut()
        {
                Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                Session.Abandon();
                Session.Clear();
                Response.Cookies.Clear();
                Session.RemoveAll();
                Session["Login"] = null;
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "User");
        }

        public JsonResult IsUserExists(string Email)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.   
            return Json(!db.User.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }


    }
}





