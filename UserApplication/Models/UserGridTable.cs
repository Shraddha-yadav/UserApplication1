using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserApplication.Models
{
    public class UserGridTable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Hobbies { get; set; }
        public string Email { get; set; }
        public string IsEmailVerified { get; set; }
        public DateTime DOB { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int Zipcode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }



    }
}