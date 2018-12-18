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

namespace ProiectDaw1.Controllers
{
    public class ImageController : Controller
    {
        private Image.ImageDBContext db = new Image.ImageDBContext();
        private ProfileDBContext db1 = new ProfileDBContext();
        // GET: Image
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image image)
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
            db.Images.Add(image);
            db.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public ActionResult View(int id)
        {
            Image image = new Image();
            image = db.Images.Where(t => t.ImageId == id).FirstOrDefault();
            var profiles = from profile in db1.Profiles orderby profile.ProfileId select profile;
            
            var ProfileNames = profiles.Select(p => p.Name).ToArray();
            var ProfileSurNames = profiles.Select(p => p.Prename).Concat(ProfileNames).ToArray();
            var ProfileIdsMany = profiles.Select(p => p.ProfileId).ToArray();
            var ProfileIds = image.Comments.Select(p => p.ProfileId).ToArray();
            int i = 0;
            foreach (int ids in ProfileIdsMany)
            {
                foreach (int ProfileId in ProfileIds)
                {
                    if (ids == ProfileId)
                    {
                        //commentNames.Select(i) = ;
                        //i++;
                    }
                }
            }
            ImageCustomClass imageCustomClass = new ImageCustomClass();
            imageCustomClass.commentNames = commentNames;
            imageCustomClass.image = image;
            return View(imageCustomClass);
        }
        public ActionResult Index()
        {
            var images = from image in db.Images orderby image.ImageId select image;
            return View(images);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Image = db.Images.Where(i => i.ImageId == id).FirstOrDefault();
            return View(Image);
        }
        [HttpPut]
        public ActionResult Edit(int id, Image requestImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Image image = db.Images.Find(id);
                    if (TryUpdateModel(image))
                    {
                        image.Descriere = requestImage.Descriere;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
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
            Image image = new Image();
            image = db.Images.Where(x => x.ImageId == id).FirstOrDefault();
            return View(image);
        }
        [HttpDelete]
        public ActionResult Delete1(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            var userId = User.Identity.GetUserId();
            var profile = db1.Profiles.Where(x => x.UserId == userId).FirstOrDefault();
                comment.ProfileId = profile.ProfileId;
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Image/View/" + comment.ImageId);

            
        }
    }
}
