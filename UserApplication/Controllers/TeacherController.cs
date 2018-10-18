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
        /// GET: Get list of all 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStudentList()
        {
            var returnedUserList = db.Users.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 3).ToList();
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

                User user = db.Users.Find(id);
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

        public ActionResult StudentCourse()
        {
            var returnedUserList = db.Users.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 3).ToList();
            return View(returnedUserList);
        }
    }
}