using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UserApplication.Models
{
    [Table("Address")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

       [Required(ErrorMessage = "Enter your permanent address")]
       [DisplayName("Permanent Address")]
       public string AddressLine1 { get; set; }


      [Required(ErrorMessage = "Enter your current address")]
      [DisplayName("Temporary Address")]
        public string AddressLine2 { get; set; }





        [DisplayName("Country Name")]
        [Required(ErrorMessage = "Please select your Country")]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }


        [DisplayName("State Name")]
        [Required(ErrorMessage = "Please select your State")]
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [DisplayName("City Name")]
        [Required(ErrorMessage = "Please select your City name")]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set;}


        [DisplayName("Zip code")]
        [Required(ErrorMessage = "Please enter Zipcode")]
        public int Zipcode { get; set; }

        public virtual ICollection<User> Users { get; set; }



    }
}