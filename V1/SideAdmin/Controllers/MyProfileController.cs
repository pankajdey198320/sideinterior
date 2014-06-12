using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SideAdmin.Filters;
using SideAdmin.Utility;
using SideInfrastructure.Model.ViewModel;
using SideInfrastructure.Service;

namespace SideAdmin.Controllers
{
    [CustomAuthentication]
	public class MyProfileController : Controller
	{

        UserService svc = new UserService();
		//
		// GET: /MyProfile/
		public ActionResult Index()
		{
			return View("MyProfile");
		}

        public ActionResult GetMyProfile() { 
            
            return Json(svc.GetUserProfile(Helper.GetLoggedinUser().UserID),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProfile(UserProfileViewModel model) {
            model.UserID = Helper.GetLoggedinUser().UserID;
            svc.SaveUserProfile(model);
            return Json(true);
        }
	}
}