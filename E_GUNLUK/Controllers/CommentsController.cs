﻿
using E_GUNLUK.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_GUNLUK.Controllers
{
    public class CommentsController : Controller

    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        // Create Comment ( Partial View )
        [HttpGet] //GET
  
        public ActionResult Comment(int id)
        {
            var newComment = new Comments();
            newComment.whichNote = id; // This would appear in Details.cshtml

            return View(newComment);
        }

        [HttpPost] //POST
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(Comments viewModel, int id)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Single(u => u.Id == userid);

            var comment = new Comments
            {
                commentator = user,
                commentDate = DateTime.Now,
                theComment = viewModel.theComment,
                whichNote = id
            };
                       

                db.comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Notes", new { id = id });

        }

        public ActionResult CommentsList(int id)
        {
            var commentlist = db.comments.Where(m => m.whichNote == id).Include(x=>x.commentator).ToList();
            if (commentlist == null)
            {
                return View("No comments are available!");
            }

            foreach (var item in commentlist)
            {

                // var query = db.comments.Single(m=> m.whichNote == id);

                if (item.whichNote == id)
                {
                    return View(commentlist);
                }


            }

            /*
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
            */
            return View();
        }
    }
}