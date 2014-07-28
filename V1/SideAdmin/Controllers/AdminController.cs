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
    [CustomAuthentication]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private string DocServerUrl
        {
            get
            {
                return ConfigurationUtility.GetDocServerUrl();
            }
        }

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
                Id = id,
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
                        if (con != null)
                        {
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

        public ActionResult ProjectList()
        {
            using (SIDEContxts context = new SIDEContxts())
            {

                var projs = (from v in context.Sections
                             where v.SectionTypeId == (long)SectionTypes.Project
                             select new ProjectViewModel()
                             {
                                 Id = (int)v.SectionId,
                                 Description = v.SectionDescription,
                                 Title = v.SectionName,
                                 CoverImage = (from k in context.SectionDocuments
                                               where k.SectionId == v.SectionId
                                               select
                                                    this.DocServerUrl + k.DocumentId
                                              ).FirstOrDefault(),
                                 Status = v.Status > 0

                             }).ToList();
                projs.Add(new ProjectViewModel()
                {
                    Id = -1,
                    Title = "New Project"
                });
                ResetSessionContainer();
                return View("ProjectList", projs);
            }

        }

        public ActionResult Project(int id)
        {
            ProjectViewModel model = new ProjectViewModel();
            using (SIDEContxts context = new SIDEContxts())
            {

                model = (from v in context.Sections
                         where v.SectionId == id
                         select new ProjectViewModel()
                         {
                             Id = (int)v.SectionId,
                             Title = v.SectionName,
                             Description = v.SectionDescription,
                             Status = v.Status > 0,
                             ImageList = (from k in context.SectionDocuments
                                          where k.SectionId == id
                                          select new CarouselViewModel()
                                          {
                                              Id = k.DocumentId,
                                              ImageSrc = this.DocServerUrl + k.DocumentId
                                          }).ToList(),
                             CheckLists = (from m in context.SectionAttributes
                                           join k in context.SectionAttributeValues
                                               on m.SectionAttributeID equals k.SectionAttributeId
                                           where m.SectionTypeId == (long)SectionTypes.Project && m.AttributeId == (long)SectionAttributeTypes.ProjectCheckList
                                           && k.SectionId == v.SectionId
                                           select k.AttributeValue
                                                ).ToList()
                         }).FirstOrDefault();
            }
            if (model == null)
                model = new ProjectViewModel();
            var docs = GetSessionContainer();
            foreach (var items in docs)
            {
                model.ImageList.Add(new Models.ViewModel.CarouselViewModel()
                {
                    Id = items.DocumentId,
                    ImageSrc = ConfigurationUtility.GetDomainUrl() + "/Admin/Thumnail/" + items.DocumentId
                });
            }

            if (model.ImageList.Any())
                model.ImageList.FirstOrDefault().IsActive = true;

            return PartialView("ProjectUpload", model);
        }

        [HttpPost]
        public ActionResult UploadProject(int id)
        {
            var docs = GetSessionContainer();
            var args = Request.Files[0];
            for (int i = 0; i < Request.Files.Count; i++)
            {
                Document vm = new Document();
                vm.DocumentId = -docs.Count();
                vm.DocumentExtension = Request.Files[i].FileName.Substring(Request.Files[i].FileName.LastIndexOf(".") + 1);
                vm.DocumentTypeId = 1;
                vm.CreatedBy = 1;
                vm.DocumentName = Request.Files[i].FileName;
                using (Stream sr = Request.Files[i].InputStream)
                {
                    byte[] data = new byte[Request.Files[i].ContentLength];
                    sr.Read(data, 0, data.Length);
                    vm.DocumentData = data;
                }
                docs.Add(vm);
            }
            return Json(new { success = true, fileName = args == null ? "NA" : args.FileName, urls = docs.Select(p => p.DocumentId.ToString()).ToList() });
        }

        [HttpPost]
        public ActionResult SaveProject(ProjectViewModel model)
        {
            var docs = GetSessionContainer();
            using (SIDEContxts ctx = new SIDEContxts())
            {
                var section = new Section();
                if (model.Id <= 0)
                {
                    section = new Section()
                    {
                        CreationDate = DateTime.Now,
                        SectionName = model.Title,
                        SectionDescription = model.Description,
                        SectionTypeId = (long)SectionTypes.Project,
                        Status = model.Status ? 1 : 0
                    };
                    ctx.Sections.Add(section);
                }
                else
                {
                    section = (from v in ctx.Sections where v.SectionId == model.Id select v).FirstOrDefault();
                    section.SectionName = model.Title;
                    section.SectionDescription = model.Description;
                    section.Status = model.Status ? 1 : 0;
                }

                var lst = new List<Document>();
                foreach (var d in docs)
                {
                    lst.Add(new Document()
                    {
                        DocumentData = d.DocumentData,
                        DocumentTypeId = d.DocumentTypeId,
                        DocumentExtension = d.DocumentExtension,
                        DocumentName = d.DocumentName,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1
                    });
                }
                ctx.Documents.AddRange(lst);
                ctx.SaveChanges();
                var existings = (from m in ctx.SectionAttributeValues
                                 join k in ctx.SectionAttributes
                                     on m.SectionAttributeId equals k.SectionAttributeID
                                 where k.SectionTypeId == (long)SectionTypes.Project
                                 && k.AttributeId == (long)SectionAttributeTypes.ProjectCheckList
                                 && m.SectionId == section.SectionId
                                 select m.AttributeValue).ToList();
                var chkattr = model.CheckLists.Where(v => !existings.Contains(v)).ToList();
                foreach (var str in chkattr)
                {
                    ctx.SectionAttributeValues.Add(new SectionAttributeValue()
                    {
                        SectionId = section.SectionId,
                        AttributeValue = str,
                        SectionAttributeId = (ctx.SectionAttributes.Where(v => v.AttributeId == (long)SectionAttributeTypes.ProjectCheckList && v.SectionTypeId == (long)SectionTypes.Project)).Select(k => k.SectionAttributeID).FirstOrDefault()
                    });
                }
                foreach (var d in lst)
                {
                    ctx.SectionDocuments.Add(new SectionDocument()
                    {
                        DocumentId = d.DocumentId,
                        SectionId = section.SectionId,
                    });
                }
                ctx.SaveChanges();
            }
            ResetSessionContainer();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteProjectImage(long id)
        {
            using (SIDEContxts ctx = new SIDEContxts())
            {
                if (id > 0)
                {
                    ctx.Database.ExecuteSqlCommand("delete from SectionDocument where documentid = " + id + ";delete from document where documentid = " + id);

                }
                else
                {
                    var docs = GetSessionContainer();
                    docs.Remove(docs.First(v => v.DocumentId == id));
                }
            }
            return Json(true);
        }
        public ActionResult Thumnail(long id, int height = 150, int width = 150)
        {
            var docs = GetSessionContainer();
            var doc = docs.FirstOrDefault(p => p.DocumentId == id);

            var img = new WebImage(doc.DocumentData).Resize(width, height, true, false);
            return File(img.GetBytes(), "image/png");


        }
        private List<Document> GetSessionContainer()
        {

            if (Session["docProj"] != null)
            {
                return (List<Document>)Session["docProj"];
            }
            else
            {
                List<Document> doc = new List<Document>();
                Session["docProj"] = doc;
                return doc;
            }
        }
        private void ResetSessionContainer()
        {
            Session["docProj"] = null;
        }
    }
}
