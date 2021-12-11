using EnCoReWebPage.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnCoReWebPage.App_Classes
{
    public class UserVM
    {
        public UserVM()
        {
        }

        public System.Guid Id { get; set; }

        [Required (ErrorMessage = "Ce champ est requis")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        public string Prenoms { get; set; }

        public string Photo { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Entrer un Email valide")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Ce champ est requis")]
        [DataType(DataType.Password)]
        [DisplayName("Mot de passe")]
        //[RegularExpression(@"(?=^.{8,}$)(?=(?:.*?\d){1})(?=.*[a-z])(?=(?:.*?[A-Z]){1})(?=(?:.*?[!@#$%*()_+-/=é'èçà~|`\¤£¨µ/§^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+-/=é'èçà~|`\¤£¨µ/§^&}{:;?.]*$", ErrorMessage = "Les mots de passe doivent comporter au moins 8 caractères et contenir au moins une lettre majuscule, une lettre minuscule, un chiffre et caractère spécial")]
        [RegularExpression(@"(?=^.{1,}$)(?=(?:.*?\d){1})(?=.*[a-z])(?=(?:.*?[A-Z]){1})(?=(?:.*?[!@#$%*()_+-/=é'èçà~|`\¤£¨µ/§^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+-/=é'èçà~|`\¤£¨µ/§^&}{:;?.]*$", ErrorMessage = "Les mots de passe doivent comporter au moins 8 caractères et contenir au moins une lettre majuscule, une lettre minuscule, un chiffre et caractère spécial")]
        public string PWD { get; set; }

        [Required(ErrorMessage = "Ce champ est requis")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmez le mot de passe")]
        [Compare("PWD", ErrorMessage = "Les mots de passe ne concordent pas")]
        public string PWDcf { get; set; }
        public string Apropos { get; set; }
        public string Blog { get; set; }
        public bool Actif { get; set; }
        public System.Guid RoleId { get; set; }
        public Nullable<System.Guid> UtilisateurId { get; set; }

        public UserVM(Utilisateur utilisateur)
        {
            Id = utilisateur.Id;
            Nom = utilisateur.Nom;
            Prenoms = utilisateur.Prenoms;
            Photo = utilisateur.Photo;
            Email = utilisateur.Email;
            PWD = utilisateur.PWD;
            Apropos = utilisateur.Apropos;
            Blog = utilisateur.Blog;
            Actif = utilisateur.Actif;
            RoleId = utilisateur.RoleId;
            UtilisateurId = utilisateur.UtilisateurId;
        }
    }
}