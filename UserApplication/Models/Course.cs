using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using UserApplication.fonts;

namespace UserApplication.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<SubjectInCourse> SubjectInCourses { get; set; }

    }
}