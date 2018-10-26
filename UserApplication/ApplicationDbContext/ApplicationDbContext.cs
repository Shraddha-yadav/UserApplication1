using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UserApplication.fonts;
using UserApplication.Models;

namespace UserApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ApplicationDbContext")
        { }


        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserInRole> UserInRole { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<SubjectInCourse> SubjectsInCourses { get; set; }
        public virtual DbSet<TeacherInSubject> TeacherInSubjects { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        public System.Data.Entity.DbSet<UserApplication.Models.UserViewModel> UserViewModels { get; set; }

        public System.Data.Entity.DbSet<UserApplication.Models.SearchRecordModel> SearchRecords { get; set; }
    }
}
