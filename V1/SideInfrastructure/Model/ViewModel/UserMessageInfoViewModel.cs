using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SideAdmin.Models.ViewModel
{
    public class UserMessageInfoViewModel
    {
        public string Message { get; set; }
        public InfoType MessageType { get; set; }

       public enum InfoType { 
            Error,info,success,failure
        }
    }
}