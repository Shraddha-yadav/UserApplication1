using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using UserApplication.fonts;

namespace UserApplication.Models
{
    [Table("Subject")]
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public virtual ICollection<SubjectInCourse> SubjectInCourses { get; set; }
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }


    }
}