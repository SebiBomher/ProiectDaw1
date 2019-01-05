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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Prename { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
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