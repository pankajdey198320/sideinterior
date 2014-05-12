using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SideAdmin.Filters
{
    public class MyCustomPrincipal : GenericPrincipal
    {
        private string _hairColor;

        public MyCustomPrincipal(IIdentity identity, string[] roles, string hairColor)
            : base(identity, roles)
        {
            _hairColor = hairColor;
        }
        public bool IsSessionAlive { get;set ;}
        public string HairColor { get { return _hairColor; } set { _hairColor = value; } }
    }
}