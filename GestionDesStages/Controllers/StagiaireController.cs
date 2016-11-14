using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDesStages.Models;
using PagedList;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace GestionDesStages.Controllers
{
    public class StagiaireController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stagiaire
        
        public JsonResult GetStageByDepartment(String Id)
        {

            //var Stages = from s in db.Stages
            //             where s.Departement.Nom == Depratment
            //             select s;
            List<String> stage = db.Stages.Where(s => s.Departement.Nom == Id && s.Disponible).Select(s=>s.Nom).ToList();

            //var Stages = db.Stages.Where(s => s.Departement.Nom == Depratment);
          
            return Json(stage, JsonRequestBehavior.AllowGet);

           

        }

        public ActionResult Index(String tags, string currentFilter,int? page)
        {
            if (tags != null)
            {

                page = 1;
            }
            else {
                tags = currentFilter;
            }
            ViewBag.CurrentFilter = tags;


            var Stagiaires  = from s in db.Stagiaires
                              select s;
            if (!String.IsNullOrEmpty(tags))
            {
                Stagiaires = Stagiaires.
                    Where(s => s.Nom.ToUpper()
                    .Contains(tags.ToUpper()) || s.Nom.ToUpper().Contains(tags.ToUpper()));
            }

            Stagiaires = Stagiaires.OrderBy(s => s.Age);

            int pageSize = 3; int pageNumber = (page ?? 1);
            return View(Stagiaires.ToPagedList(pageNumber, pageSize));
           

        }
        public JsonResult AutoComplete(string search)
        {

            List<String> Noms = db.Stagiaires.Where(s => s.Nom.StartsWith(search)).Select(s => s.Nom).ToList();

            ////var Nom = (from N in db.Stagiaires.ToList()
            //           where N.Nom.StartsWith(search)
            //           select new { N.Nom });


            //IEnumerable<String> Noms = Nom.ToList();




            return Json(Noms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewLyubomir(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stage stage = db.Stages.Find(id);
            if (stage == null)
            {
                return HttpNotFound();
            }
          
            return PartialView("_Lyubomir",stage);
        }

        // GET: Stagiaire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stagiaire stagiaire = db.Stagiaires.Find(id);
            if (stagiaire == null)
            {
                return HttpNotFound();
            }
            return View(stagiaire);
        }

        // GET: Stagiaire/Create
        public ActionResult Create()
        {
            //IEnumerable<Stage> Stages = db.Stages.Where(s => s.Disponible).ToList();
            IEnumerable<String> Departement = db.Departements.Select(d=>d.Nom).ToList();
            IEnumerable<String> Stages = new List<String> ();
            ViewBag.DepartementID = new SelectList(Departement,"");
            ViewBag.StageID = new SelectList(Stages,"");
            //ViewBag.Departement = Departement;
            return View();
        }

        // POST: Stagiaire/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StagiaireID,Nom,Prenom,Age,DateDeNaissance,LieuDeNaissance,Email,Password,Student")] Stagiaire stagiaire,
           String DepartementID,String StageID )
        {

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationSignInManager signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
           
                var user = new ApplicationUser { UserName = stagiaire.Email, Email = stagiaire.Email };
                var result = await userManager.CreateAsync(user, stagiaire.Password);



            if (ModelState.IsValid)
            {
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    List<Departement> AllDepartements = db.Departements.ToList();
                    var exist = false;
                    var existeStage = false;
                    foreach (var dep in AllDepartements)
                    {
                        List<Stage> stages = db.Stages.Where(s => s.Departement.Nom == dep.Nom && s.Disponible).ToList();

                        if (dep.Nom.Equals(DepartementID))
                        {
                            exist = true;

                            foreach (var s in stages)
                            {
                                if (s.Nom.Equals(StageID))
                                {
                                    existeStage = true;
                                }
                            }


                        }
                    }
                    if (!exist || !existeStage)
                    {
                        IEnumerable<String> Departement = db.Departements.Select(d => d.Nom).ToList();
                        IEnumerable<String> Stages = new List<String>();
                        ViewBag.DepartementID = new SelectList(Departement, "");
                        ViewBag.StageID = new SelectList(Stages, "");
                        return View(stagiaire);
                    }

                    //Departement departement = db.Departements.Where(d => d.Nom == DepartementID).FirstOrDefault();


                    stagiaire.Stages = new List<Stage>();

                    Stage stage = db.Stages.Where(s => s.Nom == StageID).FirstOrDefault();
                    stagiaire.Stages.Add(stage);

                    stage.Disponible = false;
                    db.Entry(stage).State = EntityState.Modified;

                    db.Stagiaires.Add(stagiaire);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(stagiaire);
        }

        // GET: Stagiaire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stagiaire stagiaire = db.Stagiaires.Find(id);
            if (stagiaire == null)
            {
                return HttpNotFound();
            }
            return View(stagiaire);
        }

        // POST: Stagiaire/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StagiaireID,Nom,Prenom,Age,DateDeNaissance,LieuDeNaissance,Student")] Stagiaire stagiaire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stagiaire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stagiaire);
        }

        // GET: Stagiaire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stagiaire stagiaire = db.Stagiaires.Find(id);
            if (stagiaire == null)
            {
                return HttpNotFound();
            }
            return View(stagiaire);
        }

        // POST: Stagiaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stagiaire stagiaire = db.Stagiaires.Find(id);
            IEnumerable<Stage> stages = stagiaire.Stages;

            foreach (var stage in stages)
            {
                stage.Disponible = true;
                db.Entry(stage).State = EntityState.Modified;
            }
            db.Entry(stagiaire).State = EntityState.Modified;
            db.Stagiaires.Remove(stagiaire);
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
