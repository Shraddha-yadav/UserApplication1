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
    public class SuperAdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: SuperAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllUsers()
        {
            var returnedUserList = db.Users.Where(x => x.RoleId != 1).ToList();
            return View(returnedUserList);
        }

        //public ActionResult DisplayUserList()
        //{
        //    var list = db.User.ToList();
        //    return View(list);
        //}

        // <summary>
        //  GET: To Show the details of the user
        //</summary>
        // <param name = "id" ></ param >
        // < returns ></ returns >

        //public ActionResult UserDetails(int? id)
        //{
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        User user = db.Users.Find(id);
        //        // var userData = from p in db.Users
        //        //  where p.UserId == id
        //        //  select p;
        //        // var tempUserList = db.Users.ToList();

        //        UserViewModel objUserViewModel = new UserViewModel();

        //        //objUserViewModel.UserId = user.UserId;
        //        objUserViewModel.FirstName = user.FirstName;
        //        objUserViewModel.LastName = user.LastName;
        //        objUserViewModel.Gender = user.Gender;
        //        objUserViewModel.Hobbies = user.Hobbies;
        //        objUserViewModel.Email = user.Email;
        //        objUserViewModel.Password = user.Password;
        //        objUserViewModel.DOB = user.DOB;
        //        objUserViewModel.RoleId = user.RoleId;
        //        objUserViewModel.CourseId = user.CourseId;
        //        //objUserViewModel.AddressId = user.AddressId;
        //        objUserViewModel.IsActive = user.IsActive;
        //        objUserViewModel.DateCreated = user.DateCreated;
        //        objUserViewModel.DateModified = user.DateModified;
        //        objUserViewModel.AddressLine1 = user.Address.AddressLine1;
        //        objUserViewModel.AddressLine2 = user.Address.AddressLine2;
        //        objUserViewModel.CountryId = user.Address.CountryId;
        //        objUserViewModel.StateId = user.Address.StateId;
        //        objUserViewModel.CityId = user.Address.CityId;
        //        objUserViewModel.Zipcode = user.Address.Zipcode;


        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(objUserViewModel);
        //    }
        //}

        [HttpGet]
        public ActionResult CreateUser()
        {
            //Dropdown for Role List
            List<Role> List = db.Roles.ToList();
            ViewBag.RoleList = new SelectList(List, "RoleId", "RoleName");

            //Dropdown for the Course List
            List<Course> Lists = db.Courses.ToList();
            ViewBag.CourseLists = new SelectList(Lists, "CourseId", "CourseName");

            //Dropdown for the Country List
            List<Country> List1 = db.Countries.ToList();
            ViewBag.CountryList1 = new SelectList(List1, "CountryId", "CountryName");

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUser(UserViewModel userViewModel)
        {

            //Dropdown for Role List
            List<Role> List = db.Roles.ToList();
            ViewBag.RoleList = new SelectList(List, "RoleId", "RoleName");

            //Dropdown for Course List
            List<Course> Lists = db.Courses.ToList();
            ViewBag.CourseLists = new SelectList(Lists, "CourseId", "CourseName");

            //Dropdown for Country List
            List<Country> List1 = db.Countries.ToList();
            ViewBag.CountryList1 = new SelectList(List1, "CountryId", "CountryName");

           // userViewModel.AddressId = 1;
           // userViewModel.UserId = 1;

            //Object of address table
            Address address = new Address();
            //Binding the fields of address table
           // address.AddressId = userViewModel.AddressId;
            address.CountryId = userViewModel.CountryId;
            address.StateId = userViewModel.StateId;
            address.CityId = userViewModel.CityId;
            address.Zipcode = userViewModel.Zipcode;

            db.Addresses.Add(address);  //Insert data in address table
            db.SaveChanges();           //Save data in database
            int latestAddressId = address.AddressId;

            //Object of user table 
            User user = new User();
            //Binding the fields of user table
            //user.UserId = userViewModel.UserId;
            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Gender = userViewModel.Gender;
            user.Hobbies = userViewModel.Hobbies;
            user.Password = userViewModel.Password;
            user.Email = userViewModel.Email;
            user.DOB = userViewModel.DOB;
            user.DateCreated = userViewModel.DateCreated;
            user.DateModified = userViewModel.DateModified;
            user.IsActive = userViewModel.IsActive;
            user.RoleId = userViewModel.RoleId;
            user.CourseId = userViewModel.CourseId;
            user.AddressLine1 = userViewModel.AddressLine1;
            user.AddressLine2 = userViewModel.AddressLine2;
            user.AddressId = latestAddressId;

            db.Users.Add(user);      //Insert data in user table
            db.SaveChanges();         //Save data in database

            int latestUserId = user.UserId;

            UserInRole userInRole = new UserInRole();
            userInRole.UserId = latestUserId;
            userInRole.RoleId = userViewModel.RoleId;

            db.UserInRoles.Add(userInRole);
            db.SaveChanges();

            //return View(userViewModel);
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Get all country
        /// </summary>
        SqlConnection ApplicationDbContext = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString);
        public DataSet Get_Country()
        {
            SqlCommand com = new SqlCommand("Select * from Countries", ApplicationDbContext);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Get all state
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        public DataSet Get_State(string CountryId)
        {
            SqlCommand com = new SqlCommand("Select * from State where CountryId=@countryid", ApplicationDbContext);
            com.Parameters.AddWithValue("@countryid", CountryId);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Get all city
        /// </summary>
        /// <param name="StateId"></param>
        /// <returns></returns>
        public DataSet Get_City(string StateId)
        {
            SqlCommand com = new SqlCommand("Select * from City where StateId=@stateid", ApplicationDbContext);
            com.Parameters.AddWithValue("@stateid", StateId);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// Bind the country
        /// </summary>
        public void Country_Bind()
        {
            DataSet ds = Get_Country();
            List<SelectListItem> countrylist = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                countrylist.Add(new SelectListItem { Text = dr["CountryName"].ToString(), Value = dr["CountryId"].ToString() });

            }
            ViewBag.Country = countrylist;
        }
        /// <summary>
        /// Bind the state
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        public JsonResult State_Bind(string CountryId)
        {
            DataSet ds = Get_State(CountryId);
            List<SelectListItem> statelist = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                statelist.Add(new SelectListItem { Text = dr["StateName"].ToString(), Value = dr["StateId"].ToString() });
            }
            return Json(statelist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Bind the city
        /// </summary>
        /// <param name="StateId"></param>
        /// <returns></returns>
        public JsonResult City_Bind(string StateId)
        {
            DataSet ds = Get_City(StateId);
            List<SelectListItem> citylist = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                citylist.Add(new SelectListItem { Text = dr["CityName"].ToString(), Value = dr["CityId"].ToString() });
            }
            return Json(citylist, JsonRequestBehavior.AllowGet);
        }
       
        // <summary>
        // GET To Create new User Record
        // </summary>
        // <returns></returns>

        //public ActionResult CreateUser()
        //{
        //    //Code to show DropDown for Role.
        //    List<Role> roleList = GetRoles();
        //    ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
        //   // Code to show DropDown for Course.
        //   List<Course> courseList = db.Courses.ToList();
        //   ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
        //   //Code to show DropDown for Country.
        //   List<Country> countryList = db.Countries.ToList();
        //   ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");

        //    return View();
        //}


        // <summary>
        // Post method : To Create new user Record
        // </summary>
        //<param name = "objUserModel" ></ param >
        // < returns ></ returns >

        //[HttpPost]
        //public ActionResult CreateUser(UserViewModel objUserModel)
        //{
        //    //Code to show Roles in DropDown
        //    List<Role> roleList = GetRoles();
        //    ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
        //   // Code to show Courses in DropDown
        //    List<Course> courseList = db.Courses.ToList();
        //    ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
        //    //Code to show Countries in DropDown
        //    List<Country> countryList = db.Countries.ToList();
        //    ViewBag.CountryList = new SelectList(countryList, "CountryId", "CountryName");

        //    objUserModel.UserId = 1;
        //    objUserModel.AddressId = 1;

        //    /* Create the TransactionScope to execute the commands, guaranteeing
        //     * 
        //     that both commands can commit or roll back as a single unit of work.*/

        //    using (var transaction = db.Database.BeginTransaction())
        //    {

        //        try
        //        {
        //            Address address = new Address();

        //            address.AddressId = objUserModel.AddressId;
        //            address.AddressLine1 = objUserModel.AddressLine1;
        //            address.AddressLine2 = objUserModel.AddressLine2;
        //            address.CountryId = objUserModel.CountryId;
        //            address.StateId = objUserModel.StateId;
        //            address.CityId = objUserModel.CityId;

        //            db.Addresses.Add(address); //Address of the user is stored in the DataBase.
        //            db.SaveChanges();

        //            //Data is saved in the User Table.
        //            int latestAddressId = address.AddressId;

        //            User obj = new User();

        //            obj.FirstName = objUserModel.FirstName;
        //            obj.LastName = objUserModel.LastName;
        //            obj.Gender = objUserModel.Gender;
        //            obj.Hobbies = objUserModel.Hobbies;
        //            obj.Password = objUserModel.Password;
        //            obj.ConfirmPassword = objUserModel.ConfirmPassword;
        //            obj.IsEmailVerified = objUserModel.IsEmailVerified;
        //            obj.Email = objUserModel.Email;
        //            obj.DOB = objUserModel.DOB;
        //            obj.IsActive = objUserModel.IsActive;
        //            obj.DateCreated = DateTime.Now;
        //            obj.DateModified = DateTime.Now;
        //            obj.RoleId = objUserModel.RoleId;
        //            obj.CourseId = objUserModel.CourseId;
        //            obj.AddressId = latestAddressId;
        //            db.Users.Add(obj);
        //            db.SaveChanges();

        //           // User and their Roles are saved in the UserInRole Table.
        //            int latestUserId = obj.UserId;
        //            UserInRole userInRole = new UserInRole();
        //            userInRole.RoleId = objUserModel.RoleId;
        //            userInRole.UserId = latestUserId;
        //            db.UserInRoles.Add(userInRole);

        //            db.SaveChanges();
        //            transaction.Commit();
        //            return RedirectToAction("GetAllUsers");
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            ViewBag.ResultMessage = "Error occurred in the registration process.Please register again.";
        //            return View(ex);
        //        }

        //    }

        //}
        //<summary>
        ///  GET: To Edit User Record
        //</summary>
        // <param name = "id" ></ param >
        // < returns ></ returns >
        public ActionResult EditUser(int id)
        {

            List<Role> objRoleList = db.Roles.ToList();
            ViewBag.Role = new SelectList(db.Users.ToList(), "RoleId", "RoleName");
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");

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

        // <summary>
        //  To Edit User Record
        // </summary>
        // <param name="id"></param>
        // <param name="collection"></param>
        // <returns></returns>
        [HttpPost]
        public ActionResult EditUser(int id, UserViewModel objUserViewModel)
        {
            List<Role> objRoleList = db.Roles.ToList();
            ViewBag.Role = new SelectList(db.Users.ToList(), "RoleId", "RoleName");
            List<Course> objCourseList = db.Courses.ToList();
            ViewBag.Course = objCourseList;
            List<Country> countryList = db.Countries.ToList();
            ViewBag.CountryList = new SelectList(countryList, "CountryId", "Name");
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


    }
}













      