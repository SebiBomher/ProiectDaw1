using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectDaw1.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public int ProfileId { get; set; }
        [Required(ErrorMessage ="Please enter the albums name.")]
        public string Name { get; set; }
        public class AlbumDBContext : DbContext
        {
            public AlbumDBContext() : base("DefaultConnection") { }
            public DbSet<Album> Albums { get; set; }

        }

    }
}