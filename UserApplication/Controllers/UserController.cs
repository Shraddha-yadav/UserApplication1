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
      private  ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }


        // show Registration form to New User
        [HttpGet]
        public ActionResult Registration()
        {
            //Creating object of UserViewModel
            UserViewModel model = new UserViewModel();
            //Getting Course dropdown from Database
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

           // return object of ViewModel in the view.
            return View(model);
        }
        //To post the values of the registration form to the database.
        [HttpPost]
        public ActionResult Register(UserViewModel objUserViewModel)
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
                        AddressLine1 = "Test1",
                        AddressLine2="Test2",
                        CityId = 1,
                        CountryId = 1,
                        Zipcode = 452001,
                        StateId = 7,

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
                        DOB = objUserViewModel.DateOfBirth,
                        Hobbies = objUserViewModel.Hobbies,
                        Email = objUserViewModel.Email,
                        IsEmailVerified = objUserViewModel.IsEmailVerified,
                        Password = objUserViewModel.Password,
                        ConfirmPassword = objUserViewModel.ConfirmPassword,
                        IsActive = objUserViewModel.IsActive,
                        CourseId = objUserViewModel.CourseId,
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
                    //roll back all database operations, if anything goes wrong.
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
                }
            }
            return RedirectToAction("Index", "User");
        }
    }
}