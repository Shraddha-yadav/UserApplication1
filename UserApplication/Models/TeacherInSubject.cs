using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserApplication.Models
{
    public class TeacherInSubject
    {
        [Key]
        public int TeacherInSubjectId { get; set; }
        public int UserId { get; set; }
        public int SubjectId { get; set; }
    }
}