using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserApplication.Models;

namespace UserApplication.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Show the list of teachers with courses assigned to them
        /// </summary>
        /// <returns></returns>
        public ActionResult TeachersCourse()
        {
            var listOfTeacherCourse = db.Users.Where(u => u.RoleId == 3).ToList();
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
            var listOfStudentCourse = db.Users.Where(u => u.RoleId == 4).ToList();
            return View(listOfStudentCourse);
        }
        public ActionResult SubjectsInCourse()
        {
            var listOfSubjectsInCourse = db.SubjectsInCourses.ToList();
            return View(listOfSubjectsInCourse);
        }

        }
    }