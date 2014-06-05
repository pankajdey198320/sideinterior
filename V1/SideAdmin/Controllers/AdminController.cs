using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SideAdmin.Filters;
using SideAdmin.Utility;
using SideInfrastructure;
using SideInfrastructure.Model.Edmx;
using SideInfrastructure.Model.ViewModel;
using V1.Models.ViewModel;

namespace V1.Controllers
{
    //[CustomAuthentication]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View("UploadImage");
        }

        public ActionResult ViewCarouselImageList()
        {
            IEnumerable<CarouselViewModel> model = new List<CarouselViewModel>();

            using (SIDEContxts contex = new SIDEContxts())
            {
                var url = ConfigurationUtility.GetDocServerUrl();
                model = (from v in contex.Sections
                         join map in contex.SectionDocuments
                         on v.SectionId equals map.SectionId
                         // join doc in contex.Documents on map.DocumentId equals doc.DocumentId
                         where v.SectionTypeId == (long)SectionTypes.HomePageCaraosel
                         orderby map.SectionId descending
                         select new CarouselViewModel()
                         {
                             Id = map.SectionId,
                             CarouselCaption = new CarouselViewModel.Caption()
                             {
                                 Text = v.SectionDescription
                             },
                             ImageSrc = url + map.DocumentId

                         }).ToList();
            }
            return PartialView("_ImageList", model);
        }

        [HttpPost]
        public ActionResult UploadImage(CarouselViewModel model)
        {


            var args = Request.Files[0];
            SaveFile(args, model);
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage1(string caption)
        {
            var args = Request.Files[0];
            var st = Request["txtCaption"];
            long id = -1;
            long.TryParse(Convert.ToString(Request["hdnImgId"]), out id);
            var model = new CarouselViewModel()
            { 
                Id=id,
                CarouselCaption = new CarouselViewModel.Caption()
                {
                    Text = st
                }
            };
            SaveFile(args, model);
            return View("UploadImage");
        }


        public ActionResult DeleteImage(long id)
        {
            using (SIDEContxts context = new SIDEContxts())
            {
                var item = context.Sections.FirstOrDefault(p => p.SectionId == id);
                var sectionDocumentMap = context.SectionDocuments.FirstOrDefault(p => p.SectionId == id);
                if (item != null)
                {
                    context.Sections.Remove(item);
                    if (sectionDocumentMap != null)
                    {
                        context.SectionDocuments.Remove(sectionDocumentMap);
                        var doc = context.Documents.FirstOrDefault(p => p.DocumentId == sectionDocumentMap.DocumentId);
                        if (doc != null)
                        {
                            context.Documents.Remove(doc);
                        }
                    }
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ViewCarouselImageList");
        }

        private void SaveFile(HttpPostedFileBase args, CarouselViewModel model)
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
                var img = new WebImage(data).Resize(1903, 750, false, false);
                using (SIDEContxts context = new SIDEContxts())
                {
                    if (model.Id > 0)
                    {
                        var doc = (from v in context.Documents
                                   join map in context.SectionDocuments on v.DocumentId equals map.DocumentId
                                   where map.SectionId == model.Id
                                   select v).FirstOrDefault();
                        if (doc != null)
                        {
                            doc.DocumentData = img.GetBytes();
                        }

                        var con = (from v in context.Sections where v.SectionId == model.Id select v).FirstOrDefault();
                        if (con != null) {
                            con.SectionDescription = model.CarouselCaption.Text;
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        var section = new Section()
                        {
                            CreationDate = DateTime.Now,
                            SectionName = args.FileName,
                            SectionDescription = model.CarouselCaption.Text,
                            SectionTypeId = (long)SectionTypes.HomePageCaraosel
                        };
                        context.Sections.Add(section);
                        var doc = new Document()
                        {
                            CreatedBy = 1,
                            DocumentData = img.GetBytes(),
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

        [HttpPost]
        public void SaveProjectPortfolio(PortfolioViewModel model)
        {
            var args = Request.Files[0];
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
                var img = new WebImage(data);
                {
                    var section = new Section()
                    {
                        CreationDate = DateTime.Now,
                        SectionName = model.Description,
                        SectionDescription = model.Description,
                        SectionTypeId = 1
                    };
                    var doc = new Document()
                    {
                        CreatedBy = 1,
                        DocumentData = img.GetBytes(),
                        DocumentTypeId = fileType,
                        DocumentExtension = extensioin,
                        DocumentName = args.FileName,
                        CreatedDate = DateTime.Now
                    };
                    ModelService svc = new ModelService();
                    svc.SaveSection(section, doc, new int[] { (int)model.ImageSizeType, (int)model.DataCatagory });
                }
            }
        }
    }
}
