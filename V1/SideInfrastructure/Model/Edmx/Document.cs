//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SideInfrastructure.Model.Edmx
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public long DocumentId { get; set; }
        public string DocumentName { get; set; }
        public long DocumentTypeId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public byte[] DocumentData { get; set; }
        public string DocumentExtension { get; set; }
    
        public virtual User User { get; set; }
    }
}
