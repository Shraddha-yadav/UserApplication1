using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserApplication.Models
{
    [Table("UserViewModel")]
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }

       [Required(ErrorMessage = "Enter your firstname.")]
       [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name.")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter valid DOB")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Enter your hobbies.")]
        public string Hobbies { get; set; }

        [Required (ErrorMessage = "Please enter valid email address")]
        [EmailAddress]
        [Remote("Email", "User", HttpMethod = "POST", ErrorMessage = "Email address already registered.")]

        public string Email { get; set; }


        public string IsEmailVerified { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
        ErrorMessage = "Password should be of minimum 6 characters with at least 1 Uppercase Alphabet, 1 Lowercase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{6,}",
        ErrorMessage = "Password should be of minimum 6 characters with at least 1 Uppercase Alphabet, 1 Lowercase Alphabet, 1 Number and 1 Special Character")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }


        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please select the course")]
        [DisplayName("Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "This field cannot be null")]
        [DisplayName("Address")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Enter your permanent address")]
        [DisplayName("Permanent Address")]
        public string AddressLine1 { get; set; }


        [Required(ErrorMessage = "Enter your current address")]
        [DisplayName("Temporary Address")]
        public string AddressLine2 { get; set; }

        [DisplayName("Zip code")]
        [RegularExpression(@"^(\d{6})$", ErrorMessage = "ZipCode should be of minimum 6 characters.")]
        [Required(ErrorMessage = "Please enter Zipcode")]
        public int Zipcode { get; set; }



        [Required(ErrorMessage = "Enter date created.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Enter date modified.")]
        public DateTime DateModified { get; set; }

        [DisplayName("Role")]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Please select your Country")]
        public int CountryId { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please select your State")]
        public int StateId { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Please select your City name")]
        public int CityId { get; set; }

        public List<Country> Countries { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }


        public List<Role> Roles { get; set; }
        public List<Course> Courses { get; set; }

        
        [DisplayName("Country")]
        public string CountryName { get; set; }


        [DisplayName("State")]
        public string StateName { get; set; }


        [DisplayName("City")]
        public string CityName { get; set; }

        [DisplayName("Course")]
        //[Required(ErrorMessage = "Course cannot be null.")]
        public string CourseName { get; set; }


        //[DisplayName("Subject")]
        //[Required(ErrorMessage = "Subject cannot be null.")]
        //public string SubjectName { get; set; }


    }
}


    

    



