//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnCoReWebPage.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            this.Articles = new HashSet<Article>();
        }
    
        public System.Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenoms { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PWD { get; set; }
        public string Apropos { get; set; }
        public string Blog { get; set; }
        public bool Actif { get; set; }
        public System.Guid RoleId { get; set; }
        public Nullable<System.Guid> UtilisateurId { get; set; }
    
        public virtual ICollection<Article> Articles { get; set; }
        public virtual Role Role { get; set; }
    }
}
