using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SideInfrastructure.Model.Edmx;

namespace V1.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View("UploadImage");
        }



        [HttpPost]
        public ActionResult UploadImage()
        {
            var args = Request.Files[0];
            SaveFile(args);
            return View();
        }

        private void SaveFile(HttpPostedFileBase args)
        {
            byte[] data = new byte[args.ContentLength];
            long fileType = 1;
            string extensioin = args.FileName.Substring(args.FileName.LastIndexOf(".") + 1);
            //set the file type based on File Extension
            switch (extensioin)
            {
                case "jpg":
                    fileType = 1;// "image/jpg";
                    break;
                case "png":
                    fileType = 2;//"image/png";
                    break;
                case "gif":
                    fileType = 3;// "image/gif";
                    break;
                default:
                    fileType = 4;// "OTHERS";
                    break;
            }

            using (Stream stream = args.InputStream)
            {
                stream.Read(data, 0, data.Length);
                using (SIDEContxts context = new SIDEContxts())
                {
                    var section = new Section()
                    {
                        CreationDate = DateTime.Now,
                        SectionName = args.FileName,
                        SectionTypeId = 1
                    };
                    context.Sections.Add(section);
                    var doc = new Document()
                    {
                        CreatedBy = 1,
                        DocumentData = data,
                        DocumentTypeId = fileType,
                        DocumentExtension = extensioin,
                        DocumentName = args.FileName,
                        CreatedDate = DateTime.Now
                    };
                    context.Documents.Add(doc);

                    context.SaveChanges();
                    context.SectionDocuments.Add(new SectionDocument()
                    {
                        SectionId = section.SectionId,
                        DocumentId = doc.DocumentId
                    });
                    context.SaveChanges();
                }
            }
        }

    }
}
