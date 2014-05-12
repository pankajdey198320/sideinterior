using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SideAdmin.Utility
{
    public static class ConfigurationUtility
    {
        public static string GetDocServerUrl() {
            return System.Configuration.ConfigurationManager.AppSettings[ConfigurationConstants.DocserverUrl];
        }
    }
}