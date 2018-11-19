using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserApplication.Models;

namespace UserApplication.Controllers
{
    public class StudentController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Student
        
         
        /// <summary>
        /// Student homepage.
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentHomePage1( )
        {
            if (Session["Login"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            User user = (User)Session["User"];
            var usr = db.User.Find(user.UserId);


            if (Session["User"] != null)
            {


                UserViewModel objUserViewModel = new UserViewModel();

                objUserViewModel.UserId = usr.UserId;
                objUserViewModel.FirstName = usr.FirstName;
                objUserViewModel.LastName = usr.LastName;
                objUserViewModel.Gender = usr.Gender;
                objUserViewModel.Hobbies = usr.Hobbies;
                objUserViewModel.Email = usr.Email;
                objUserViewModel.Password = usr.Password;
                objUserViewModel.DOB = usr.DOB;
                objUserViewModel.RoleId = usr.RoleId;
                objUserViewModel.CourseId = usr.CourseId;
                objUserViewModel.AddressId = usr.AddressId;
                objUserViewModel.IsActive = usr.IsActive;
                objUserViewModel.DateCreated = usr.DateCreated;
                objUserViewModel.DateModified = usr.DateModified;
                objUserViewModel.AddressLine1 = usr.Address.AddressLine1;
                objUserViewModel.AddressLine2 = usr.Address.AddressLine2;
                objUserViewModel.CountryId = usr.Address.CountryId;
                objUserViewModel.StateId = usr.Address.StateId;
                objUserViewModel.CityId = usr.Address.CityId;
                objUserViewModel.Zipcode = usr.Address.Zipcode;
                objUserViewModel.CountryName = usr.Address.Country.CountryName;
                objUserViewModel.StateName = usr.Address.State.StateName;
                objUserViewModel.CityName = usr.Address.City.CityName;

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
        public ActionResult EditStudentProfile(int id)
        {
            if (Session["Login"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }

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
            var tempRoleList = db.Roles.Where(u => u.RoleId != 1 && u.RoleId != 2 && u.RoleId !=3).ToList();

            // model.States = tempStateList;
            //  model.Cities = tempCityList;
            //   model.Courses = tempCourseList;
            //   model.Roles = tempRoleList;




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
            objUserViewModel.Countries = tempCountryList;
            objUserViewModel.States = tempStateList;
            objUserViewModel.Cities = tempCityList;
            objUserViewModel.Courses = tempCourseList;
            objUserViewModel.Roles = tempRoleList;


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
        public ActionResult EditStudentProfile(int id, UserViewModel objUserViewModel)
        {
            if (Session["Login"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }

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

                    return RedirectToAction("StudentHomePage1", new { id = objUser.UserId });

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
        /// Show the list of teachers with courses assigned to them
        /// </summary>
        /// <returns></returns>
        public ActionResult TeachersList(int id)
        {
            if (Session["Login"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var listOfTeacherCourse = db.User.Where(u => u.RoleId == 3 && u.CourseId == id).ToList();
            return View(listOfTeacherCourse);
        }
        /// <summary>
        /// Show the list of teachers with subjects against that particular course
        /// </summary>
        /// <returns></returns>
        public ActionResult TeachersSubject(int id)
        {
            if (Session["Login"] == null && Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var listOfTeachersSubject = db.SubjectsInCourses.Where(u => u.CourseId == id).ToList();
            return View(listOfTeachersSubject);
        }
        //public ActionResult StudentCourse()
        //{
        //    var listOfStudentCourse = db.User.Where(u => u.RoleId == 4).ToList();
        //    return View(listOfStudentCourse);
        //}
        //public ActionResult SubjectsInCourse()
        //{
        //    var listOfSubjectsInCourse = db.SubjectsInCourses.ToList();
        //    return View(listOfSubjectsInCourse);
        //}

    }
    }