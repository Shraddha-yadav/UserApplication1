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
        

      private  ApplicationDbContext obj = new ApplicationDbContext();


        // Registration form
        [HttpGet]
        public ActionResult Registration()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.FirstName + "" + user.LastName + "" + user.Gender + "" + user.Hobbies + "" + user.Password + "" + user.Email + "" + user.DOB + "" + user.Role + "" + user.Course + "Succesfully Registered.";

            }
            return View();
        }

        // Coding for dropdown
        [HttpGet]
        public ActionResult Index()
        {
            //Country_Bind();

            List<Role> List = obj.Roles.ToList();
            ViewBag.RoleList = new SelectList(List, "RoleId", "RoleName");
          
            


            List<Course> Lists = obj.Course.ToList();
            ViewBag.CourseLists = new SelectList(Lists, "CourseId", "CourseName");
            //  List<Country> countries = db.Countries.ToList();

            //ViewBag.CountryList = new SelectList(countries, "ContryId", "CountryName");
            //Country_Bind();
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            List<Role> List = obj.Roles.ToList();
            ViewBag.RoleList = new SelectList(List, "RoleId", "RoleName");

            List<Course> Lists = obj.Course.ToList();
            ViewBag.CourseLists = new SelectList(Lists, "CourseId", "CourseName");
            // Country_Bind();

            User select = new User();
            select.UserId = user.UserId;
            select.FirstName = user.FirstName;
            select.LastName = user.LastName;
            select.Gender = user.Gender;
            select.Hobbies = user.Hobbies;
            select.Password = user.Password;
            select.Email = user.Email;
            select.DOB = user.DOB;
            select.RoleId = user.RoleId;
            select.CourseId = user.CourseId;
            select.AddressLine1 = user.AddressLine1;
            select.AddressLine2 = user.AddressLine2;
            select.AddressId = user.AddressId;

            obj.Users.Add(user);
            obj.SaveChanges();

            int latestUserId = user.UserId;

            UserInRole userInRole = new UserInRole();
            userInRole.UserId = latestUserId;
            userInRole.RoleId = user.RoleId;

            obj.UserInRoles.Add(userInRole);
            obj.SaveChanges();


            return View(user);
        }
        //SqlConnection ApplicationDbContext = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationDbContext"].ConnectionString);
        ////Get all country
        //public DataSet Get_Country()
        //{

        //    SqlCommand com = new SqlCommand("Select * from Countries", ApplicationDbContext);
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;
        //}

        ////Get all state
        //public DataSet Get_State(string CountryId)
        //{
        //    SqlCommand com = new SqlCommand("Select * from States where CountryId=@countryid", ApplicationDbContext);
        //    com.Parameters.AddWithValue("@countryid", CountryId);
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;
        //}
        ////Get all city
        //public DataSet Get_City(string StateId)
        //{
        //    SqlCommand com = new SqlCommand("Select * from Cities where StateId=@stateid", ApplicationDbContext);
        //    com.Parameters.AddWithValue("@stateid", StateId);
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;
        //}
        //// function to bind countries
        //public void Country_Bind()
        //{
        //    DataSet ds = Get_Country();
        //    List<SelectListItem> countrylist = new List<SelectListItem>();

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        countrylist.Add(new SelectListItem { Text = dr["CountryName"].ToString(), Value = dr["CountryId"].ToString() });

        //    }
        //    ViewBag.Country = countrylist;
        //}
        //public JsonResult State_Bind(string CountryId)
        //{
        //    DataSet ds = Get_State(CountryId);
        //    List<SelectListItem> statelist = new List<SelectListItem>();

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        statelist.Add(new SelectListItem { Text = dr["StateName"].ToString(), Value = dr["StateId"].ToString() });
        //    }
        //    return Json(statelist, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult City_Bind(string StateId)
        //{
        //    DataSet ds = Get_City(StateId);
        //    List<SelectListItem> citylist = new List<SelectListItem>();

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        citylist.Add(new SelectListItem { Text = dr["CityName"].ToString(), Value = dr["CityId"].ToString() });
        //    }
        //    return Json(citylist, JsonRequestBehavior.AllowGet);
        //}
        ////public JsonResult GetStateById(int CountryId)
        ////{
        ////    obj.Configuration.ProxyCreationEnabled = false;
        ////    return Json(obj.States.Where(p => p.CountryId == CountryId), JsonRequestBehavior.AllowGet);
        ////}

    }
}