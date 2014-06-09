using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V1.Models.ViewModel;

namespace SideInfrastructure.Model.ViewModel
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            this.ImageList = new List<CarouselViewModel>();
            this.CheckLists = new List<string>();
            this.RelatedProjects = new List<ProjectViewModel>();
        }

        public int Id { get; set; }

        public string CoverImage { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<CarouselViewModel> ImageList { get; set; }

        public List<string> CheckLists { get; set; }

        public List<ProjectViewModel> RelatedProjects { get; set; }
    }
}
