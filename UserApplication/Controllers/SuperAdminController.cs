using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserApplication.fonts;
using UserApplication.Models;

namespace UserApplication.Controllers
{
    public class SuperAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// GET: Get all users except Super Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUsers()
        {
            var returnedUserList = db.User.Where(x => x.RoleId != 1).ToList();
            return View(returnedUserList);
        }
        
        /// <summary>
        /// GET: To Show the details of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult UserDetails(int? id)
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
        /// <summary>
        /// GET:Super Admin can create user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateUser()
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
            var tempRoleList = db.Roles.Where(u => u.RoleId != 1).ToList();

            model.Countries = tempCountryList;
            model.States = tempStateList;
            model.Cities = tempCityList;
            model.Courses = tempCourseList;
            model.Roles = tempRoleList;
            return View(model);
        }
        /// <summary>
        /// POST:Super Admin can create user
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUser(UserViewModel objUserViewModel)
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
        public JsonResult GetStates(int CountryId)
        {
            //State dropdown
            List<State> stateslist = new List<State>();
            try
            {
                //data from db is filled in the data variable which is in the form of list
                var newTemp = db.States.Where(val => val.CountryId == CountryId).Select(val => new { val.StateName, val.StateId }).ToList();

                //using loop putting each value in the countriesList
                foreach (var item in newTemp)
                {
                    State testState = new State
                    {
                        StateId = Convert.ToInt32(item.StateId),
                        StateName = item.StateName.ToString()
                    };
                    stateslist.Add(testState);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Source);
            }
            return Json(stateslist, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// this for binding as well as get list of selected cities
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public JsonResult GetCities(int StateId)
        {
            //City dropdown
            List<City> citieslist = new List<City>();
            try
            {
                //data from db is filled in the data variable which is in the form of list
                var newTemp = db.Cities.Where(val => val.StateId == StateId).Select(val => new { val.CityName, val.CityId }).ToList();

                //using loop putting each value in the countriesList
                foreach (var item in newTemp)
                {
                    City testCity = new City
                    {
                        CityId = Convert.ToInt32(item.CityId),
                        CityName = item.CityName.ToString()
                    };
                    citieslist.Add(testCity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Source);
            }

            return Json(citieslist, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GET:Super Admin can edit the user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUser(int id)
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
            var tempRoleList = db.Roles.ToList();

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
        public ActionResult EditUser(int id, UserViewModel objUserViewModel)
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

                    return RedirectToAction("GetAllUsers");

                }
                return View(objUserViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// GET:Super Admin can remove user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.User.Find(id);
            // var userData = (from p in db.Users
            // where p.UserId == id
            // select p).ToList();
            //var tempUserList = db.Users.ToList();

            UserViewModel objUserViewModel = new UserViewModel();
            objUserViewModel.FirstName = objUser.FirstName;
            objUserViewModel.LastName = objUser.LastName;
            objUserViewModel.Gender = objUser.Gender;
            objUserViewModel.Hobbies = objUser.Hobbies;
            objUserViewModel.Email = objUser.Email;
            objUserViewModel.Password = objUser.Password;
            objUserViewModel.DOB = objUser.DOB;
            objUserViewModel.RoleId = objUser.RoleId;
            objUserViewModel.CourseId = objUser.CourseId;
            objUserViewModel.IsActive = objUser.IsActive;
            objUserViewModel.DateCreated = objUser.DateCreated;
            objUserViewModel.DateModified = objUser.DateModified;
            objUserViewModel.AddressLine1 = objUser.Address.AddressLine1;
            objUserViewModel.AddressLine2 = objUser.Address.AddressLine2;
            objUserViewModel.CountryId = objUser.Address.CountryId;
            objUserViewModel.StateId = objUser.Address.StateId;
            objUserViewModel.CityId = objUser.Address.CityId;
            objUserViewModel.Zipcode = objUser.Address.Zipcode;

            if (objUser == null)
            {
                return HttpNotFound();
            }
            return View(objUserViewModel);

        }
        /// <summary>
        /// POST:Super Admin can remove user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UserInRole objUserInRole = db.UserInRole.Where(m => m.UserId == id).FirstOrDefault();
                    User objUser = db.User.Where(m => m.UserId == id).FirstOrDefault();
                    Address objAddress = db.Addresses.Where(m => m.AddressId == objUser.AddressId).FirstOrDefault();

                    //To remove address of user from address table
                    db.Addresses.Remove(objAddress);
                    //To Remove User from User Table
                    db.User.Remove(objUser);

                    // To remove User from UserInRole table.
                    db.UserInRole.Remove(objUserInRole);

                    db.SaveChanges();

                }
                return RedirectToAction("GetAllUsers");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult CreateCourse()
        {
            return View();
        }
        /// <summary>
        /// POST : Admin can create course
        /// </summary>
        /// <param name="objCourse"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateCourse(Course objCourse)
        {
            db.Courses.Add(objCourse);      //Insert data 
            db.SaveChanges();               //Save data

            return RedirectToAction("CourseList");
        }
        /// <summary>
        /// GET : Super Admin can create subject
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateSubject()
        {
            return View();
        }
        /// <summary>
        /// POST: Super Admin can create subject
        /// </summary>
        /// <param name="objSubject"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSubject(Subject objSubject)
        {
            db.Subjects.Add(objSubject);
            db.SaveChanges();

            return RedirectToAction("SubjectList");
        }
        /// <summary>
        /// GET: Super Admin can assign subject to course
        /// </summary>
        /// <returns></returns>

        public ActionResult AssignSubjectForCourse()
        {
            List<Course> List = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(List, "CourseId", "CourseName");

            List<Subject> Lists = db.Subjects.ToList();
            ViewBag.SubjectList = new SelectList(Lists, "SubjectId", "SubjectName");

            return View();
        }
        /// <summary>
        /// POST:Super Admin can assign subject to course
        /// </summary>
        /// <param name="objSubjectInCourse"></param>
        /// <returns></returns>           
        [HttpPost]
        public ActionResult AssignSubjectForCourse(SubjectInCourse objSubjectInCourse)
        {
            List<Course> List = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(List, "CourseId", "CourseName", objSubjectInCourse.CourseId);

            List<Subject> Lists = db.Subjects.ToList();
            ViewBag.SubjectList = new SelectList(Lists, "SubjectId", "SubjectName", objSubjectInCourse.SubjectId);

            db.SubjectsInCourses.Add(objSubjectInCourse);
            db.SaveChanges();


            //return View(objSubjectInCourse);
            return RedirectToAction("CourseAndSubjectList");
        }
        public ActionResult CourseList()
        {
            var listOfCourse = db.Courses.ToList();
            return View(listOfCourse);
        }
        public ActionResult SubjectList()
        {
            var listOfSubject = db.Subjects.ToList();
            return View(listOfSubject);
        }
        [HttpGet]
        public ActionResult DeleteCourse(int id)
        {
            var removeCourse = db.Courses.Single(x => x.CourseId == id);

            return View(removeCourse);
        }
        [HttpPost]
        public ActionResult DeleteCourse(int id, Course objCourse)
        {
            try
            {
                // TODO: Add delete logic here
                var deleteCourse = db.Courses.Single(x => x.CourseId == id);
                db.Courses.Remove(deleteCourse);

                db.SaveChanges();

                return RedirectToAction("CourseList");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult DeleteSubject(int id)
        {
            var removeSubject = db.Subjects.Single(x => x.SubjectId == id);

            return View(removeSubject);
        }
        [HttpPost]
        public ActionResult DeleteSubject(int id, Subject objSubject)
        {
            try
            {
                // TODO: Add delete logic here
                var deleteSubject = db.Subjects.Single(x => x.SubjectId == id);
                db.Subjects.Remove(deleteSubject);

                db.SaveChanges();

                return RedirectToAction("SubjectList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CourseAndSubjectList()
        {
            var listOfCourseAndSubject = db.SubjectsInCourses.ToList();
            return View(listOfCourseAndSubject);
        }
        [HttpGet]
        public ActionResult CourseAndSubject(int id)
        {
            var removeCourseAndSubject = db.SubjectsInCourses.Single(x => x.CourseId == id);

            return View(removeCourseAndSubject);
        }

        /// <summary>
        /// GET: Super Admin can assign subject to teacher
        /// </summary>
        /// <returns></returns>

        public ActionResult AssignSubjectToTeacher()
        {
            

            List<User> List = db.User.Where(u => u.RoleId != 1 && u.RoleId != 2 && u.RoleId != 4).ToList();
            ViewBag.TeacherList = new SelectList(List, "UserId", "FirstName");

            List<Subject> Lists = db.Subjects.ToList();
            ViewBag.SubjectList = new SelectList(Lists, "SubjectId", "SubjectName");

            return View();
        }
        /// <summary>
        /// POST:Super Admin can assign subject to teacher
        /// </summary>
        /// <param name="objTeacherInSubject"></param>
        /// <returns></returns>           
        [HttpPost]
        public ActionResult AssignSubjectToTeacher(TeacherInSubject objTeacherInSubject)
        {
            
            List<User> List = db.User.Where(u => u.RoleId != 1 && u.RoleId != 2 && u.RoleId != 4).ToList();
            ViewBag.TeacherList = new SelectList(List, "UserId", "FirstName", objTeacherInSubject.UserId);


            List<Subject> Lists = db.Subjects.ToList();
            ViewBag.SubjectList = new SelectList(Lists, "SubjectId", "SubjectName", objTeacherInSubject.SubjectId);

            db.TeacherInSubjects.Add(objTeacherInSubject);
            db.SaveChanges();


            return RedirectToAction("TeacherAndSubjectList");
        }
        public ActionResult TeacherAndSubjectList()
        {
            var listOfTeacherAndSubject = db.TeacherInSubjects.ToList();
            return View(listOfTeacherAndSubject);
        }

    }
}















