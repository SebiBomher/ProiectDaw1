using Microsoft.AspNet.Identity;
using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Profile;

namespace ProiectDaw1.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db1 = ApplicationDbContext.Create();
        private ProfileDBContext db = new ProfileDBContext();
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
            return View();
        }
        public ActionResult Index()
        {
            var profiles = from profile in db.Profiles orderby profile.ProfileId select profile;
            return View(profiles);
        }
        public ActionResult View(int id)
        {
            
            var profile = db.Profiles.Where(x => x.ProfileId == id).FirstOrDefault();
            var images = db.Images.Where(x => x.ProfileId == profile.ProfileId).ToList();
            CustomModelsClass.ViewProfileClass viewProfile = new CustomModelsClass.ViewProfileClass();
            viewProfile.profile = profile;
            viewProfile.images = images;
            return View(viewProfile);
        }
        public ActionResult Edit(int id)
        {
            var Profile = db.Profiles.Where(p => p.ProfileId == id).FirstOrDefault();
            return View(Profile);
        }
        [HttpPut]
        public ActionResult Edit(int id, Profile requestProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Profile profile = db.Profiles.Find(id);
                    if (TryUpdateModel(Profile))
                    {
                        
                        profile.Name = requestProfile.Name;
                        profile.Prename = requestProfile.Prename;
                        profile.Nickname = requestProfile.Nickname;
                        profile.City = requestProfile.City;
                        profile.Country = requestProfile.Country;
                        profile.Language = requestProfile.Language;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
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
        public ActionResult AddPhoto()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPhoto(int id,Image image)
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
            image.ProfileId = id;
            db.Images.Add(image);
            db.SaveChanges();
            ModelState.Clear();
            return View();
        }
    }
}