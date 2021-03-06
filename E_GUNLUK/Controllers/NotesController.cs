﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_GUNLUK.Models;
using Microsoft.AspNet.Identity;

namespace E_GUNLUK.Controllers
{
    public class NotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // HOMEPAGE
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User!=null)
            {
                var userid = User.Identity.GetUserId();

                var user = db.Users.FirstOrDefault(u => u.Id == userid);
                var noteslist = db.notes.ToList().Where(n => n.NoteTaker == user);
                return View(noteslist);

            }
            return View();
        }

         // /Notes/Details/id
        public ActionResult Details(int? id)
        {

            Note note = db.notes.Include(u => u.NoteTaker).SingleOrDefault(i => i.NoteId == id);


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (note == null)
            {
                return HttpNotFound();
            }
            ////List<Comments> commentlist = db.comments.ToList();
            
            //ViewBag.comments = commentlist;

            return View(note);
        }

        //  Notes/Create ** NEW NOTE **
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note viewModel)
        {
            var userid = User.Identity.GetUserId();

            var user = db.Users.Single(u => u.Id == userid);
            var note = new Note {
                NoteTaker = user,
                NoteDate = DateTime.Now,
                Title = viewModel.Title,
                NoteText = viewModel.NoteText,
                PubOrPvt = viewModel.PubOrPvt
            };
                //ViewBag.HtmlContent = viewModel.noteviewmodel.NoteText;
                db.notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");

        }

        //  Notes/Edit/id
        public ActionResult Edit(int? id)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.Single(u => u.Id == userid);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.notes.Find(id);
            //db.notes.FirstOrDefault(n=>n.NoteTaker.Id==user.Id);
            
            if (note == null)
            {
                return HttpNotFound();
            }
            else if(user!=note.NoteTaker)
            {
                return PartialView("~/Views/Notes/NotAuthorized.cshtml", null);
            }
            return View(note);
        }

        // POST: Notes/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoteId,Title,NoteText,PubOrPvt")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
               db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Notes/Delete/id
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.notes.Find(id);

            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.notes.Find(id);
            db.notes.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
