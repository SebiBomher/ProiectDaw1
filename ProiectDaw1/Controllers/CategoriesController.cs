using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Categories;
using static ProiectDaw1.Models.Image;

namespace ProiectDaw1.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoriesDBContext db = new CategoriesDBContext();
        private ImageDBContext db1 = new ImageDBContext();
        [Authorize(Roles = "Administrator")]
        public ActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Add(Categories categories)
        {
            db.Categories.Add(categories);
            db.SaveChanges();
            return View();
        }
        public ActionResult Index()
        {
            var categoriess = from categories in db.Categories orderby categories.CategoryId select categories;
            return View(categoriess);
        }
        public ActionResult View(int id)
        {
            var images = db1.Images.Where(x => x.CategoryId == id);
            return View(images);
        }
    }
}