using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideInfrastructure.Model.ViewModel
{
    public class NavigationViewModel
    {
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public string ClientSideFunction { get; set; }
        public IEnumerable<NavigationViewModel> Childs { get; set; }
        public bool IsActive { get; set; }
        public bool isSubMenu { get; set; }
        public string DataOptionValue { get; set; }
    }
}
