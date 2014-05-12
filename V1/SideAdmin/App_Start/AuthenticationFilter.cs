using System.Web.Mvc;
using System.Web.Mvc.Filters;
using SideAdmin.Utility;

namespace SideAdmin.Filters
{
    public class CustomAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.User;

            //Demo purpose only. The custom principal could be retrived via the current context.
            var xp=new MyCustomPrincipal(filterContext.HttpContext.User.Identity, new[] { "Admin" }, "Red");
            xp.IsSessionAlive = filterContext.HttpContext.Session[LoginConstants.LoginSession] != null;
            filterContext.Principal = xp;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var sessionAlive = ((MyCustomPrincipal)filterContext.HttpContext.User).IsSessionAlive;
            var user = filterContext.HttpContext.User;

            if (!sessionAlive)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}