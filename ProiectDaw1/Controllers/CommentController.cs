using Microsoft.AspNet.Identity;
using ProiectDaw1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProiectDaw1.Models.Comment;
using static ProiectDaw1.Models.Profile;

namespace ProiectDaw1.Controllers
{
    public class CommentController : Controller
    {
        private CommentDBContext db = new CommentDBContext();
        private ProfileDBContext db1 = new ProfileDBContext();
        [HttpPost]
        public ActionResult Add(Comment comment)
        {
            if (comment.comment == null)
            {
                return Redirect("/Image/View/" + comment.ImageId);
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var profile = db1.Profiles.Where(x => x.UserId == userId).FirstOrDefault();
                comment.ProfileId = profile.ProfileId;
                comment.visible = 1;
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Image/View/" + comment.ImageId);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string UID = User.Identity.GetUserId();
            var comment = db.Comments.Where(i => i.Id == id).FirstOrDefault();
            var Profile = db1.Profiles.Where(x => x.ProfileId == comment.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui comentariu care nu va apartine";
                return Redirect("/Image/View/" + comment.ImageId);
            }
            else
            {
                return View(comment);
            }
        }
        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            string UID = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment = db.Comments.Find(id);
                    var Profile = db1.Profiles.Where(x => x.ProfileId == comment.ProfileId).FirstOrDefault();
                    if (!Profile.UserId.Equals(UID))
                    {
                        TempData["mesage"] = "Nu aveti dreptul sa faceti modificari unui comentariu care nu va apartine";
                        return Redirect("/Image/View/" + comment.ImageId);
                    }
                    else
                    {
                        if (TryUpdateModel(comment))
                        {
                            comment.comment = requestComment.comment;
                            db.SaveChanges();
                        }
                        return Redirect("/Image/View/" + comment.ImageId);
                    }
                }
                else
                {
                    return View(requestComment);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        // GET: Comment
        public ActionResult Delete(int id)
        {
            string UID = User.Identity.GetUserId();
            Comment comment = new Comment();
            comment = db.Comments.Where(x => x.Id == id).FirstOrDefault();
            var Profile = db1.Profiles.Where(x => x.ProfileId == comment.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti un comentariu care nu va apartine";
                return Redirect("/Image/View/" + comment.ImageId);
            }
            else
            {
                return View(comment);
            }

        }
        [HttpDelete]
        public ActionResult Delete1(int id)
        {
            string UID = User.Identity.GetUserId();
            Comment comment = db.Comments.Find(id);
            var Profile = db1.Profiles.Where(x => x.ProfileId == comment.ProfileId).FirstOrDefault();
            if (!Profile.UserId.Equals(UID))
            {
                TempData["mesage"] = "Nu aveti dreptul sa stergeti un comentariu care nu va apartine";
                return Redirect("/Image/View/" + comment.ImageId);
            }
            else
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return Redirect("/Image/View/" + comment.ImageId);
            }
        }
        public ActionResult Hide(int id)
        {
            try
            {
                Comment comment = db.Comments.Find(id);
                if (ModelState.IsValid)
                {
                    
                    if (TryUpdateModel(comment))
                    {
                        comment.visible = 0;
                        db.SaveChanges();
                    }
                    return Redirect("/Image/View/" + comment.ImageId);
                }
                else
                {
                    return Redirect("/Image/View/" + comment.ImageId);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult Unhide(int id)
        {
            try
            {
                Comment comment = db.Comments.Find(id);
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(comment))
                    {
                        comment.visible = 1;
                        db.SaveChanges();
                    }
                    return Redirect("/Image/View/" + comment.ImageId);
                }
                else
                {
                    return Redirect("/Image/View/" + comment.ImageId);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}