using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectDaw1.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is Required!")]
        public string Prename { get; set; }
        [Required(ErrorMessage = "Nickname is required!")]
        public string Nickname { get; set; }
        [Required(ErrorMessage = "Please enter your city.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter your country")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Please enter your language.")]
        public string Language { get; set;}
        public virtual Image ProfilePicture { get; set; }
        public int nrOfImages { get; set; }
        public string UserId { get; set; }

        public class ProfileDBContext : DbContext
        {
            public ProfileDBContext() : base("DefaultConnection") { }
            public DbSet<Profile> Profiles { get; set; }
            public DbSet<Image> Images { get; set; }

        }

        
    }
}