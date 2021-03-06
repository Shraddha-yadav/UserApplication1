﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UserApplication.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [DisplayName("First Name")]
      [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
       [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field cannot be null")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please select Hobbies")]
        public string Hobbies { get; set; }

        [Required(ErrorMessage = "Enter your password.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
         ErrorMessage = "Email And Password doesn't match")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
         ErrorMessage = "Password should be of minimum 6 characters with at least 1 Uppercase Alphabet, 1 Lowercase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter valid email address")]
        [EmailAddress]

        public string Email { get; set; }

        public string IsEmailVerified { get; set; }

        [Required(ErrorMessage = "Please enter valid DOB")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Please select the course")]
        [DisplayName("Course")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [Required(ErrorMessage = "Please select the Role")]
         [DisplayName("Role")]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role RoleName { get; set; }

        [Required(ErrorMessage = "This field cannot be null")]
        [DisplayName("Address")]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

       //[Required(ErrorMessage = "Enter date created.")]
        public DateTime DateCreated { get; set; }

        //[Required(ErrorMessage = "Enter date modified.")]
        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; }
        public virtual ICollection<TeacherInSubject> TeacherInSubjects { get; set; }
    }
}