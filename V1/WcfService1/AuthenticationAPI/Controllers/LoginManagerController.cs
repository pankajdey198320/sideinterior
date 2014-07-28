using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Services;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AuthenticationAPI.Controllers
{
    public class LoginManagerController : Controller
    {
        CustomSecurityTokenServiceConfiguration stsConfiguration;
        SecurityTokenService securityTokenService;

        public LoginManagerController()
        {
            stsConfiguration = new CustomSecurityTokenServiceConfiguration();
            securityTokenService = new CustomSecurityTokenService(this.stsConfiguration);
        }
        //
        // GET: /LoginManager/
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        public ActionResult Issue(string wa, string wtrealm, string wctx, string wct, string wreply = "")
        {
            if (wa == "wsignin1.0")
                return View("Login");

            return View();
        }
       
        [HttpPost]
        public System.Web.Mvc.ActionResult Login(string wa, string wtrealm, string wctx, string wct, string wreply = "")
        {
            if (Request["uname"] == null || Request["pass"] == null)
                return View();
            var username = Convert.ToString(Request["uname"]);
            var pass = Convert.ToString(Request["pass"]);
            if (username == "pankaj" && pass == "dey")
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                string fullRequest = Constants.HttpLocalhost +

                    Constants.WSFedStsIssue +
                    string.Format("?wa=wsignin1.0&wtrealm={0}&wctx={1}&wct={2}&wreply={3}", wtrealm, HttpUtility.UrlEncode(wctx), wct, wreply);

                SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(new Uri(fullRequest));

                ClaimsIdentity identity = new ClaimsIdentity();
                ///  identity.AuthenticationType = AuthenticationTypes.Federation;
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, principal, this.securityTokenService);
                responseMessage.Write(writer);

                writer.Flush();
                stream.Position = 0;
                return File(stream, "text/html");
            }
            return View();
        }
    }
}