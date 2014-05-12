using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using V1.Models.ViewModel;

namespace V1.Controllers
{
    using SideAdmin.Utility;
    using SideInfrastructure.Model.Edmx;
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            IEnumerable<CarouselViewModel> model = new List<CarouselViewModel>();

            using (SIDEContxts contex = new SIDEContxts())
            {
                var url = ConfigurationUtility.GetDocServerUrl();
                model = (from v in contex.Sections
                         join map in contex.SectionDocuments
                         on v.SectionId equals map.SectionId
                         // join doc in contex.Documents on map.DocumentId equals doc.DocumentId
                         where v.SectionTypeId == 1
                         select new CarouselViewModel()
                         {
                             CarouselCaption = new CarouselViewModel.Caption()
                             {
                                 Text = v.SectionDescription
                             },
                             ImageSrc = url + map.DocumentId

                         }).ToList();
            }
            if (model.Any())
            {
                model.First().IsActive = true;
            }

            return View(model);
        }

        public ActionResult GetNavs(string controller = "Home", string action = "Index", bool isSubMenu = false)
        {
            if (!isSubMenu)
            {
                return  PartialView("_navItems", GetMenu(0)); 
            }
            switch (controller)
            {
               
                case "About":
                    {
                        return PartialView("_navItems", GetMenu(1));
                    }
                case "Project":
                    {
                        return PartialView("_navItems", GetMenu(2));
                    }
                case "portfolio":
                    {
                        return PartialView("_navItems", GetMenu(3));
                    }
                default:
                    { return PartialView("_navItems", GetMenu(0)); }
            }
        }

        private IEnumerable<SideInfrastructure.Model.ViewModel.NavigationViewModel> GetMenu(int parenMenuId)
        {
            var xNavs = new List<SideInfrastructure.Model.ViewModel.NavigationViewModel>();
            if (parenMenuId <= 0)
            {
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Home",
                    Url = Url.Action("Index", "Home"),
                    IsActive = true
                });

                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "About",
                    Url = Url.Action("Index", "About"),
                    IsActive = false
                });
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Projects",
                    Url = Url.Action("Index", "Project"),
                    IsActive = false
                });
            }
            else if(parenMenuId == 1)
            {
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "About Us",
                    Url = "#story",
                    IsActive = true,
                    isSubMenu = true
                });

                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Team SIDE",
                    Url = "#team",
                    IsActive = false,
                    isSubMenu = true
                });
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Vision",
                    Url = "#vision",
                    IsActive = false,
                    isSubMenu = true
                });
            }
            else if (parenMenuId == 2)
            {
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Some Kitchen",
                    Url = "#story",
                    IsActive = true,
                    isSubMenu = true
                });

                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Leaving room",
                    Url = "#team",
                    IsActive = false,
                    isSubMenu = true
                });
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Outer world",
                    Url = "#vision",
                    IsActive = false,
                    isSubMenu = true
                });
            }
            else if (parenMenuId == 3)
            {
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "All",
                    Url = "#filter",
                    IsActive = true,
                    isSubMenu = true,
                    DataOptionValue = "*"
                });

                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Residential",
                    Url = "#filter",
                    IsActive = false,
                    isSubMenu = true,
                    DataOptionValue = ".Residential"
                });
                xNavs.Add(new SideInfrastructure.Model.ViewModel.NavigationViewModel()
                {
                    DisplayName = "Retail",
                    Url = "#filter",
                    IsActive = false,
                    isSubMenu = true,
                    DataOptionValue = ".Retail"
                });
            }
            return xNavs;
        }
    }
}
