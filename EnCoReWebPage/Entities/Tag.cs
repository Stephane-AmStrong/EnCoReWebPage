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
    
    public partial class Tag
    {
        public Tag()
        {
            this.ArticleTags = new HashSet<ArticleTag>();
        }
    
        public System.Guid Id { get; set; }
        public string Libelle { get; set; }
    
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
