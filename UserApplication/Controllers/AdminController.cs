using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserApplication.Models;

namespace UserApplication.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
         /// <summary>
        /// GET: Get all Users Expect Super Admin and Admin
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUsers()
        {
            var returnedUserList = db.Users.Where(x => x.RoleId != 1 && x.RoleId !=2).ToList();
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
        /// <summary>
        /// GET :To create user
        /// </summary>
        /// <returns></returns>
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
            var tempRoleList = db.Roles.ToList();

            model.Countries = tempCountryList;
            model.States = tempStateList;
            model.Cities = tempCityList;
            model.Courses = tempCourseList;
            model.Roles = tempRoleList;
            return View(model);


        }
        /// <summary>
        /// Post method : To Create new user Record
        /// </summary>
        /// <param name="objUserViewModel"></param>
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



        /// <summary>
        ///  GET: To Edit User Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            model.Countries = tempCountryList;
            model.States = tempStateList;
            model.Cities = tempCityList;
            model.Courses = tempCourseList;
            model.Roles = tempRoleList;




            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.Users.Find(id);

            // var userData = from p in db.Users where p.UserId == id select p;       
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
        ///  To Edit User Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUser(int id, UserViewModel objUserViewModel)
        {
            try
            {
                User objUser = db.Users.Find(id);
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
        ///  GET: To Delete User from User table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult DeleteUser(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User objUser = db.Users.Find(id);
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
        ///  POST Method: To Delete User from User table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User objUser = db.Users.Find(id);
                    db.Users.Remove(objUser);
                    db.SaveChanges();
                }

                return RedirectToAction("GetAllUsers");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Function to get list of Roles
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoles()
        {
            using (var db = new ApplicationDbContext())
            {
                // condition not to Display SuperAdmin and Admin
                var roleList = db.Roles.Where(x => x.RoleId != 1 && x.RoleId != 2);
                return roleList.ToList();
            }
        }




        //public DataSet GetStates(string countryId)
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString);

        //    SqlCommand com = new SqlCommand("Select * from State where CountryId=@catid", con);
        //    com.Parameters.AddWithValue("@catid", countryId);

        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);

        //    return ds;

        //}

        ///// <summary>
        ///// Code to bind States.
        ///// </summary>
        ///// <param name="countryId"></param>
        ///// <returns></returns>
        //public JsonResult StateBind(string countryId)
        //{
        //    DataSet ds = GetStates(countryId);
        //    List<SelectListItem> stateList = new List<SelectListItem>();

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        stateList.Add(new SelectListItem { Text = dr["StateName"].ToString(), Value = dr["StateId"].ToString() });
        //    }
        //    return Json(stateList, JsonRequestBehavior.AllowGet);
        //}
        ///// <summary>
        ///// Get all Cities from City table
        ///// </summary>
        ///// <param name="stateId"></param>
        ///// <returns></returns>
        //public DataSet GetCity(string stateId)
        //{
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString);

        //    SqlCommand com = new SqlCommand("Select * from City where StateId=@staid", con);
        //    com.Parameters.AddWithValue("@staid", stateId);

        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);

        //    return ds;

        //}
        ///// <summary>
        ///// Code To bind City
        ///// </summary>
        ///// <param name="stateId"></param>
        ///// <returns></returns>
        //public JsonResult CityBind(string stateId)
        //{

        //    DataSet ds = GetCity(stateId);

        //    List<SelectListItem> cityList = new List<SelectListItem>();
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        cityList.Add(new SelectListItem { Text = dr["CityName"].ToString(), Value = dr["CityId"].ToString() });
        //    }
        //    return Json(cityList, JsonRequestBehavior.AllowGet);
        //}








    }
}