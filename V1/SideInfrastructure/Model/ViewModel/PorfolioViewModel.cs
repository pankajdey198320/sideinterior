using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideInfrastructure.Model.ViewModel
{
    public class PorfolioViewModel
    {
        public string ImgSrc { get; set; }
        public string Description { get; set; }
        public string DataCatagory { get; set; }
        public string PageLink { get; set; }
        public string ImageSizeType { get; set; }
    }

    public static class ImageSize {
        public const string Small = "item-small";
        public const string Small = "item-small";
        public const string Small = "item-small";
        public const string Small = "item-small";
        public const string Small = "item-small";
    }
}
