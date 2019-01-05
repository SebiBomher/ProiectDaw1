using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProiectDaw1.Models;
using static ProiectDaw1.Models.Profile;
using static ProiectDaw1.Models.EnumerableExtension;
using static ProiectDaw1.Models.CustomModelsClass;
using static ProiectDaw1.Models.Album;
using static ProiectDaw1.Models.Categories;

namespace ProiectDaw1.Controllers
{

    public class ImageController : Controller
    {

        
        private Image.ImageDBContext db = new Image.ImageDBContext();
        private ProfileDBContext db1 = new ProfileDBContext();

        private CategoriesDBContext db2 = new CategoriesDBContext();
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
        // GET: Image
        [HttpGet]
        public ActionResult Add()
        {
            Image image = new Image();
            image.Categories = GetAllCategories();
            return View(image);
        }
        [HttpPost]
        public ActionResult Add(int id, Image image)
        {
            using (Stream inputStream = image.ImageFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                image.ByteString = memoryStream.ToArray();
            }
            var Album = db3.Albums.Where(p => p.AlbumId == id).FirstOrDefault();
            image.ProfileId = Album.ProfileId;
            image.AlbumId = id;
            db.Images.Add(image);
            db.SaveChanges();
            ModelState.Clear();

            return RedirectToAction("View","Profile",new { id = Album.ProfileId});

        }
        public ActionResult View(int id)
        {
            int i = 0;
            var temp = "";
            var temp1 = "";
            List<string> commentNames = new List<string>();
            List<string> fullNames = new List<string>();
            Image image = new Image();
            image = db.Images.Where(t => t.ImageId == id).FirstOrDefault();
            var profiles = from profile in db1.Profiles orderby profile.ProfileId select profile;
            if (image.Comments.ToList() != null)
            {


                var ProfileNames = profiles.Select(p => p.Name).ToArray();
                var ProfileSurNames = profiles.Select(p => p.Prename).ToArray();
                for (i = 0; i < ProfileNames.Length; i++)
                {
                    temp = ProfileSurNames.ElementAtOrDefault(i).ToString();
                    temp1 = ProfileNames.ElementAtOrDefault(i).ToString();
                    var fullnames = temp + " " + temp1;
                    fullNames.Add(fullnames);
                }

                var ProfileIdsMany = profiles.Select(p => p.ProfileId).ToArray();
                var ProfileIds = image.Comments.Select(p => p.ProfileId).ToArray();

                foreach (int ProfileId in ProfileIds)
                {
                    foreach (int ids in ProfileIdsMany)
                    {

                        if (ids == ProfileId)
                        {
                            int number = ProfileIds.IndexOf(ProfileId);
                            commentNames.Add(fullNames.ElementAt(number).ToString());
                        }
                    }
                }
            }
            CustomModelsClass.AddComentClass imageCustomClass = new AddComentClass();
            imageCustomClass.commentNames = commentNames;
            imageCustomClass.image = image;
            return View(imageCustomClass);
        }
        
        public ActionResult Index()
        {
            var images = from image in db.Images orderby image.ImageId descending select image;
            return View(images);
        }
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string UID = User.Identity.GetUserId();
            var Image = db.Images.Where(i => i.ImageId == id).FirstOrDefault();
            var Profile = db1.Profiles.Where(x => x.ProfileId == Image.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui profil care nu va apartine";
                return RedirectToAction("Index");
            }
            else
            {
                return View(Image);
            }
        }
        [HttpPut]
        public ActionResult Edit(int id, Image requestImage)
        {
            string UID = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    Image image = db.Images.Find(id);
                    var Profile = db1.Profiles.Where(x => x.ProfileId == image.ProfileId).FirstOrDefault();
                    if (!Profile.UserId.Equals(UID))
                    {
                        TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unei imagini care nu va apartine";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (TryUpdateModel(image))
                        {
                            image.Descriere = requestImage.Descriere;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestImage);
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
            Image image = new Image();
            image = db.Images.Where(x => x.ImageId == id).FirstOrDefault();
            var Profile = db1.Profiles.Where(x => x.ProfileId == image.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti o imagine care nu va apartine";
                return RedirectToAction("Index");
            }
            else
            {
                return View(image);
            }
            
        }
        [HttpDelete]
        public ActionResult Delete1(int id)
        {
            string UID = User.Identity.GetUserId();
            Image image = db.Images.Find(id);
            var Profile = db1.Profiles.Where(x => x.ProfileId == image.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti o imagine care nu va apartine";
                return RedirectToAction("Index");
            }
            else
            {
                db.Images.Remove(image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Search(string searching)
        {
            return View(db.Images.Where(x => x.Descriere.Contains(searching) || searching == null).ToList());
        }
    }
}
