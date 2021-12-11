using EnCoReWebPage.App_Classes;
using EnCoReWebPage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EnCoReWebPage.Controllers
{
    public class ArticlesController : Controller
    {
        DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();
        TimeZoneInfo benin_time = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");

        [ValidateInput(false)]
        // GET: Articles
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            return View();
        }

        public ActionResult Read()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            var user = db.Utilisateurs.Find(Guid.Parse(Session["UserId"].ToString()));
            List<ArticlesVM> articles = new List<ArticlesVM>();

            if (user.Role.Valider_Article)
            {
                foreach (var item in db.Articles.OrderBy(a => a.Parution))
                {
                    var article = new ArticlesVM
                    {
                        Id = item.Id,
                        Image = item.Image,
                        Categorie = item.Category.Libelle,
                        Libelle = item.Libelle,
                        Titre = item.Titre,
                        Valider = item.Valider,
                        Parution = item.Parution.ToString("dd/MM/yyyy HH:mm", culture),
                        Auteur = item.Utilisateur.Prenoms + " " + item.Utilisateur.Nom
                    };
                    articles.Add(article);
                }
            }
            else
            {
                foreach (var item in db.Articles.Where(a => a.UtilisateurId == user.Id).OrderBy(a => a.Parution).ToList())
                {
                    var article = new ArticlesVM
                    {
                        Id = item.Id,
                        Image = item.Image,
                        Categorie = item.Category.Libelle,
                        Libelle = item.Libelle,
                        Titre = item.Titre,
                        Valider = item.Valider,
                        Parution = item.Parution.ToString("dd/MM/yyyy HH:mm", culture),
                        Auteur = item.Utilisateur.Prenoms + " " + item.Utilisateur.Nom
                    };
                    articles.Add(article);
                }
            }

            return PartialView(articles);
        }

        public ActionResult ShowComments(Guid idArticle)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleValider_Commentaire"]) return RedirectToAction("Home", "Index");

            return View(db.Articles.Find(idArticle));
        }


        public ActionResult UnreadComments()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            ViewBag.OnArticlePage = "false";

            return View();
        }

        public ActionResult GetListComment(Guid ? idArticle)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            if (idArticle!=null)
            {
                return PartialView(db.Commentaires.Where(p => p.ArticleId == idArticle).OrderBy(p => p.Date).ToList());
            }
            else
            {
                return PartialView(db.Commentaires.Where(c => c.Valider == false).OrderBy(p => p.Date).ToList());
            }
        }

        public ActionResult AutorizeOrDelete(Guid idArticle, string action = "Valider")
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            var article = db.Articles.Find(idArticle);
            if (article!=null)
            {
                try
                {
                    if (action == "Valider")
                    {
                        article.Valider = true;
                    }
                    else if (action == "Supprimer")
                    {
                        db.Articles.Remove(article);
                    }
                    db.SaveChanges();
                    return Content("OK");
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }

            }
            return null;
        }
        
        public ActionResult AutorizeOrDeleteComment(Guid idComment, string action = "Valider")
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            var comment = db.Commentaires.Find(idComment);
            if (comment!=null)
            {
                try
                {
                    if (action == "Valider")
                    {
                        comment.Valider = true;
                    }
                    else if (action == "Supprimer")
                    {
                        db.Commentaires.Remove(comment);
                    }
                    db.SaveChanges();
                    return Content("OK");
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }

            }
            return null;
        }

        public ActionResult PostComment(string id, string nom, string email, string message)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");


            try
            {
                Commentaire com = new Commentaire
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    CommentateurNomPrenoms = nom,
                    CommentateurEmail = email,
                    Libelle = message,
                    ArticleId = Guid.Parse(id)
                };
                db.Commentaires.Add(com);
                db.SaveChanges();

                return Content("OK");
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }

        // Create Partial
        public ActionResult Form(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            if (id != null)
            {
                var article = ArticlesVM.GetArticles(id.Value);
                ViewBag.CategorieId = new SelectList(db.Categories.OrderBy(p => p.Libelle).ToList(), "Id", "Libelle", article.CategorieId);

                return PartialView(article);
            }
            else
            {
                ViewBag.CategorieId = new SelectList(db.Categories.OrderBy(p => p.Libelle).ToList(), "Id", "Libelle");
                return PartialView();
            }
            
        }

        // POST : Save
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Save(ArticlesVM articleVM, HttpPostedFileBase file)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");


            if (articleVM.Id != null && articleVM.Id != new Guid())
            {
                // Control existant
                
                if (db.Articles.Count(p => p.Libelle.ToUpper().Trim() == articleVM.Libelle.ToUpper().Trim() && p.Id != articleVM.Id) > 0) return Content("Cet article existe déjà");

                if (string.IsNullOrWhiteSpace(articleVM.Titre)) return Content("Le titre est requis");
                if (string.IsNullOrWhiteSpace(articleVM.Libelle)) return Content("Le libellé est requis");
                
               //
                var article = db.Articles.Find(articleVM.Id);
                
                // Modif. Article
                if ((file != null) && (file.ContentType.Contains("image")))
                {
                    string chemin_fichier = Server.MapPath(article.Image);
                    System.IO.File.Delete(chemin_fichier);
                    file.SaveAs(chemin_fichier);
                }

                // Enr. Article
                article.Titre = articleVM.Titre.Trim();
                article.CategorieId = articleVM.CategorieId;
                article.Libelle = articleVM.Libelle.Trim();

                db.SaveChanges();

                return Content("OK");
            }
            else
            {
                // Control existant
                var existe = db.Articles.Where(t => t.Libelle.ToUpper().Trim() == articleVM.Libelle.ToUpper().Trim()).ToList();


                if (string.IsNullOrWhiteSpace(articleVM.Titre)) return Content("Le titre est requis");
                if (string.IsNullOrWhiteSpace(articleVM.Libelle)) return Content("Le libellé est requis");

                if (existe.Count != 0) return Content("Cette article existe déjà");

                if ((file != null) && (file.ContentType.Contains("image")))
                {
                    string chemin_dossier = Server.MapPath("/Photo/");
                    string nom_fichier = Guid.NewGuid().ToString();
                    string extention = "." + file.FileName.Split('.').Last();
                    string new_chemin = chemin_dossier + nom_fichier + extention;
                    articleVM.Image = "/Photo/" + nom_fichier + extention;
                    file.SaveAs(new_chemin);
                }

                // Enr. Article
                var article = new Article
                {
                    Id = Guid.NewGuid(),
                    Titre = articleVM.Titre.Trim(),
                    CategorieId = articleVM.CategorieId,
                    Libelle = articleVM.Libelle.Trim(),
                    Parution = TimeZoneInfo.ConvertTime(DateTime.Now, benin_time),
                    Image = articleVM.Image, 
                    UtilisateurId = (Guid)Session["UserId"]
                };
                db.Articles.Add(article);

                db.SaveChanges();

                return Content("OK");
            }
        }

        public ActionResult Delete(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticlesVM article = ArticlesVM.GetArticles(id.Value); 
            if (article == null)
            {
                return HttpNotFound();
            }
            return PartialView(article);
        }


        public ActionResult CommentDeletionPopup(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commentaire commentaire = db.Commentaires.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            return PartialView(commentaire);
        }


        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleArticle"]) return RedirectToAction("Home", "Index");

            Article article = db.Articles.Find(id);

            // Suppression du fichier image
            db.Commentaires.RemoveRange(article.Commentaires);
            db.Articles.Remove(article);
            db.SaveChanges();
            System.IO.File.Delete(Server.MapPath(article.Image));
            return Content("OK");
        }

    }
}