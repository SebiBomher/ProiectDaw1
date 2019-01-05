using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Album;
using static ProiectDaw1.Models.Image;

namespace ProiectDaw1.Controllers
{
    public class AlbumController : Controller
    {
        private AlbumDBContext db = new AlbumDBContext();

        private ImageDBContext db1 = new ImageDBContext();
        public ActionResult Index()
        {
            var albums = from album in db.Albums orderby album.AlbumId select album;
            return View(albums);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(int id, Album album)
        {
            album.ProfileId = id;
            db.Albums.Add(album);
            db.SaveChanges();
            return View();
        }
        public ActionResult View(int id)
        {
            var Image = db1.Images.Where(i => i.AlbumId == id);
            return View(Image);
        }
    }
}