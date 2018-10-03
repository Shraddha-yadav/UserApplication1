using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserApplication.fonts
{
    public class SubjectInCourse
    {
        [Key]
        public int SubjectInCourseId { get; set; }
        public int SubjectId { get; set; }
        public int CourseId { get; set; }
    }
}