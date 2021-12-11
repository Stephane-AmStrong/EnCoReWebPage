using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnCoReWebPage.Data;
using EnCoReWebPage.Entities;

namespace EnCoReWebPage.Controllers
{
    public class RolesController : Controller
    {
        DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();

        // GET: Roles
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleRole"]) return RedirectToAction("Home", "Index");

            return View();
        }

        public ActionResult Read()
        {
            return PartialView(db.Roles.ToList());
        }

        // Create Partial
        public ActionResult Form(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleRole"]) return RedirectToAction("Home", "Index");
            
            if (id != null)
            {
                var role = db.Roles.Find(id);

                return PartialView(role);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult Save(Role roleVM)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleRole"]) return RedirectToAction("Home", "Index");

            // Control champ vide
            if (string.IsNullOrWhiteSpace(roleVM.Nom))
            {
                return Content("Le nom est requis");
            }

            if (roleVM.Id != null && roleVM.Id != new Guid())
            {

                if (db.Roles.Any(r => r.Nom == roleVM.Nom && r.Id!= roleVM.Id))
                {
                    return Content("Cette role existe déjà");
                }

                var role = db.Roles.Find(roleVM.Id);

                // Modif. Role
                role.Nom = roleVM.Nom;
                role.Article = roleVM.Article;
                role.Categorie = roleVM.Categorie;
                role.Role_ = roleVM.Role_;
                role.Valider_Article = roleVM.Valider_Article;
                role.Valider_Commentaire = roleVM.Valider_Commentaire;
                role.Utilisateur = roleVM.Utilisateur;
                db.SaveChanges();

                return Content("OK");
            }
            else
            {

                if (db.Roles.Any(r => r.Nom == roleVM.Nom)) return Content("Cette role existe déjà");

                Role role = new Role()
                {
                    Id = Guid.NewGuid(),
                    Nom = roleVM.Nom,
                    Article = roleVM.Article,
                    Categorie = roleVM.Categorie,
                    Role_ = roleVM.Role_,
                    Valider_Article = roleVM.Valider_Article,
                    Valider_Commentaire = roleVM.Valider_Commentaire,
                    Utilisateur = roleVM.Utilisateur,
                };

                db.Roles.Add(role);
                db.SaveChanges();
                return Content("OK");
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleRole"]) return RedirectToAction("Home", "Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            if (role.Nom == "Admin") return Content("Admin");

            return PartialView(role);
        }

        // POST: Roles/Delete/5
        [HttpPost/*, ActionName("Delete")*/]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleRole"]) return RedirectToAction("Home", "Index");

            Role role = db.Roles.Find(id);

            db.Roles.Remove(role);
            db.SaveChanges();
            return Content("OK");
        }
    }
}
