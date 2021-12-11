using EnCoReWebPage.App_Classes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnCoReWebPage.Controllers
{
    public class HomeController : Controller
    {
        EnCoReWebPage.Entities.DB_BlogEnCoReEntities db = new Entities.DB_BlogEnCoReEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult qui_sommes_nous()
        {
            return View();
        }

        public ActionResult notre_organigramme()
        {
            return View();
        }

        public ActionResult notre_fonctionnement()
        {
            return View();
        }

        public ActionResult nos_referents()
        {
            return View();
        }

        public ActionResult notre_projet()
        {
            return View();
        }
























        public ActionResult History()
        {
            return View();
        }

        public ActionResult Portfolio1()
        {
            return View();
        }
        
        public ActionResult Portfolio2()
        {
            return View();
        }
        
        public ActionResult PortfolioMasonry()
        {
            return View();
        }
         
        public ActionResult Faqs()
        {
            return View();
        }

        //public ActionResult BlogListSidebar()
        //{
        //    return View();
        //}
        
        public ActionResult BlogGridSidebar()
        {
            return View();
        }

        public ActionResult ReadArticles(Guid? id_categorie, Guid? id_tag, string rech, int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var articles = ArticlesVM.BlogList(id_categorie, id_tag, rech);

            return PartialView(articles.OrderByDescending(p => p.Parution).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DetailArticle(Guid id)
        {
            var article = db.Articles.Find(id);
            return View(article);
        }
        
        public ActionResult ShowComments(Guid idArticle)
        {
            var comments = db.Commentaires.Where(c => c.ArticleId == idArticle && c.Valider).OrderBy(c => c.Date).ToList();
            return PartialView(comments);
        }

        public ActionResult SideBar()
        {
            return PartialView();
        }
        
        //public ActionResult EventsList()
        //{
        //    return View();
        //}
        
        //public ActionResult EventsGrid()
        //{
        //    return View();
        //}
        
        //public ActionResult EventDetails()
        //{
        //    return View();
        //}
        
        public ActionResult CityNews()
        {
            return View();
        }
        
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostContact(string Nom, string Email, string Objet, string Message)
        {
            return Content("OK");
        }
    }
}