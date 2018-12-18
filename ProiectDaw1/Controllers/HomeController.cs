using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Image;

namespace ProiectDaw1.Controllers
{
    public class HomeController : Controller
    {
        private ImageDBContext db = new ImageDBContext();
        public ActionResult Index()
        {
            var images = from image in db.Images orderby image.ImageId select image;
            return View(images);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}