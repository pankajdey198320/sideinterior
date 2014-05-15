using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideInfrastructure.Model.Edmx;

namespace SideInfrastructure
{
    public class ModelService
    {
        public void SaveSection(Section data,Document docs, int[] attributes) {
            using (SIDEContxts context = new SIDEContxts()) {
                context.Sections.Add(data); context.Documents.Add(docs);
                context.SaveChanges();
                SectionDocument da = new SectionDocument()
                {
                    SectionId = data.SectionId,
                    DocumentId = docs.DocumentId
                };
                context.SectionDocuments.Add(da);
                foreach (int attr in attributes) {
                    context.SectionAttributes.Add(new SectionAttribute() { 
                     AttributeId = attr,
                     SectionId= data.SectionId
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
