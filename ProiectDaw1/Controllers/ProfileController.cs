using Microsoft.AspNet.Identity;
using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Album;
using static ProiectDaw1.Models.Categories;
using static ProiectDaw1.Models.Profile;

namespace ProiectDaw1.Controllers
{
    public class ProfileController : Controller
    {
        private CategoriesDBContext db2 = new CategoriesDBContext();
        private ApplicationDbContext db1 = ApplicationDbContext.Create();
        private ProfileDBContext db = new ProfileDBContext();
        private AlbumDBContext db3 = new AlbumDBContext();
        [NonAction]
        public IEnumerable<Categories> GetAllCategories()
        {
            // generam o lista goala    
            var selectList = new List<Categories>();
            // Extragem toate categoriile din baza de date    
            var categories = from cat in db2.Categories select cat;
            // iteram prin categorii          
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown     
                selectList.Add(new Categories
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name.ToString()
                });
            }
            // returnam lista de categorii       
            return selectList;
        }
        
        public ActionResult New(string id)
        {
            var userId = User.Identity.GetUserId();
            var profile = db.Profiles.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
            if (profile == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("View","Profile", new { id = profile.ProfileId });
            }
            
        }
        [HttpPost]
        public ActionResult New(Profile profile)
        {
            using (Stream inputStream = profile.ProfilePicture.ImageFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                profile.ProfilePicture.ByteString = memoryStream.ToArray();
            }
            profile.ProfilePicture.ProfileId = profile.ProfileId;
            profile.UserId = User.Identity.GetUserId();
            profile.nrOfImages = 0;
            db.Images.Add(profile.ProfilePicture);
            db.Profiles.Add(profile);
            db.SaveChanges();
            return RedirectToAction("View","Profile", new { id = profile.ProfileId });
        }
        public ActionResult Index()
        {
            var profiles = from profile in db.Profiles orderby profile.ProfileId select profile;
            return View(profiles);
        }
        public ActionResult View(int id)
        {
            string UID = User.Identity.GetUserId();
            var profile = db.Profiles.Where(x => x.ProfileId == id).FirstOrDefault();
            var images = db.Images.Where(x => x.ProfileId == profile.ProfileId).ToList();
            CustomModelsClass.ViewProfileClass viewProfile = new CustomModelsClass.ViewProfileClass();
            viewProfile.profile = profile;
            if (profile.UserId == UID)
            {
                viewProfile.OwnerOfProfile = true;
            }
            else
            {
                viewProfile.OwnerOfProfile = false;
            }
            viewProfile.images = images;
            return View(viewProfile);
        }
        public ActionResult Edit(int id)
        {
            string UID = User.Identity.GetUserId();
            var Profile = db.Profiles.Where(p => p.ProfileId == id).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui profil care nu va apartine";
                return RedirectToAction("Index", "Image");
            }
            return View(Profile);
        }
        [HttpPut]
        public ActionResult Edit(int id, Profile requestProfile)
        {
            string UID = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    
                    Profile profile = db.Profiles.Find(id);
                    if (!profile.UserId.Equals(UID))
                    {
                        TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui profil care nu va apartine";
                        return RedirectToAction("Index","Image");
                    }
                    else
                    {
                        if (TryUpdateModel(profile))
                        {

                            profile.Name = requestProfile.Name;
                            profile.Prename = requestProfile.Prename;
                            profile.Nickname = requestProfile.Nickname;
                            profile.City = requestProfile.City;
                            profile.Country = requestProfile.Country;
                            profile.Language = requestProfile.Language;
                            db.SaveChanges();
                        }
                        return RedirectToAction("View", "Profile", new { id = id});
                    }
                }
                else
                {
                    return View(requestProfile);
                }
            }
            catch (Exception e)
            {

                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            string UID = User.Identity.GetUserId();
            Profile profile = new Profile();
            profile = db.Profiles.Where(x => x.ProfileId == id).FirstOrDefault();
            if (!profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti o imagine care nu va apartine";
                return RedirectToAction("Index");
            }
            else
            {
                return View(profile);
            }

        }
        [HttpDelete]
        public ActionResult Delete1(int id)
        {
            string UID = User.Identity.GetUserId();
            Profile profile = db.Profiles.Find(id);
            if (!profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti o imagine care nu va apartine";
                return RedirectToAction("Index");
            }
            else
            {
                var Image = db.Images.Where(i => i.ProfileId == id);
                var Album = db3.Albums.Where(x => x.ProfileId == id);
                foreach (var item in Image)
                {
                    db.Images.Remove(item);
                }
                foreach (var item in Album)
                {
                    db3.Albums.Remove(item);
                }
                db.Profiles.Remove(profile);
                db.SaveChanges();
                return RedirectToAction("Index","Image");
            }
        }
        public ActionResult Search(string searching)
        {
            return View(db.Profiles.Where(x => x.Nickname.Contains(searching) || searching == null).ToList());
        }

    }
}