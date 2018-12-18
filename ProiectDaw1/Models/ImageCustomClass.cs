using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectDaw1.Models
{
    public class ImageCustomClass
    {
        public Image image { get; set; }
        public List<string> commentNames { get; set; }

        
    }
}