using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SideAdmin.Utility;
using SideInfrastructure.Model.Edmx;
using SideInfrastructure.Model.ViewModel;
using V1.Models.ViewModel;

namespace V1.Controllers
{
    public class ProjectController : Controller
    {
        private string DocServerUrl
        {
            get
            {
                return ConfigurationUtility.GetDocServerUrl();
            }
        }

        private string ThumbnailUrl
        {
            get
            {
                return ConfigurationUtility.GetImageThumbnailUrl();
            }
        }
        //
        // GET: /Project/
        public ActionResult Index()
        {

            using (SIDEContxts context = new SIDEContxts())
            {

                var projs = (from v in context.Sections
                             where v.SectionTypeId == (long)SectionTypes.Project &&
                             v.Status == (int)ComponentStatus.Active
                             select new ProjectViewModel()
                             {
                                 Id = (int)v.SectionId,
                                 Description = v.SectionDescription,
                                 Title = v.SectionName,
                                 CoverImage = (from k in context.SectionDocuments
                                               where k.SectionId == v.SectionId
                                               select
                                                    ThumbnailUrl + k.DocumentId
                                              ).FirstOrDefault()
                             }).ToList();
                return View(projs);
            }

        }

        public ActionResult MostPopularProjects()
        {

            using (SIDEContxts context = new SIDEContxts())
            {

                var projs = (from v in context.Sections
                             where v.SectionTypeId == (long)SectionTypes.Project &&
                             v.Status == (int)ComponentStatus.Active
                             orderby v.CreationDate descending
                             select new ProjectViewModel()
                             {
                                 Id = (int)v.SectionId,
                                 Description = v.SectionDescription,
                                 Title = v.SectionName,
                                 CoverImage = (from k in context.SectionDocuments
                                               where k.SectionId == v.SectionId
                                               select
                                                    ThumbnailUrl + k.DocumentId
                                              ).FirstOrDefault(),
                                 Date = v.CreationDate
                             }).Take(4).ToList();
                return View("_PopularItems", projs);
            }

        }
        public ActionResult Details(long id)
        {

            ProjectViewModel model = new ProjectViewModel();
            using (SIDEContxts context = new SIDEContxts())
            {
                int pre, post;
                var projs = (from v in context.Sections
                             where v.SectionTypeId == (long)SectionTypes.Project &&
                             v.Status == (int)ComponentStatus.Active
                             select v.SectionId
                             ).ToList();
                var index = projs.IndexOf(id);
                if (index > 0)
                {
                    pre = (int)projs[index - 1];
                }
                else
                {
                    pre = post = (int)projs[projs.Count-1];
                }
                if (index + 1 < projs.Count)
                {
                    post = (int)projs[index + 1];
                }
                else
                {
                    post = (int)projs[0]; ;
                }

                model = (from v in context.Sections
                         where v.SectionId == id &&
                             v.Status == (int)ComponentStatus.Active
                         select new ProjectViewModel()
                         {
                             Id = (int)v.SectionId,
                             Title = v.SectionName,
                             Description = v.SectionDescription,
                             ImageList = (from k in context.SectionDocuments
                                          where k.SectionId == id
                                          select new CarouselViewModel()
                                          {
                                              ImageSrc = DocServerUrl + k.DocumentId + "?height=300&width=263"
                                          }).ToList(),
                             CheckLists = (from m in context.SectionAttributes
                                           join k in context.SectionAttributeValues
                                               on m.SectionAttributeID equals k.SectionAttributeId
                                           where m.SectionTypeId == (long)SectionTypes.Project && m.AttributeId == (long)SectionAttributeTypes.ProjectCheckList
                                           && k.SectionId == v.SectionId
                                           select k.AttributeValue
                                                ).ToList(),
                             Pre = pre,
                             Next = post

                         }).FirstOrDefault();
            }
            if (model == null)
                model = new ProjectViewModel();

            if (model.ImageList.Any())
                model.ImageList.FirstOrDefault().IsActive = true;
            model.RelatedProjects.Add(new ProjectViewModel()
            {
                Title = "Related 1",
                CoverImage = "http://localhost/V1/Document/Thumnail/29?height=300&width=263",
                Id = 29
            });
            return View(model);
        }

    }
}