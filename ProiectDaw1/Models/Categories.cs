using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectDaw1.Models
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public class CategoriesDBContext : DbContext
        {
            public CategoriesDBContext() : base("DefaultConnection") { }
            public DbSet<Categories> Categories { get; set; }

        }

    }
}