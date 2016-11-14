using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDesStages.Models;
using System.Data.Entity.Infrastructure;
using GestionDesStages.DAL;

namespace GestionDesStages.Controllers
{
    public class StageController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Stage
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //var Stages = db.Stages.Include(s => s.Departement).Include(s=>s.Sujet) ;
            var Stages = unitOfWork.StageRepository.Get(includeProperties: "Departement,Sujet");

            switch (sortOrder)
            {
                case "name_desc":
                    Stages = Stages.OrderByDescending(s => s.Nom);
                    break;
                case "Date":
                    Stages = Stages.OrderBy(s => s.DateDebut);
                    break;
                case "date_desc":
                    Stages = Stages.OrderByDescending(s => s.DateDebut);
                    break;
                default:
                    Stages = Stages.OrderBy(s => s.Nom); break;
            }
            return View(Stages.ToList());
        }

        // GET: Stage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Stage stage = db.Stages.Find(id);
            Stage stage = unitOfWork.StageRepository.GetByID(id);

            if (stage == null)
            {
                return HttpNotFound();
            }
            return View(stage);
        }

        // GET: Stage/Create
        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Stage/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StageID,Nom,DateDebut,DateFin,Type")] Stage stage, Sujet sujet, string DepartementID)
        {
            try {

                if (ModelState.IsValid)
            {
                    stage.valide = false;
                    stage.Sujet = sujet;
                    stage.DepartementID = Int16.Parse(DepartementID);
                    //db.Stages.Add(stage);
                    //db.SaveChanges();
                    unitOfWork.StageRepository.Insert(stage);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
            }
            }
            catch (RetryLimitExceededException /* dex */)
            {       //Log the error (uncomment dex variable name and add a line here to write a log.)   
                ModelState.AddModelError("", 
                    "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateDepartmentsDropDownList(stage.StageID);   

                return View(stage);
        }

        // GET: Stage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Stage stage = db.Stages.Find(id);
            Stage stage = unitOfWork.StageRepository.GetByID(id);

            if (stage == null)
            {
                return HttpNotFound();
            }
            PopulateDepartmentsDropDownList(stage.DepartementID);
            return View(stage);
        }

        // POST: Stage/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StageID,Nom,DateDebut,DateFin,Type,valide,RowVersion,DepartementID")] Stage stage)
        {
            try { 
            if (ModelState.IsValid)
            {
                    //db.Entry(stage).State = EntityState.Modified;
                    //db.SaveChanges();
                    unitOfWork.StageRepository.Update(stage);
                    unitOfWork.Save();
                return RedirectToAction("Index");
            }
            }
            catch (DbUpdateConcurrencyException ex)
            {


                var entry = ex.Entries.Single();
                var clientValues = (Stage)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The Stage was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Stage)databaseEntry.ToObject();
                    if (databaseValues.Nom != clientValues.Nom)
                        ModelState.AddModelError("Nom", "Current value: " + databaseValues.Nom);
                    if (databaseValues.DateDebut != clientValues.DateDebut)
                        ModelState.AddModelError("DateDebut", "Current value: " + String.Format("{0:c}", databaseValues.DateDebut));
                    if (databaseValues.DateFin != clientValues.DateFin)
                        ModelState.AddModelError("DateFin", "Current value: " + String.Format("{0:d}", databaseValues.DateFin));
                    if (databaseValues.Type != clientValues.Type)
                        ModelState.AddModelError("Type", "Current value: " + String.Format("{0:d}", databaseValues.Type));
                    if (databaseValues.valide != clientValues.valide)
                        ModelState.AddModelError("valide", "Current value: " + String.Format("{0:d}", databaseValues.valide));
                    if (databaseValues.DepartementID != clientValues.DepartementID)
                        ModelState.AddModelError("DepartementID", "Current value: " + unitOfWork.StageRepository.GetByID(databaseValues.DepartementID).Nom);
                    ModelState.AddModelError(string.Empty, "Les données que vous avez tenté de modifier ont été modifiées par un autre utilisateur après avoir obtenu la valeur d'origine. L'opération d'édition a été annulée et les valeurs actuelles dans la base de données ont été affichées. Si vous voulez continuer à modifier cet enregistrement, cliquez à nouveau sur le bouton Save. Sinon cliquez sur le lien hypertexte Back to List.");
                    stage.RowVersion = databaseValues.RowVersion;
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {       //Log the error (uncomment dex variable name and add a line here to write a log.)  
                ModelState.AddModelError("",
                    "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateDepartmentsDropDownList(stage.StageID); 
                return View(stage);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null) {
            //var departmentsQuery = from d in db.Departements
            //                       orderby d.Nom
            //                       select d;
            var departmentsQuery = unitOfWork.DepartementRepository.Get(orderBy: d => d.OrderBy(e => e.Nom)).ToList();

            ViewBag.DepartementID = new SelectList(departmentsQuery, "DepartementID", "Nom", selectedDepartment);
        }
        // GET: Stage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Stage stage = db.Stages.Find(id);
            Stage stage = unitOfWork.StageRepository.GetByID(id);
            if (stage == null)
            {
                return HttpNotFound();
            }
            return View(stage);
        }

        // POST: Stage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stage stage = unitOfWork.StageRepository.Get(filter: s=>s.StageID==id,includeProperties:"Sujet").Single();
            unitOfWork.StageRepository.Delete(stage);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
