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
        public ActionResult StudentHomePage(int? id)
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


                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(objUserViewModel);
            }

        }

        /// <summary>
        ///  GET: To Edit student profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditStudentProfile(int? id)
        {
            try
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




            var studentDetails = (from
                                         user in db.User
                                  join userInRole in db.UserInRole on user.UserId equals userInRole.UserId

                                  where user.UserId == id

                                  select new UserViewModel
                                  {

                                      UserId = user.UserId,
                                      CountryId = user.Address.CountryId,
                                      AddressId = user.AddressId,
                                      StateId = user.Address.StateId,
                                      CityId = user.Address.CityId,
                                      CourseId = user.CourseId,
                                      RoleId = userInRole.RoleId,
                                      RoleName = userInRole.Role.RoleName,


                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Gender = user.Gender,
                                      DOB = user.DOB,
                                      Hobbies = user.Hobbies,
                                      Email = user.Email,
                                      IsEmailVerified = user.IsEmailVerified,
                                      Password = user.Password,
                                      ConfirmPassword = user.ConfirmPassword,
                                      IsActive = user.IsActive,
                                      DateCreated = user.DateCreated,
                                      DateModified = user.DateModified,
                                     // CourseName = user.Course.CourseName,
                                      AddressLine1 = user.Address.AddressLine1,
                                      AddressLine2 = user.Address.AddressLine2,

                                      //CityName = user.Address.City.CityName,
                                     // StateName = user.Address.State.StateName,
                                      //CountryName = user.Address.Country.CountryName,
                                      Zipcode = user.Address.Zipcode

                                  }).FirstOrDefault();

            studentDetails.Countries = countryList;
            studentDetails.States = stateList;
            studentDetails.Cities = cityList;
            studentDetails.Courses = courseList;
            studentDetails.Roles = roleList;

            return View(studentDetails);
        }
             catch(Exception er)
            {
                Console.Write(er.Message);
                return View();
             }

}

        /// <summary>
        /// Save updates in database.
        /// </summary>
        [HttpPost]
        public ActionResult EditStudentProfile(UserViewModel objUserViewModel)
        {
            try
            {
                //Update User table.

                var userRecord = (from user in db.User
                                  where user.UserId == objUserViewModel.UserId
                                  select user).FirstOrDefault();
                if (userRecord != null)

                {
                    userRecord.DateCreated = DateTime.Now;
                    userRecord.DateModified = DateTime.Now;
                    userRecord.UserId = objUserViewModel.UserId;
                    userRecord.FirstName = objUserViewModel.FirstName;
                    userRecord.LastName = objUserViewModel.LastName;
                    userRecord.Gender = objUserViewModel.Gender;
                    userRecord.DOB = objUserViewModel.DOB;
                    userRecord.Hobbies = objUserViewModel.Hobbies;
                    userRecord.Email = objUserViewModel.Email;
                    userRecord.IsEmailVerified = objUserViewModel.IsEmailVerified;
                    userRecord.Password = objUserViewModel.Password;
                    userRecord.ConfirmPassword = objUserViewModel.ConfirmPassword;
                    userRecord.IsActive = objUserViewModel.IsActive;
                    userRecord.CourseId = objUserViewModel.CourseId;
                }

                //Update Address table.
                var addressRecord = (from address in db.Addresses
                                     where address.AddressId == objUserViewModel.AddressId
                                     select address
                              ).FirstOrDefault();
                if (addressRecord != null)
                {

                    addressRecord.AddressLine1 = objUserViewModel.AddressLine1;
                    addressRecord.AddressLine2 = objUserViewModel.AddressLine2;

                    addressRecord.CityId = objUserViewModel.CityId;
                    addressRecord.CountryId = objUserViewModel.CountryId;
                    addressRecord.Zipcode = objUserViewModel.Zipcode;
                    addressRecord.StateId = objUserViewModel.StateId;
                }

                //Update UserInRole Table.
                var userInRoleRecord = (from userInRole in db.UserInRole
                                        where userInRole.UserId == objUserViewModel.UserId
                                        select userInRole
                                      ).FirstOrDefault();

                if (userInRoleRecord != null)
                {
                    userInRoleRecord.RoleId = objUserViewModel.RoleId;
                    userInRoleRecord.UserId = objUserViewModel.UserId;
                }

                //Save to database.
                db.SaveChanges();



                return RedirectToAction("StudentHomePage", new { id = objUserViewModel.UserId });
            }

            catch (Exception er)
            {
                Console.Write(er.Message);
                return View();
            }

        }



        /// <summary>
        /// Show the list of teachers with courses assigned to them
        /// </summary>
        /// <returns></returns>
        public ActionResult TeachersCourse( )
        {
            var listOfTeacherCourse = db.User.Where(u => u.RoleId == 3 ).ToList();
            return View(listOfTeacherCourse);
        }
        /// <summary>
        /// Show the list of teachers with subjects against that particular course
        /// </summary>
        /// <returns></returns>
        public ActionResult TeachersSubject()
        {
            var listOfTeachersSubject = db.TeacherInSubjects.ToList();
            return View(listOfTeachersSubject);
        }
        public ActionResult StudentCourse()
        {
            var listOfStudentCourse = db.User.Where(u => u.RoleId == 4).ToList();
            return View(listOfStudentCourse);
        }
        public ActionResult SubjectsInCourse()
        {
            var listOfSubjectsInCourse = db.SubjectsInCourses.ToList();
            return View(listOfSubjectsInCourse);
        }

        }
    }