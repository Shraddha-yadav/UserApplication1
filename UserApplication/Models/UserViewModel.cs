using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserApplication.Models
{
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

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DOB { get; set; }

        [Required(ErrorMessage = "Enter your hobbies.")]
        public string Hobbies { get; set; }

        [Required(ErrorMessage = "Enter your email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Your email address is not verified.")]
        public string IsEmailVerified { get; set; }

        [Required(ErrorMessage = "Enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        //[Required(ErrorMessage = "Your account is inactive.")]
        public bool IsActive { get; set; }

        public int CourseId { get; set; }

        public int AddressId { get; set; }
        [Required(ErrorMessage = "Enter your permanent address")]
        [DisplayName("Permanent Address")]
        public string AddressLine1 { get; set; }


        [Required(ErrorMessage = "Enter your current address")]
        [DisplayName("Temporary Address")]
        public string AddressLine2 { get; set; }

        [DisplayName("Zip code")]
        [Required(ErrorMessage = "Please enter Zipcode")]
        public int Zipcode { get; set; }



        [Required(ErrorMessage = "Enter date created.")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Enter date modified.")]
        public DateTime DateModified { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }

        public List<CountryModel> Countries { get; set; }
        public List<StateModel>  States { get; set; }
        public List<CityModel> Cities { get; set; }


        public List<RoleModel> Roles { get; set; }
        public List<CourseModel> Courses { get; set; }

    }

    public class RoleModel
    {
        
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class CourseModel
    {
        
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }

    public class CountryModel
    {
        
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please enter your country")]
        public string CountryName { get; set; }

    }
    public class StateModel
    {
        
        public int StateId { get; set; }

        public string StateName { get; set; }


    }
    public class CityModel
    {
        
        public int CityId { get; set; }

        public string CityName { get; set; }



    }

}


