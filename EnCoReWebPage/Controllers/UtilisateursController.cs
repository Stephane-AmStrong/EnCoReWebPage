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
using EnCoReWebPage.App_Classes;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Routing;

namespace EnCoReWebPage.Controllers
{
    public class UtilisateursController : Controller
    {
        DB_BlogEnCoReEntities db = new DB_BlogEnCoReEntities();
        TimeZoneInfo benin_time = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");

        public ActionResult Login()
        {
            ViewBag.email = "";
            ViewBag.pwd = "";
            
            if (!db.Roles.Any(r => r.Nom == "Admin"))
            {
                db.Roles.Add(new Role
                {
                    Id = Guid.NewGuid(),
                    Nom = "Admin",
                    Article = true,
                    Categorie = true,
                    Role_ = true,
                    Valider_Article = true,
                    Valider_Commentaire = true,
                    Utilisateur = true,
                });
                db.SaveChanges();
            }

            if (!db.Utilisateurs.Any())
            {
                Clear("UserId"); 
                return RedirectToAction("RegisterAdmin");
            }

            try
            {
                HttpCookie cookieUser = Request.Cookies["UserId"];
                if (!string.IsNullOrWhiteSpace(cookieUser.Value))
                {
                    Guid userId = Guid.Parse(cookieUser.Value);

                    var user = db.Utilisateurs.Where(u => u.Id == userId && u.Actif == true).FirstOrDefault();
                    if (user != null)
                    {
                        this.Session["UserId"] = user.Id;
                        this.Session["UserNom"] = user.Nom + " " + user.Prenoms;
                        this.Session["UserPhoto"] = user.Photo;
                        this.Session["UserRole"] = user.Role.Nom;

                        this.Session["RoleArticle"] = user.Role.Article;
                        this.Session["RoleCategorie"] = user.Role.Categorie;
                        this.Session["RoleRole"] = user.Role.Role_;
                        this.Session["RoleValider_Article"] = user.Role.Valider_Article;
                        this.Session["RoleValider_Commentaire"] = user.Role.Valider_Commentaire;
                        this.Session["RoleUtilisateur"] = user.Role.Utilisateur;

                        return RedirectToAction("Dashboard", "Utilisateurs");
                    }
                }
            }
            catch (Exception)
            {

            }


            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pwd)
        {
            //Trouver l'utilisateur
            var user = new Utilisateur();
            //ViewBag.email = login;
            //ViewBag.pwd = pwd;
            try
            {
                user = db.Utilisateurs.Where(u => (u.Email == email)).First();

                if (!string.IsNullOrWhiteSpace(user.PWD) && !user.Actif)
                {
                    ModelState.AddModelError("", "contactez l'administrateur, votre compte n'est pas actif");
                    return View();
                }
                else if (string.IsNullOrWhiteSpace(user.PWD) && !user.Actif)
                {
                    //ModelState.AddModelError("", "ceci est votre première connection, veuillez modifier");
                    //return RedirectToAction("RegisterUser", new RouteValueDictionary( new { controller = "Utilisateur", action = "RegisterUser", idUtilisateur = userVM.Id }));
                    
                    return RedirectToAction("RegisterUser", new { idUtilisateur = user.Id } );

                }
                else if (user.Actif && SimpleHash.VerifyHash(pwd, "SHA512", user.PWD))
                {
                    this.Session["UserId"] = user.Id;
                    this.Session["UserNom"] = user.Nom + " " + user.Prenoms;
                    this.Session["UserPhoto"] = user.Photo;
                    this.Session["UserRole"] = user.Role.Nom;

                    this.Session["RoleArticle"] = user.Role.Article;
                    this.Session["RoleCategorie"] = user.Role.Categorie;
                    this.Session["RoleRole"] = user.Role.Role_;
                    this.Session["RoleValider_Article"] = user.Role.Valider_Article;
                    this.Session["RoleValider_Commentaire"] = user.Role.Valider_Commentaire;
                    this.Session["RoleUtilisateur"] = user.Role.Utilisateur;

                    //creat cookie
                    HttpCookie cookieUser = new HttpCookie("UserId", user.Id.ToString());
                    cookieUser.Expires.AddDays(7);

                    //commit cookie
                    HttpContext.Response.SetCookie(cookieUser);

                    return RedirectToAction("Dashboard", "Utilisateurs");
                }
                else
                {
                    ModelState.AddModelError("", "Identifiant ou mot de passe incorrect");
                    return View();
                }
            }
            catch
            {
                try
                {
                    db.Utilisateurs.Where(u => (u.Login == email || u.Email == email)).First();
                    try
                    {
                        db.Utilisateurs.Where(u => (u.Login == email || u.Email == email) && u.Actif == false).First();
                        ModelState.AddModelError("", "Votre compte n'est pas actif");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Identifiant ou mot de passe incorrect");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Identifiant ou mot de passe incorrect");
                }
            }
            return View();
        }

