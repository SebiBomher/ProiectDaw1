using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Categories;

namespace ProiectDaw1.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoriesDBContext db = new CategoriesDBContext();
        public ActionResult Add()
        {
            return View();
        }
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
    }
}