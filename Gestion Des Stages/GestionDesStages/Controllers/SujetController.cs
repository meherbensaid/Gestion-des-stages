using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDesStages.Models;

namespace GestionDesStages.Controllers
{
    public class SujetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sujet
        public ActionResult Index()
        {
            var sujets = db.Sujets.Include(s => s.Stage);
            return View(sujets.ToList());
        }

        // GET: Sujet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sujet sujet = db.Sujets.Find(id);
            if (sujet == null)
            {
                return HttpNotFound();
            }
            return View(sujet);
        }

        // GET: Sujet/Create
        public ActionResult Create()
        {

            ViewBag.StageID = new SelectList(db.Stages, "StageID", "Nom");
            return View();
        }

        // POST: Sujet/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StageID,Titre,Description")] Sujet sujet)
        {
            if (ModelState.IsValid)
            {
                db.Sujets.Add(sujet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StageID = new SelectList(db.Stages, "StageID", "Nom", sujet.StageID);
            return View(sujet);
        }

        // GET: Sujet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sujet sujet = db.Sujets.Find(id);
            if (sujet == null)
            {
                return HttpNotFound();
            }
            ViewBag.StageID = new SelectList(db.Stages, "StageID", "Nom", sujet.StageID);
            return View(sujet);
        }

        // POST: Sujet/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StageID,Titre,Description")] Sujet sujet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sujet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StageID = new SelectList(db.Stages, "StageID", "Nom", sujet.StageID);
            return View(sujet);
        }

        // GET: Sujet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sujet sujet = db.Sujets.Find(id);
            if (sujet == null)
            {
                return HttpNotFound();
            }
            return View(sujet);
        }

        // POST: Sujet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sujet sujet = db.Sujets.Find(id);
            db.Sujets.Remove(sujet);
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
