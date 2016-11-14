using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDesStages.Models;
using GestionDesStages.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using GestionDesStages.DAL;

namespace GestionDesStages.Controllers
{
    public class EncadrantController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Encadrant
       
        public ActionResult Index(int? id, int? stageID,string sortOrder)
        {
            var viewModel = new EncadrantIndexData();
            //viewModel.Encadrants = db.Encadrants.Include(i => i.Bureau)
            //    .Include(i => i.Stages.Select(c => c.Departement))
            //    .OrderBy(i => i.Prenom);

            viewModel.Encadrants = unitOfWork.EncadrantRepository.Get(null,
                orderBy: q => q.OrderBy(d => d.Prenom),
                includeProperties: "Bureau, Stages, Departement");


            if (id != null)
            {
                ViewBag.EncadrantID = id.Value;
                viewModel.Stages = viewModel.Encadrants
                    .Where(i => i.EncadrantID == id.Value)
                    .Single()
                    .Stages;
            }
            if (stageID != null) {
                ViewBag.StageID = stageID.Value;
                /* viewModel.Stagiaires = viewModel.Stages.Where(x => x.StageID == stageID).Single().Stagiaire;*/
                var SelectecStage = viewModel.Stages.Where(x => x.StageID == stageID).Single();
                //db.Entry(SelectecStage).Collection(s => s.Stagiaire).Load();
                viewModel.Stagiaires = SelectecStage.Stagiaire;
            }
            return View(viewModel);
        }
        // GET: Encadrant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encadrant encadrant = unitOfWork.EncadrantRepository.GetByID(id);
            //Encadrant encadrant = db.Encadrants.Find(id);
            if (encadrant == null)
            {
                return HttpNotFound();
            }
            return View(encadrant);
        }

        // GET: Encadrant/Create
        public ActionResult Create(object SelectedStage=null)
        {
            var Encadrant = new Encadrant();
            Encadrant.Stages = new List<Stage>();
            /// PopulateAssignedStageData(Encadrant);
            /// 
            var Stages = unitOfWork.StageRepository.Get(orderBy: e => e.OrderBy(o => o.Nom));
            //var Stages = from d in db.Stages
            //                       orderby d.Nom
            //                       select d;
            var s= unitOfWork.StageRepository.Get(orderBy: e => e.OrderBy(o => o.Nom)).Select(m=>m.Nom);
            //var s = from d in db.Stages
            //             orderby d.Nom
            //             select d.Nom;


            ViewBag.StageID = new SelectList(Stages, "StageID", "Nom", "");
            ViewBag.Stages = s;

            return View();
        }

        // POST: Encadrant/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EncadrantID,Nom,Prenom,DateEmbauche,Email,Password,Bureau")] Encadrant encadrant,
            string[] StageID
            )
        {
            if (StageID != null)
            {

                encadrant.Stages = new List<Stage>();
                foreach (var stage in StageID)
                {
                    var stageToAdd = db.Stages.Find(int.Parse(stage));
                    //var stageToAdd = db.Stages.Find(int.Parse(stage));
                    encadrant.Stages.Add(stageToAdd);
                }
            }
            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            if (ModelState.IsValid)
            {
                    var user = new ApplicationUser { UserName = encadrant.Email, Email = encadrant.Email };
                    var result = await userManager.CreateAsync(user,encadrant.Password);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    db.Encadrants.Add(encadrant);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                 
            }

            PopulateAssignedStageData(encadrant);
            return View(encadrant);
        }

       


        // GET: Encadrant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encadrant encadrant = db.Encadrants.Include(e => e.Bureau).
                Include(e=>e.Stages).
                Where(e => e.EncadrantID == id).Single();

            PopulateAssignedStageData(encadrant);
            if (encadrant == null)
            {
                return HttpNotFound();
            }
           
            return View(encadrant);
        }
        private void PopulateAssignedStageData(Encadrant encadrant)
        {
            var allStages = db.Stages;
            var EncadrantStages = new HashSet<int>(encadrant.Stages.Select(c => c.StageID));
            var viewModel = new List<AssignedStageData>();
            foreach (var stage in allStages)
            {
                viewModel.Add(new AssignedStageData
                {
                    StageID = stage.StageID,
                    Titre = stage.Nom,
                    Assigned = EncadrantStages.Contains(stage.StageID)
                });
            }
            ViewBag.stages = viewModel;
            ViewBag.StageID = new SelectList(viewModel, "StageID", "Titre");
            
        }

        // POST: Encadrant/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.

        //public ActionResult Edit([Bind(Include = "EncadrantID,Nom,Prenom,DateEmbauche")] Encadrant encadrant)
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost( int? id, string[] StageID,String Password)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var EncadrantToUpdate = db.Encadrants.Include(i => i.Bureau).
                Include(i=>i.Stages).
                Where(i => i.EncadrantID == id).Single();

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

            if (TryUpdateModel(EncadrantToUpdate, "", new string[] { "Nom", "Prenom", "DateEmbauche", "Bureau","Email","Password" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(EncadrantToUpdate.Bureau.Localisation)) {
                        EncadrantToUpdate.Bureau = null;
                    }

                    var user = await userManager.FindByNameAsync(EncadrantToUpdate.Email);
                    if (user == null)
                    {
                        // Ne révélez pas que l'utilisateur n'existe pas
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                    var result = await userManager.ResetPasswordAsync(user.Id, code, Password);

                  
                    UpdateInstructorCourses(StageID, EncadrantToUpdate);

                    db.Entry(EncadrantToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {          //Log the error (uncomment dex variable name and add a line here to write a log.      
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedStageData(EncadrantToUpdate);
            return View(EncadrantToUpdate);
        }


        private void UpdateInstructorCourses(string[] selectedStages, Encadrant EncadrantToUpdate)
        {
            if (selectedStages == null) {
                EncadrantToUpdate.Stages = new List<Stage>();
                return;
            }
            var selectedStage = new HashSet<string>(selectedStages);
            var EncadrantStages = new HashSet<int>(EncadrantToUpdate.Stages.Select(c => c.StageID));
            foreach (var stage in db.Stages) {
                if (selectedStage.Contains(stage.StageID.ToString()))
                {
                    if (!EncadrantStages.Contains(stage.StageID)) {
                        EncadrantToUpdate.Stages.Add(stage);
                    }
                }
                else 
                    if (EncadrantStages.Contains(stage.StageID)) {
                    EncadrantToUpdate.Stages.Remove(stage);
                    }
                }
            }
        
        // GET: Encadrant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encadrant encadrant = unitOfWork.EncadrantRepository.GetByID(id);
            //Encadrant encadrant = db.Encadrants.Find(id);
            if (encadrant == null)
            {
                return HttpNotFound();
            }
            return View(encadrant);
        }

        // POST: Encadrant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Encadrant encadrant = db.Encadrants.Find(id);

            //Encadrant Encadrant = db.Encadrants.Include(i => i.Bureau).
            //    Where(i => i.EncadrantID == id).Single();

            Encadrant Encadrant = unitOfWork.EncadrantRepository.Get(filter: e => e.EncadrantID == id,includeProperties: "Bureau").Single();
            Encadrant.Bureau = null;
            unitOfWork.EncadrantRepository.Delete(Encadrant);
            unitOfWork.Save();
            //db.Encadrants.Remove(Encadrant);
            //db.SaveChanges();
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
