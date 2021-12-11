using EnCoReWebPage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EnCoReWebPage.Controllers
{
    public class CategoriesController : Controller
    {
        DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();

        // GET: Categories
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleCategorie"]) return RedirectToAction("Home", "Index");

            return View();
        }

        public ActionResult Liste()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleCategorie"]) return RedirectToAction("Home", "Index");

            var categories = db.Categories.OrderBy(p => p.Libelle).ToList();
            return PartialView(categories);
        }

        // Create Partial
        public ActionResult FormPartial(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleCategorie"]) return RedirectToAction("Home", "Index");

            if (id != null)
            {
                var categorie = db.Categories.Find(id);
                ViewBag.Id = id.ToString();
                ViewBag.Libelle = categorie.Libelle;
            }

            return PartialView();
        }

        // POST : Save
        [HttpPost]
        public ActionResult Save(Guid? id, string Libelle)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleCategorie"]) return RedirectToAction("Home", "Index");

            // Control champ vide
            if (string.IsNullOrWhiteSpace(Libelle))
            {
                return Content("Ce champ est requis");
            }

            if (id != null)
            {
                var categorie = db.Categories.Find(id);

                // Control existant
                var existe = db.Categories.Where(p => p.Libelle.ToUpper().Trim() == Libelle.ToUpper().Trim() && p.Id != categorie.Id).ToList();
                if (existe.Count != 0)
                {
                    return Content("Cette catégorie existe déjà");
                }

                // Modif. Categorie
                categorie.Libelle = Libelle;

                db.SaveChanges();

                return Content("OK");
            }
            else
            {
                // Control existant
                var existe = db.Categories.Where(p => p.Libelle.ToUpper().Trim() == Libelle.ToUpper().Trim()).ToList();
                if (existe.Count != 0)
                {
                    return Content("Cette catégorie existe déjà");
                }

                // Enr. Categorie
                var categorie = new Category { Id = Guid.NewGuid(), Libelle = Libelle };
                db.Categories.Add(categorie);

                db.SaveChanges();

                return Content("OK");
            }
        }

        public ActionResult Delete(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleCategorie"]) return RedirectToAction("Home", "Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return PartialView(category);
        }


        // POST: Roles/Delete/5
        [HttpPost/*, ActionName("Delete")*/]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleCategorie"]) return RedirectToAction("Home", "Index");

            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return Content("OK");
        }

    }
}