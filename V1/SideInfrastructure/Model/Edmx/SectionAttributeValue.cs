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
    
    public partial class SectionAttributeValue
    {
        public long SectionAttributeValueId { get; set; }
        public long SectionAttributeId { get; set; }
        public string AttributeValue { get; set; }
        public Nullable<long> SectionId { get; set; }
    
        public virtual SectionAttribute SectionAttribute { get; set; }
        public virtual Section Section { get; set; }
    }
}
