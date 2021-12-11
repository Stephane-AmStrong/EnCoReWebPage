using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnCoReWebPage.Entities;

namespace EnCoReWebPage.Controllers
{
    public class TagsController : Controller
    {
        DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();

        // GET: Tags
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            return View();
        }

        public ActionResult Read()
        {
            var tags = db.Tags.OrderBy(p => p.Libelle).ToList();
            return PartialView(tags);
        }

        // Create Partial
        public ActionResult Form(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            if (id != null)
            {
                var tag = db.Tags.Find(id);
                ViewBag.Id = id.ToString();
                ViewBag.Libelle = tag.Libelle;
            }

            return PartialView();
        }

        // POST : Save
        [HttpPost]
        public ActionResult Save(Guid? id, string Libelle)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            // Control champ vide
            if (string.IsNullOrWhiteSpace(Libelle))
            {
                return Content("Ce champ est requis");
            }

            if (id != null)
            {
                var tag = db.Tags.Find(id);

                // Control existant
                var existe = db.Tags.Where(p => p.Libelle.ToUpper().Trim() == Libelle.ToUpper().Trim() && p.Id != tag.Id).ToList();
                if (existe.Count != 0)
                {
                    return Content("Cette tag existe déjà");
                }

                // Modif. Tag
                tag.Libelle = Libelle;

                db.SaveChanges();

                return Content("OK");
            }
            else
            {
                // Control existant
                var existe = db.Tags.Where(t => t.Libelle.ToUpper().Trim() == Libelle.ToUpper().Trim()).ToList();
                if (existe.Count != 0)
                {
                    return Content("Cette tag existe déjà");
                }

                // Enr. Tag
                var tag = new Tag { Id = Guid.NewGuid(), Libelle = Libelle };
                db.Tags.Add(tag);

                db.SaveChanges();

                return Content("OK");
            }
        }

        public ActionResult Delete(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return PartialView(tag);
        }


        // POST: Roles/Delete/5
        [HttpPost/*, ActionName("Delete")*/]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            db.SaveChanges();
            return Content("OK");
        }

    }
}
