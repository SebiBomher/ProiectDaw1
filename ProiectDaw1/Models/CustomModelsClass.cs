using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDaw1.Models
{
    public class CustomModelsClass
    {
        public class AddComentClass
        {
            public Image image { get; set; }
            public List<string> commentNames { get; set; }

        }
        public class EditProfile
        {
            [Key]
            public int ProfileId { get; set; }
            public string Name { get; set; }
            public string Prename { get; set; }
            public string Nickname { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Language { get; set; }
            public class ProfileDBContext : DbContext
            {
                public ProfileDBContext() : base("DefaultConnection") { }
                public DbSet<Profile> Profiles { get; set; }

            }
        }
        public class ViewProfileClass
        {
            public Profile profile { get; set; }
            public List<Image> images { get; set; }
        }
        public class AddPhotoClass
        {
            public Image image { get; set; }
            public virtual Categories CategoriesHelper { get; set; }
            public virtual IEnumerable<SelectListItem> Categories { get; set; }
        }

    }
}