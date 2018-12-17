using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectDaw1.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string comment { get; set; }
        public int ImageId { get; set; }
        public int ProfileId { get;set; }
        public Image Image;
        public class CommentDBContext : DbContext
        {
            public CommentDBContext() : base("DefaultConnection") { }
            public DbSet<Comment> Comments { get; set; }

        }
    }
}