        public ActionResult Deconnexion()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();

            Clear("UserId");

            return RedirectToAction("Login");
        }

        public void Clear(string key)
        {
            HttpCookie cookieUser = new HttpCookie(key);
            cookieUser.Expires = DateTime.Now.AddMilliseconds(-2);

            //commit cookie
            HttpContext.Response.SetCookie(cookieUser);
        }


        [HttpPost]
        public ActionResult Register(UserVM user)
        {
            if (string.IsNullOrEmpty(user.Nom))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Nom");
                return View();
            }

            if (string.IsNullOrEmpty(user.Prenoms))
            {
                ModelState.AddModelError("", "Veuillez saisir vos Prénoms");
                return View();
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Email");
                return View();
            }

            if (!Utilities.IsValidEmail(user.Email))
            {
                ModelState.AddModelError("", "Veuillez entrer un Email valide");
                return View();
            }

            if (string.IsNullOrEmpty(user.PWD))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Mot de passe");
                return View();
            }

            if (string.IsNullOrEmpty(user.PWDcf))
            {
                ModelState.AddModelError("", "Veuillez confirmer votre mot de passe");
                return View();
            }
            if (user.PWD != user.PWDcf)
            {
                ModelState.AddModelError("", "Les 2 mots de passe saisis ne sont pas conformes");
                return View();
            }

            //2 - Crypter mot de passe à l'enregistrement dans la base de donnée

            var role = db.Roles.Where(r => r.Nom == "Admin").First();
            var users = new Utilisateur { Id = Guid.NewGuid(), Nom = user.Nom, Email = user.Email, Prenoms = user.Prenoms, PWD = user.PWD, RoleId = role.Id, Actif = true };
            string passhashed = SimpleHash.ComputeHash(users.PWD, "SHA512", null);
            users.PWD = passhashed;
            db.Utilisateurs.Add(users);

            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult RegisterAdmin()
        {
            ViewBag.nom = "";
            ViewBag.prenom = "";
            ViewBag.email = "";
            ViewBag.pwd = "";
            ViewBag.pwdcf = "";

            if (!db.Roles.Any(r => r.Nom == "Admin"))
            {
                db.Roles.Add(new Role
                {
                    Id = Guid.NewGuid(),
                    Nom = "Admin",
                    Article = true,
                    Categorie = true,
                    Role_ = true,
                    Utilisateur = true,
                });
                db.SaveChanges();
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult RegisterAdmin(UserVM admin)
        {
            ViewBag.nom = admin.Nom;
            ViewBag.prenom = admin.Prenoms;
            ViewBag.email = admin.Email;
            ViewBag.pwd = admin.PWD;
            ViewBag.pwdcf = admin.PWDcf;

            if (string.IsNullOrEmpty(admin.Nom))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Nom");
                return View();
            }

            if (string.IsNullOrEmpty(admin.Prenoms))
            {
                ModelState.AddModelError("", "Veuillez saisir vos Prénoms");
                return View();
            }

            if (string.IsNullOrEmpty(admin.Email))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Login");
                return View();
            }

            if (!Utilities.IsValidEmail(admin.Email))
            {
                ModelState.AddModelError("", "Veuillez entrer un Email valide");
                return View();
            }

            if (string.IsNullOrEmpty(admin.PWD))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Mot de passe");
                return View();
            }

            if (string.IsNullOrEmpty(admin.PWDcf))
            {
                ModelState.AddModelError("", "Veuillez confirmer votre mot de passe");
                return View();
            }
            if (admin.PWD != admin.PWDcf)
            {
                ModelState.AddModelError("", "Les 2 mots de passe saisis ne sont pas conformes");
                return View();
            }

            //2 - Crypter mot de passe à l'enregistrement dans la base de donnée

            var role = db.Roles.Where(r => r.Nom == "Admin").First();
            var user = new Utilisateur { Id = Guid.NewGuid(), Nom = admin.Nom, Email = admin.Email, Prenoms = admin.Prenoms, PWD = admin.PWD, RoleId = role.Id, Actif = true };
            string passhashed = SimpleHash.ComputeHash(user.PWD, "SHA512", null);
            user.PWD = passhashed;
            db.Utilisateurs.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult RegisterUser(Guid idUtilisateur)
        {
            return View(new UserVM(db.Utilisateurs.Find(idUtilisateur)));
        }

        [HttpPost]
        public ActionResult RegisterUser(UserVM userVM)
        {
            if (string.IsNullOrEmpty(userVM.Nom))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Nom");
                return View();
            }

            if (string.IsNullOrEmpty(userVM.Prenoms))
            {
                ModelState.AddModelError("", "Veuillez saisir vos Prénoms");
                return View();
            }

            if (string.IsNullOrEmpty(userVM.PWD))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Mot de passe");
                return View();
            }

            if (string.IsNullOrEmpty(userVM.PWDcf))
            {
                ModelState.AddModelError("", "Veuillez confirmer votre mot de passe");
                return View();
            }
            if (userVM.PWD != userVM.PWDcf)
            {
                ModelState.AddModelError("", "Les 2 mots de passe saisis ne sont pas conformes");
                return View();
            }

            var user = db.Utilisateurs.Find(userVM.Id);

            user.Nom = userVM.Nom;
            user.Prenoms = userVM.Prenoms;
            user.Apropos = userVM.Apropos;
            user.Blog = userVM.Blog;
            user.PWD = SimpleHash.ComputeHash(userVM.PWD, "SHA512", null);

            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Dashboard()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            return View();
        }

        public ActionResult Profil()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            Guid userId = Guid.Parse((Session["UserId"]).ToString());
            return View(db.Utilisateurs.Find(userId));
        }

        public ActionResult ProfilCard()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            Guid userId = Guid.Parse((Session["UserId"]).ToString());
            return PartialView(db.Utilisateurs.Find(userId));
        }

        [HttpPost]
        public ActionResult Profil(Utilisateur user)
        {
            return View();
        }

        // GET: Utilisateurs
        public ActionResult Index()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleUtilisateur"]) return RedirectToAction("Home", "Index");

            return View();
        }

        public ActionResult Read()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleUtilisateur"]) return RedirectToAction("Home", "Index");
            
            return PartialView(db.Utilisateurs.OrderBy(u => u.Nom).ThenBy(u => u.Prenoms).ToList());
        }

        public ActionResult Form(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleUtilisateur"]) return RedirectToAction("Home", "Index");
            
            if (!db.Roles.Any(r => r.Nom == "Admin"))
            {
                db.Roles.Add(new Role
                {
                    Id = Guid.NewGuid(),
                    Nom = "Admin",
                    Article = true,
                    Categorie = true,
                    Role_ = true,
                    Valider_Article = true,
                    Valider_Commentaire = true,
                    Utilisateur = true,
                });
                db.SaveChanges();
            }

            if (id != null)
            {
                Utilisateur user = db.Utilisateurs.Find(id.Value);
                var userVM = new UserVM(user);

                ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.Nom != "Admin").OrderByDescending(p => p.Nom).ToList(), "Id", "Nom", userVM.RoleId);

                return PartialView(userVM);
            }
            else
            {
                ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.Nom != "Admin").OrderByDescending(p => p.Nom).ToList(), "Id", "Nom");
                return PartialView();
            }
        }

        // POST : Save
        [HttpPost]
        public ActionResult Save(UserVM userVM, HttpPostedFileBase file)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleUtilisateur"]) return RedirectToAction("Home", "Index");

            if (string.IsNullOrWhiteSpace(userVM.Nom)) return Content("Le nom est requis");
            if (string.IsNullOrWhiteSpace(userVM.Prenoms)) return Content("Le prénom est requis");
            if (string.IsNullOrWhiteSpace(userVM.Email)) return Content("L'email est requis");
            if (!Utilities.IsValidEmail(userVM.Email)) return Content("Veuillez entrer un Email valide");
            
            if (userVM.Id != null && userVM.Id != new Guid())
            {
                var user = db.Utilisateurs.Find(userVM.Id);

                // Control existant
                var existe = db.Utilisateurs.Where(p => p.Email.ToUpper().Trim() == userVM.Email.ToUpper().Trim() && p.Id != user.Id).ToList();
                if (existe.Count != 0) return Content("Cet email existe déjà");

                // Modif. Utilisateur
                if ((file != null) && (file.ContentType.Contains("image")))
                {
                    string chemin_fichier = Server.MapPath(user.Photo);
                    System.IO.File.Delete(chemin_fichier);
                    file.SaveAs(chemin_fichier);
                    // upload
                    string cheminDossier = Server.MapPath("/Photo/");
                    string nomFichier = Guid.NewGuid().ToString();
                    string extention = "." + file.FileName.Split('.').Last();
                    string newPath = cheminDossier + nomFichier + extention;
                    userVM.Photo = "/Photo/" + nomFichier + extention;
                    file.SaveAs(newPath);
                }

                // Enr. Utilisateur
                user.Nom = userVM.Nom;
                user.Prenoms = userVM.Prenoms;
                user.Photo = userVM.Photo;
                user.Email = userVM.Email;
                user.Apropos = userVM.Apropos;
                user.Blog = userVM.Blog;
                user.RoleId = userVM.RoleId;
                user.UtilisateurId = (Guid)Session["UserId"];

                db.SaveChanges();
                return Content("OK");
            }
            else
            {
                // Control existant
                var existe = db.Utilisateurs.Where(t => t.Email.ToUpper().Trim() == userVM.Email.ToUpper().Trim()).ToList();
                if (existe.Count != 0)
                {
                    return Content("Cette article existe déjà");
                }

                if ((file != null) && (file.ContentType.Contains("image")))
                {
                    string cheminDossier = Server.MapPath("/Photo/");
                    string nomFichier = Guid.NewGuid().ToString();
                    string extention = "." + file.FileName.Split('.').Last();
                    string newPath = cheminDossier + nomFichier + extention;
                    userVM.Photo = "/Photo/" + nomFichier + extention;
                    file.SaveAs(newPath);
                }

                // Enr. Article
                var user = new Utilisateur
                {
                    Id = Guid.NewGuid(),
                    Nom = userVM.Nom,
                    Prenoms = userVM.Prenoms,
                    Photo = userVM.Photo,
                    Email = userVM.Email,
                    Apropos = userVM.Apropos,
                    Blog = userVM.Blog,
                    RoleId = userVM.RoleId,
                    UtilisateurId = (Guid)Session["UserId"]
                };
                db.Utilisateurs.Add(user);
                db.SaveChanges();

                return Content("OK");
            }
        }

        //Create Partial
        public ActionResult EditMyProfile(Guid id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            if (id != null)
            {
                var utilisateur = db.Utilisateurs.Find(id);

                return PartialView(new UserVM(utilisateur));
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveMyProfile(UserVM userVM)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");

            // Control champ vide

            if (string.IsNullOrEmpty(userVM.Nom))
            {
                ModelState.AddModelError("", "Veuillez saisir votre Nom");
                return View();
            }

            if (string.IsNullOrEmpty(userVM.Prenoms))
            {
                ModelState.AddModelError("", "Veuillez saisir vos Prénoms");
                return View();
            }

            if (userVM.Id != null)
            {

                if (db.Utilisateurs.Any(r => r.Nom == userVM.Nom && r.Id != userVM.Id))
                {
                    return Content("Cette utilisateur existe déjà");
                }

                var utilisateur = db.Utilisateurs.Find(userVM.Id);

                // Modif. Utilisateur
                utilisateur.Nom = userVM.Nom;
                utilisateur.Prenoms = userVM.Prenoms;
                utilisateur.Blog = userVM.Blog;
                utilisateur.Apropos = userVM.Apropos;
                db.SaveChanges();

                return Content("OK");
            }
            else
            {
                return Content("Une erreur s'est produite");
            }
        }

        // GET: Utilisateurs/Disable/5
        public ActionResult Disable(Guid? id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleUtilisateur"]) return RedirectToAction("Home", "Index");

            if (id == null)
            {
                return Content("NOTOK");
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return PartialView(utilisateur);
        }

        // POST: Utilisateurs/Disable/5
        [HttpPost/*, ActionName("Disable")*/]
        public ActionResult DisableConfirmed(Guid id)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Utilisateurs");
            if (!(bool)Session["RoleUtilisateur"]) return RedirectToAction("Home", "Index");
            
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            utilisateur.Actif = !utilisateur.Actif;
            db.SaveChanges();
            return Content("OK");
        }

        //


        // GET: Utilisateurs/ForgetPass
        public ActionResult ForgetPass()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("Deconnexion", "Utilisateurs");
            }
        }

        // POST: Utilisateurs/ForgetPass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPass(UserVM userVM)
        {
            try
            {
                if (string.IsNullOrEmpty(userVM.Email))
                {
                    ModelState.AddModelError("email", "Ce champ est requis");
                    return View(userVM);
                }

                // Controle Email
                var atpos = userVM.Email.IndexOf("@");
                var dotpos = userVM.Email.LastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= userVM.Email.Length)
                {
                    ModelState.AddModelError("email", "Entrer un Email valide");
                    return View(userVM);
                }

                // Control si l'Email existe
                try
                {
                    var rechUser = new UserVM(db.Utilisateurs.Where(p => p.Email.ToUpper().Trim() == userVM.Email.ToUpper().Trim()).First());
                    string x = Request.Url.Scheme + "://" + Request.Url.Authority;

                    // Envoie mail
                    MailMessage mailMsg = new MailMessage();

                    //Email de l'Admin
                    mailMsg.From = new MailAddress("dev@cultureweb.net", "EnCoRe");

                    //Code pour afficher une image dans le body du mail
                    LinkedResource logo = new LinkedResource(Server.MapPath("~//Content/logo_small.png"), "image/png");
                    logo.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    logo.ContentId = "CompanyLogo"; // formatage HTML fait dans la ligne suivante pour afficher mon logo
                    LinkedResource style = new LinkedResource(Server.MapPath("~//Content/mail.css"), "text/css");
                    style.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    style.ContentId = "CssStyle";

                    LinkedResource ico_facebook = new LinkedResource(Server.MapPath("~//Content/Ico_Mail/ico-facebook.png"), "image/png");
                    ico_facebook.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    ico_facebook.ContentId = "ico_facebook";
                    LinkedResource ico_twitter = new LinkedResource(Server.MapPath("~//Content/Ico_Mail/ico-google-plus.png"), "image/png");
                    ico_twitter.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    ico_twitter.ContentId = "ico_twitter";
                    LinkedResource ico_google_plus = new LinkedResource(Server.MapPath("~//Content/Ico_Mail/ico-linkedin.png"), "image/png");
                    ico_google_plus.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    ico_google_plus.ContentId = "ico_google_plus";
                    LinkedResource ico_linkedin = new LinkedResource(Server.MapPath("~//Content/Ico_Mail/ico-twitter.png"), "image/png");
                    ico_linkedin.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                    ico_linkedin.ContentId = "ico_linkedin";

                    mailMsg.Body = "<html>" +
                                     "<head>" +
                                        "<link href= \"cid:CssStyle\" rel=\"stylesheet\" type=\"text/css\" />" +
                                    "</head>" +
                                    "<body style=\"margin: 0; padding: 0; \" bgcolor=\"#eaeced\">" +
                                        "<table style = \"min-width:320px;\" width = \"100%\" cellspacing = \"0\" cellpadding = \"0\" bgcolor = \"#eaeced\">" +
                                            "<tr>" +
                                                "<td class=\"hide\">" +
                                                    "<table width = \"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:600px !important;\">" +
                                                        "<tr>" +
                                                            "<td style = \"min-width:600px; font-size:0; line-height:0;\" > &nbsp;</td>" +
                                                        "</tr>" +
                                                    "</table>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td class=\"wrapper\" style=\"padding:0 10px;\">" +
                                                    "<table data-module = \"module-1\" data-thumb = \"thumbnails/01.png\" width = \"100%\" cellpadding = \"0\" cellspacing = \"0\" >" +
                                                        "<tr>" +
                                                            "<td data-bgcolor = \"bg-module\" bgcolor = \"#eaeced\" >" +
                                                                "<table class=\"flexible\" align=\"center\" style=\"margin:0 auto;\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                    "<tr>" +
                                                                        "<td style = \"padding:29px 0 30px;\">" +
                                                                            "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                "<tr>" +
                                                                                    "<th class=\"flex\" width=\"113\" align=\"left\" style=\"padding:0;\">" +
                                                                                        "<table class=\"center\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                            "<tr>" +
                                                                                                "<td style = \"line-height:0;\">" +
                                                                                                "<a target = \"_blank\" style = \"text-decoration:none;\" href = \"https://www.cauripay.com/\"><img src = \"cid:CompanyLogo\" border = \"0\" style = \"font:bold 12px/12px Arial, Helvetica, sans-serif; color:#606060;\" align = \"center\" vspace = \"0\" hspace = \"0\" alt = \"EnCoRe\" /></a>" +
                                                                                                 "</td>" +
                                                                                             "</tr>" +
                                                                                         "</table>" +
                                                                                     "</th>" +
                                                                                "</tr>" +
                                                                            "</table>" +
                                                                        "</td>" +
                                                                    "</tr>" +
                                                                "</table>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                    "</table>" +
                                                    "<table data-module=\"module-2\" data-thumb=\"thumbnails/02.png\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                        "<tr>" +
                                                            "<td data-bgcolor=\"bg-module\" bgcolor=\"#eaeced\">" +
                                                                "<table class=\"flexible\" width=\"600\" align=\"center\" style=\"margin:0 auto;\" cellpadding=\"0\" cellspacing=\"0\">" +

                                                                    "<tr>" +
                                                                        "<td data-bgcolor = \"bg-block\" class=\"holder\" style=\"padding:58px 60px 52px;\" bgcolor=\"#f9f9f9\">";

                    mailMsg.Body += "<table width = \"100%\" cellpadding = \"0\" cellspacing = \"0\" >" +
                                        "<tr>" +
                                            "<td data-color = \"title\" data-size = \"size title\" data-min = \"25\" data-max = \"45\" data-link-color = \"link title color\" data-link-style = \"text-decoration:none; color:#292c34;\" class=\"title\" align=\"center\" style=\"font:35px/38px Arial, Helvetica, sans-serif; color:#292c34; padding:0 0 24px;\">" +
                                                "Bonjour " + rechUser.Prenoms +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td data-color= \"text\" data-size= \"size text\" data-min= \"10\" data-max= \"26\" data-link-color= \"link text color\" data-link-style= \"font-weight:bold; text-decoration:underline; color:#40aceb;\" align= \"center\" style= \"font:bold 16px/25px Arial, Helvetica, sans-serif; color:#888; padding:0 0 23px;\">" +
                                                "Vous avez demander une modification de votre mot de passe ? <br/>" +
                                                "Cliquez sur le bouton ci-dessous pour changer de mot de passe. <br/>" +
                                            //"Si vous n'avez pas initier cette action,  <br/>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style = \"padding:0 0 20px;\">" +
                                                "<table width= \"134\" align= \"center\" style= \"margin:0 auto;\" cellpadding= \"0\" cellspacing= \"0\" >" +
                                                    "<tr>" +
                                                        "<td data-bgcolor= \"bg-button\" data-size= \"size button\" data-min= \"10\" data-max= \"16\" class=\"btn\" align=\"center\" style=\"font:12px/14px Arial, Helvetica, sans-serif; color:#f8f9fb; text-transform:uppercase; mso-padding-alt:12px 10px 10px; border-radius:2px;\" bgcolor=\"#7bb84f\">" +
                                                            "<a target = \"_blank\" style=\"text-decoration:none; color:#f8f9fb; display:block; padding:12px 10px 10px;\" href=\"" + x + "/Utilisateurs/FormForgetPwd?id=" + rechUser.Id + "\">Changer mon mot de passe </a>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</td>" +
                            "</tr>" +
                            "<tr><td height = \"28\"></td></tr>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +
                "</table>";

                    mailMsg.Body += "<table data-module = \"module-7\" data-thumb = \"thumbnails/07.png\" width = \"100%\" cellpadding = \"0\" cellspacing = \"0\">" +
                                             "<tr>" +
                                                 "<td data-bgcolor = \"bg-module\" bgcolor = \"#eaeced\">" +
                                                    "<table class=\"flexible\" width=\"600\" align=\"center\" style=\"margin:0 auto;\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                         "<tr>" +
                                                            "<td class=\"footer\" style=\"padding:0 0 10px;\">" +
                                                                "<table width = \"100%\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                    "<tr class=\"table-holder\">" +
                                                                        "<th class=\"tfoot\" width=\"400\" align=\"left\" style=\"vertical-align:top; padding:0;\">" +
                                                                            "<table width = \"100%\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                "<tr>" +
                                                                                    "<td data-color=\"text\" data-link-color=\"link text color\" data-link-style=\"text-decoration:underline; color:#797c82;\" class=\"aligncenter\" style=\"font:12px/16px Arial, Helvetica, sans-serif; color:#797c82; padding:0 0 10px;\">" +
                                                                                        "EnCoRe, " + TimeZoneInfo.ConvertTime(DateTime.Now, benin_time).Year + ". &nbsp; All Rights Reserved." +
                                                                                    "</td>" +
                                                                                "</tr>" +
                                                                            "</table>" +
                                                                        "</th>" +
                                                                        "<th class=\"thead\" width=\"200\" align=\"left\" style=\"vertical-align:top; padding: 0; \">" +
                                                                            "<table class=\"center\" align =\"right\" cellpadding =\"0\" cellspacing =\"0\" >" +
                                                                                "<tr>" +
                                                                                    "<td class=\"btn\" valign =\"top\" style =\"line-height:0; padding: 3px 0 0; \"> " +
                                                                                        "<a target = \"_blank\" style = \"text-decoration:none;\" href = \"#\" ><img src = \"cid:ico_facebook\" border = \"0\" style = \"font:12px/15px Arial, Helvetica, sans-serif; color:#797c82;\" align = \"left\" vspace = \"0\" hspace = \"0\" width = \"6\" height = \"13\" alt = \"fb\" /></a>" +
                                                                                    "</td>" +
                                                                                    "<td width = \"20\" ></td>" +
                                                                                    "<td class=\"btn\" valign=\"top\" style=\"line-height:0; padding: 3px 0 0; \">" +
                                                                                        "<a target = \"_blank\" style=\"text-decoration:none;\" href =\"#\" ><img src = \"cid:ico_twitter\" border=\"0\" style =\"font:12px/15px Arial, Helvetica, sans-serif; color:#797c82;\" align =\"left\" vspace =\"0\" hspace =\"0\" width =\"13\" height =\"11\" alt =\"tw\" /></a>" +
                                                                                    "</td>" +
                                                                                    "<td width = \"19\" ></td>" +
                                                                                    "<td class=\"btn\" valign=\"top\" style=\"line-height:0; padding:3px 0 0;\">" +
                                                                                        "<a target = \"_blank\" style=\"text-decoration:none;\" href =\"#\" ><img src = \"cid:ico_google_plus\" border =\"0\" style=\"font:12px/15px Arial, Helvetica, sans-serif; color:#797c82;\" align =\"left\" vspace =\"0\" hspace =\"0\" width =\"19\" height =\"15\" alt =\"g +\" /></a>" +
                                                                                    "</td>" +
                                                                                    "<td width = \"20\"></td>" +
                                                                                    "<td class=\"btn\" valign=\"top\" style=\"line-height:0; padding:3px 0 0;\">" +
                                                                                        "<a target = \"_blank\" style =\"text-decoration:none;\" href =\"#\" ><img src = \"cid:ico_linkedin\" border=\"0\" style =\"font:12px/15px Arial, Helvetica, sans-serif; color:#797c82;\" align =\"left\" vspace =\"0\" hspace =\"0\" width =\"13\" height =\"11\" alt =\"in\" /></a>" +
                                                                                    "</td>" +
                                                                                "</tr>" +
                                                                            "</table>" +
                                                                        "</th>" +
                                                                    "</tr>" +
                                                                "</table>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                    "</table>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style = \"line-height:0;\"><div style = \"display:none; white-space:nowrap; font:15px/1px courier;\" > &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</div></td>" +
                                "</tr>" +
                            "</table>" +
                        "</body>" +
                    "</html>";

                    AlternateView av1 = AlternateView.CreateAlternateViewFromString(mailMsg.Body, null, MediaTypeNames.Text.Html);
                    av1.LinkedResources.Add(logo);
                    av1.LinkedResources.Add(style);
                    av1.LinkedResources.Add(ico_facebook);
                    av1.LinkedResources.Add(ico_twitter);
                    av1.LinkedResources.Add(ico_google_plus);
                    av1.LinkedResources.Add(ico_linkedin);
                    mailMsg.AlternateViews.Add(av1);

                    mailMsg.IsBodyHtml = true;//Pour utiliser des balises html
                    mailMsg.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

                    mailMsg.To.Add(rechUser.Email);

                    mailMsg.Subject = "EnCoRe : Réinitialisez votre mot de passe";

                    // Smtp configuration
                    SmtpClient smtp = new SmtpClient("mail.cultureweb.net");
                    smtp.Credentials = new System.Net.NetworkCredential("dev@cultureweb.net", "Culture1@web.2017");

                    try
                    {
                        smtp.Send(mailMsg);
                    }
                    catch (Exception)
                    {

                    }

                    ViewBag.OK = "Un lien de modification de votre mot de passe à été envoyé à votre adresse mail.";
                    return View(rechUser);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Cet Email n'existe pas dans nos données. Entrer l'email utilisé pour créer votre compte.";
                    return View(userVM);
                }
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("Deconnexion", "Utilisateurs");
            }
        }

        // GET: Utilisateurs/FormForgetPwd
        public ActionResult FormForgetPwd(Guid id)
        {
            try
            {
                try
                {
                    Utilisateur user = db.Utilisateurs.Where(p => p.Id == id).First();
                    UserVM userVM = new UserVM() { Id = user.Id , Email = user.Email };
                    return View(userVM);
                }
                catch (Exception)
                {
                    return RedirectToAction("Connexion");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Deconnexion", "Utilisateurs");
            }
        }

        // POST: Utilisateurs/FormForgetPwd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormForgetPwd(string email, string pwd, string pwdCf)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Ce champ est requis");
                    return View();
                }
                if (String.IsNullOrWhiteSpace(pwd))
                {
                    ModelState.AddModelError("", "Ce champ est requis");
                    return View();
                }

                if (pwd.Length < 6)
                {
                    ModelState.AddModelError("PWD", "Le mot de passe doit contenir un minimum de 8 caractères");
                    return View();
                }
                if (!pwd.Any(char.IsUpper))
                {
                    ModelState.AddModelError("PWD", "Veuillez entrer au moins une lettre majuscule");
                    return View();
                }
                if (!pwd.Any(char.IsNumber))
                {
                    ModelState.AddModelError("password", "Veuillez entrer au moins un chiffre");
                    return View();
                }
                if (!pwd.Any(char.IsSymbol) && !pwd.Any(char.IsPunctuation) && !pwd.Any(char.IsSeparator))
                {
                    ModelState.AddModelError("password", "Veuillez entrer au moins un caractère spécial");
                    return View();
                }

                if (pwd != pwdCf)
                {
                    ViewBag.Error = "Les deux mots de passe ne concordent pas";
                    return View();
                }

                // Enr nouveau mot de passe
                var new_user = db.Utilisateurs.Where(p => p.Email == email).First();
                string passhashed = SimpleHash.ComputeHash(pwd, "SHA512", null);
                new_user.PWD = passhashed;
                db.SaveChanges();

                return RedirectToAction("Confirm_Edit_Pass", new { id = new_user.Id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Deconnexion", "Utilisateurs");
            }
        }

        // GET: users/Confirm_Edit_Pass
        public ActionResult Confirm_Edit_Pass(Guid id)
        {
            try
            {
                var user = db.Utilisateurs.Find(id);

                return View(new UserVM(user));
            }
            catch (Exception)
            {
                return RedirectToAction("Deconnexion", "Users");
            }
        }

        // GET: Utilisateurs/ConfirmEditPass
        public ActionResult ConfirmEditPass(Guid id)
        {
            try
            {
                var users = db.Utilisateurs.Find(id);
                return View(users);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Deconnexion", "Utilisateurs");
            }
        }
    }
}
