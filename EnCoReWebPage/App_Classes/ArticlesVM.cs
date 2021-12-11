using EnCoReWebPage.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnCoReWebPage.App_Classes
{
    public class ArticlesVM
    {
        public Guid Id { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]        
        public string Libelle { get; set; }
        public string Parution { get; set; }
        public bool Valider { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [DisplayName("Catégorie")]
        public Guid CategorieId { get; set; }

        public string Categorie { get; set; }
        public string Auteur { get; set; }
        public string NbrCommentaires { get; set; }

        public static ArticlesVM GetArticles(Guid id)
        {
            DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");

            var article = db.Articles.Find(id);

            var articlevm = new ArticlesVM
            {
                Id = article.Id,
                Image = article.Image,
                Categorie = article.Category.Libelle,
                CategorieId = article.CategorieId,
                Titre = article.Titre,
                Libelle = article.Libelle,
                Valider = article.Valider,
                Parution = article.Parution.ToString("dd/MM/yyyy HH:mm", culture),
                Auteur = article.Utilisateur.Prenoms + " " + article.Utilisateur.Nom,
                NbrCommentaires = article.Commentaires.Count(c=>c.Valider).ToString().PadLeft(2, '0')
            };

            return articlevm;
        }

        public static List<ArticlesVM> BlogList(Guid? id_categorie, Guid? id_tag, string rech)
        {
            DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");

            List<ArticlesVM> liste = new List<ArticlesVM>();

            var articles = new List<Article>();

            if (id_tag.HasValue)
            {
                articles = db.ArticleTags.Where(p => p.TagId == id_tag).Select(p => p.Article).ToList();
            }
            else
            {
                articles = db.Articles.ToList();
            }
            
            if (id_categorie.HasValue)
            {
                articles = articles.Where(p => p.CategorieId == id_categorie).ToList();
            }            
            if (!string.IsNullOrWhiteSpace(rech))
            {
                articles = articles.Where(p => p.Titre.ToUpper().Trim().Contains(rech.ToUpper())).ToList();
            }

            foreach (var item in articles.Where(a=>a.Valider).OrderBy(a => a.Parution).ToList())
            {
                var article = new ArticlesVM
                {
                    Id = item.Id,
                    Image = item.Image,
                    Categorie = item.Category.Libelle,
                    Libelle = item.Libelle,
                    Titre = item.Titre,
                    Valider = item.Valider,
                    Parution = item.Parution.ToString("dd.MM.yyyy", culture),
                    Auteur = item.Utilisateur.Prenoms + " " + item.Utilisateur.Nom,
                    NbrCommentaires = item.Commentaires.Count(c => c.Valider).ToString().PadLeft(2, '0')
                };
                liste.Add(article);
            }

            return liste;
        }
    }
}