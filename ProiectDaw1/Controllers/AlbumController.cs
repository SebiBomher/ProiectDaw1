using Microsoft.AspNet.Identity;
using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Album;
using static ProiectDaw1.Models.CustomModelsClass.EditProfile;
using static ProiectDaw1.Models.Image;

namespace ProiectDaw1.Controllers
{
    public class AlbumController : Controller
    {
        private AlbumDBContext db = new AlbumDBContext();

        private ImageDBContext db1 = new ImageDBContext();
        private ProfileDBContext db2 = new ProfileDBContext();
        public ActionResult Index(int id)
        {
            var albums = db.Albums.Where(p => p.ProfileId == id);
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
            return RedirectToAction("Index", "Album", new { id = album.ProfileId });
        }
        public ActionResult View(int id)
        {
            var image = db1.Images.Where(i => i.AlbumId == id);
            return View(image);
        }
        public ActionResult Delete(int id)
        {
            string UID = User.Identity.GetUserId();
            Album album = new Album();
            album = db.Albums.Where(x => x.AlbumId == id).FirstOrDefault();
            var Profile = db2.Profiles.Where(x => x.ProfileId == album.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti unui album care nu va apartine";
                return RedirectToAction("View", "Profile", new { id = album.ProfileId });
            }
            else
            {
                return View(album);
            }

        }
        [HttpDelete]
        public ActionResult Delete1(int id)
        {
            string UID = User.Identity.GetUserId();
            Album album = db.Albums.Find(id);
            var Profile = db2.Profiles.Where(x => x.ProfileId == album.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti o imagine care nu va apartine";
                return RedirectToAction("View", "Profile", new { id = album.ProfileId });
            }
            else
            {
                var Images = db1.Images.Where(i => i.AlbumId == id);
                foreach (var item in Images)
                {
                    db1.Images.Remove(item);
                }
                db.Albums.Remove(album);
                db.SaveChanges();
                return RedirectToAction("Index", "Album", new { id = album.ProfileId });
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string UID = User.Identity.GetUserId();
            var Album = db.Albums.Where(i => i.AlbumId == id).FirstOrDefault();
            var Profile = db2.Profiles.Where(x => x.ProfileId == Album.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui profil care nu va apartine";
                return RedirectToAction("View", "Profile", new { id = Album.ProfileId });
            }
            else
            {
                return View(Album);
            }
        }
        [HttpPut]
        public ActionResult Edit(int id, Album requestAlbum)
        {
            string UID = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    Album album = db.Albums.Find(id);
                    var Profile = db2.Profiles.Where(x => x.ProfileId == album.ProfileId).FirstOrDefault();
                    if (!Profile.UserId.Equals(UID))
                    {
                        TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui album care nu va apartine";
                        return RedirectToAction("View", "Profile", new { id = album.ProfileId });
                    }
                    else
                    {
                        if (TryUpdateModel(album))
                        {
                            album.Name = requestAlbum.Name;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index", "Album", new { id = album.ProfileId });
                    }
                }
                else
                {
                    return View(requestAlbum);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}