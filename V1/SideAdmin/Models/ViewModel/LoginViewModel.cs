using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace V1.Models.ViewModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserViewModel {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Email { get; set; }

        public LoginViewModel LoginDetails { get; set; }
    }

    public class ChangePasswordViewModel {
        public string Password { get; set; }
        public string RepeatePassword { get; set; }
    }
}
