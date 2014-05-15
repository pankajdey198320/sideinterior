using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideAdmin.Utility;

namespace SideInfrastructure.Model.ViewModel
{
    public class PortfolioViewModel
    {
        public string ImgSrc { get; set; }
        public string Description { get; set; }
        public IsotopDataCatagoryType DataCatagory { get; set; }
        public string PageLink { get; set; }
        public IsotopeSigeType ImageSizeType { get; set; }
    }
}
