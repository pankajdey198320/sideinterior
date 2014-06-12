using System.Web;
using SideInfrastructure.Model.Edmx;
using V1.Models.ViewModel;

namespace SideAdmin.Utility
{
    public static class Helper
    {
        public static UserViewModel GetLoggedinUser()
        {

            return (UserViewModel)HttpContext.Current.Session[LoginConstants.LoginSession];
        }

        public static void SetUserSession(UserViewModel usr)
        {
            HttpContext.Current.Session[LoginConstants.LoginSession] = usr;
        }

        public static void ResetUserSession() {
            HttpContext.Current.Session[LoginConstants.LoginSession] = null;
        }
    }
}