using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDesStages.Models;
using GestionDesStages.DAL;

namespace GestionDesStages.Controllers
{
    public class DepartementController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Departement
        public ActionResult Index()
        {
            //return View(db.Departements.ToList());
            return View(unitOfWork.DepartementRepository.Get().ToList());
        }

        // GET: Departement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Departement departement = db.Departements.Find(id);

            Departement departement =unitOfWork.DepartementRepository.GetByID(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // GET: Departement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departement/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartementID,Nom")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                //db.Departements.Add(departement);
                unitOfWork.DepartementRepository.Insert(departement);
                //db.SaveChanges();
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(departement);
        }

        // GET: Departement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Departement departement = db.Departements.Find(id);
            Departement departement = unitOfWork.DepartementRepository.GetByID(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // POST: Departement/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartementID,Nom")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.DepartementRepository.Update(departement);
                unitOfWork.Save();
                //db.Entry(departement).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departement);
        }

        // GET: Departement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Departement departement = db.Departements.Find(id);
            Departement departement = unitOfWork.DepartementRepository.GetByID(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // POST: Departement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Departement departement = unitOfWork.DepartementRepository.GetByID(id);
            unitOfWork.DepartementRepository.Delete(departement);
            unitOfWork.Save();
            //Departement departement = db.Departements.Find(id);
            //db.Departements.Remove(departement);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
