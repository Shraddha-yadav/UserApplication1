using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserApplication.Models;

namespace UserApplication.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
      


        /// <summary>
        /// Get : get Teachers details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TeacherHomePage1()
        {

            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            User user = (User)Session["User"];
            var usr = db.User.Find(user.UserId);
           

            if (Session["User"] != null)
            {
              
             
                UserViewModel objUserViewModel = new UserViewModel();

                objUserViewModel.UserId = user.UserId;
                objUserViewModel.FirstName = user.FirstName;
                objUserViewModel.LastName = user.LastName;
                objUserViewModel.Gender = user.Gender;
                objUserViewModel.Hobbies = user.Hobbies;
                objUserViewModel.Email = user.Email;
                objUserViewModel.Password = user.Password;
                objUserViewModel.DOB = user.DOB;
                objUserViewModel.RoleId = user.RoleId;
                objUserViewModel.CourseId = user.CourseId;
                objUserViewModel.AddressId = user.AddressId;
                objUserViewModel.IsActive = user.IsActive;
                objUserViewModel.DateCreated = user.DateCreated;
                objUserViewModel.DateModified = user.DateModified;
                objUserViewModel.AddressLine1 = user.Address.AddressLine1;
                objUserViewModel.AddressLine2 = user.Address.AddressLine2;
                objUserViewModel.CountryId = user.Address.CountryId;
                objUserViewModel.StateId = user.Address.StateId;
                objUserViewModel.CityId = user.Address.CityId;
                objUserViewModel.Zipcode = user.Address.Zipcode;
                objUserViewModel.CountryName = user.Address.Country.CountryName;
                objUserViewModel.StateName = user.Address.State.StateName;
                objUserViewModel.CityName = user.Address.City.CityName;

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(objUserViewModel);


            }
            return View(usr);

           

        }

        /// <summary>
        /// GET:Super Admin can edit the user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditProfile(int id)
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
            var tempRoleList = db.Roles.Where(u => u.RoleId != 1 && u.RoleId != 2 && u.RoleId != 4).ToList();

            //model.States = tempStateList;
            //model.Cities = tempCityList;
            //model.Courses = tempCourseList;
            //model.Roles = tempRoleList;




            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.User.Find(id);

            // var userData = from p in db.Users where p.UserId == id select p;       
            //var tempUserList = db.Users.ToList();


            UserViewModel objUserViewModel = new UserViewModel();
            objUserViewModel.UserId = objUser.UserId;
            objUserViewModel.FirstName = objUser.FirstName;
            objUserViewModel.LastName = objUser.LastName;
            objUserViewModel.Gender = objUser.Gender;
            objUserViewModel.Hobbies = objUser.Hobbies;
            objUserViewModel.Email = objUser.Email;
            objUserViewModel.Password = objUser.Password;
            objUserViewModel.ConfirmPassword = objUser.ConfirmPassword;
            objUserViewModel.IsEmailVerified = objUser.IsEmailVerified;
            objUserViewModel.DOB = objUser.DOB;
            objUserViewModel.RoleId = objUser.RoleId;
            objUserViewModel.CourseId = objUser.CourseId;
            objUserViewModel.AddressId = objUser.AddressId;
            objUserViewModel.IsActive = objUser.IsActive;
            //objUserViewModel.DateCreated = objUser.DateCreated;
            objUser.DateModified = DateTime.Now;
            objUserViewModel.AddressLine1 = objUser.Address.AddressLine1;
            objUserViewModel.AddressLine2 = objUser.Address.AddressLine2;
            objUserViewModel.CountryId = objUser.Address.CountryId;
            objUserViewModel.StateId = objUser.Address.StateId;
            objUserViewModel.CityId = objUser.Address.CityId;
            objUserViewModel.Zipcode = objUser.Address.Zipcode;
            objUserViewModel.States = tempStateList;
            objUserViewModel.Cities = tempCityList;
            objUserViewModel.Courses = tempCourseList;
            objUserViewModel.Roles = tempRoleList;
            objUserViewModel.Countries = tempCountryList;


            if (objUser == null)
            {
                return HttpNotFound();
            }
            return View(objUserViewModel);
        }
        /// <summary>
        ///  POST:Super Admin can edit the user details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProfile(int id, UserViewModel objUserViewModel)
        {
            try
            {
                User objUser = db.User.Find(id);
                //  var userData = from p in db.Users where p.UserId == id select p;
                // var tempUserList = db.Users.FirstOrDefault();

                if (ModelState.IsValid)
                {
                    objUser.FirstName = objUserViewModel.FirstName;
                    objUser.LastName = objUserViewModel.LastName;
                    objUser.Gender = objUserViewModel.Gender;
                    objUser.Hobbies = objUserViewModel.Hobbies;
                    objUser.Email = objUserViewModel.Email;
                    objUser.IsEmailVerified = objUserViewModel.IsEmailVerified;
                    objUser.Password = objUserViewModel.Password;
                    objUser.ConfirmPassword = objUserViewModel.ConfirmPassword;
                    objUser.DOB = objUserViewModel.DOB;
                    objUser.CourseId = objUserViewModel.CourseId;
                    objUser.RoleId = objUserViewModel.RoleId;
                    objUser.Address.AddressLine1 = objUserViewModel.AddressLine1;
                    objUser.Address.AddressLine2 = objUserViewModel.AddressLine2;
                    objUser.Address.CountryId = objUserViewModel.CountryId;
                    objUser.Address.StateId = objUserViewModel.StateId;
                    objUser.Address.CityId = objUserViewModel.CityId;
                    objUser.Address.Zipcode = objUserViewModel.Zipcode;
                    objUser.IsActive = objUserViewModel.IsActive;
                    objUser.DateModified = DateTime.Now;

                    db.SaveChanges();  //User Data is saved in the user table

                    return RedirectToAction("TeacherHomePage1", new { id = objUser.UserId });

                }
                return View(objUserViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw ex;
            }

        }



        /// <summary>
        /// GET: Get list of all 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStudentList(int id)
        {
           
                var returnedUserList = db.User.Where(x => x.RoleId == 4 && x.CourseId == id).ToList();   /*db.User.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 3 && x.CourseId == id ).ToList();*/

                return View(returnedUserList);
            
           
        }
        /// <summary>
        /// GET: To Show the details of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult StudentDetails(int? id)
        {
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.User.Find(id);
                // var userData = from p in db.Users
                // where p.UserId == id
                // select p;
                // var tempUserList = db.Users.ToList();

                UserViewModel objUserViewModel = new UserViewModel();

                //objUserViewModel.UserId = user.UserId;
                objUserViewModel.FirstName = user.FirstName;
                objUserViewModel.LastName = user.LastName;
                objUserViewModel.Gender = user.Gender;
                objUserViewModel.Hobbies = user.Hobbies;
                objUserViewModel.Email = user.Email;
                objUserViewModel.Password = user.Password;
                objUserViewModel.DOB = user.DOB;
                objUserViewModel.RoleId = user.RoleId;
                objUserViewModel.CourseId = user.CourseId;
                objUserViewModel.AddressId = user.AddressId;
                objUserViewModel.IsActive = user.IsActive;
                objUserViewModel.DateCreated = user.DateCreated;
                objUserViewModel.DateModified = user.DateModified;
                objUserViewModel.AddressLine1 = user.Address.AddressLine1;
                objUserViewModel.AddressLine2 = user.Address.AddressLine2;
                objUserViewModel.CountryId = user.Address.CountryId;
                objUserViewModel.StateId = user.Address.StateId;
                objUserViewModel.CityId = user.Address.CityId;
                objUserViewModel.Zipcode = user.Address.Zipcode;
                objUserViewModel.CountryName = user.Address.Country.CountryName;
                objUserViewModel.StateName = user.Address.State.StateName;
                objUserViewModel.CityName = user.Address.City.CityName;


                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(objUserViewModel);
            }
        }

        public ActionResult StudentCourse()
        {
            var returnedUserList = db.User.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 3).ToList();
            return View(returnedUserList);
        }
    }
}