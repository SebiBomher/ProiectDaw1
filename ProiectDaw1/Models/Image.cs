using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace ProiectDaw1.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        [Required]
        public byte[] ByteString { get; set; }
        public string Descriere { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public Profile profile;
        public int ProfileId { get; set; }
        public class ImageDBContext : DbContext
        {
            public ImageDBContext() : base("DefaultConnection") { }
            public DbSet<Image> Images { get; set; }
            public DbSet<Comment> Comments { get; set; }
            public DbSet<Categories> Categories { get; set; }

        }
    }
}