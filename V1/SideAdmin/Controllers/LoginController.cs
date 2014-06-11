using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SideInfrastructure.Model.Edmx;
using SideAdmin.Models.ViewModel;
using V1.Models.ViewModel;
using SideAdmin.Utility;

namespace V1.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private const string KUSERID = "PreloginUserId";
        public ActionResult Login()
        {
            HttpContext.Session[LoginConstants.LoginSession] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!this.Authenticate(model.UserName, this.HashPassword(model.Password)))
            {

                return View();
            }
            return RedirectToAction("Index", "Admin");
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var personId = this.AddPerson(model);
                if (personId == -1)
                {
                    return this.ShowMessage("Existing email address", UserMessageInfoViewModel.InfoType.Error);
                }
                string activationLink = this.GetActivationLink(model.Email, personId);
                this.SendMail("Admin@side-interior.com", model.Email, "Activation", activationLink);
            }
            return this.ShowMessage("Successfully added, one activation mail has been send to you, please follow to proceed", UserMessageInfoViewModel.InfoType.success); ;
        }

        private ActionResult ShowMessage(string message, UserMessageInfoViewModel.InfoType type)
        {
            ViewBag.Message = message;
            return View("Message", new UserMessageInfoViewModel()
            {
                Message = message,
                MessageType = type
            });
        }

        public ActionResult Validate()
        {
            if (Request.QueryString["t"] != null)
            {
                long userId = 0;
                if (this.ValidateToken(Request.QueryString["t"], out userId))
                {
                    TempData.Add(KUSERID, userId);
                    return RedirectToAction("ChangePassword", "Login");
                }
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.ChangePass(model.Password, (long)TempData[KUSERID]);
            }
            return View();
        }
        #region private methods
        private bool ChangePass(string password, long userID)
        {
            bool returnVal = false;
            using (SIDEContxts context = new SIDEContxts())
            {
                var user = (from v in context.Users where v.UserId == userID && v.IsActive == false select v).FirstOrDefault();
                if (user != null)
                {
                    user.Password = this.HashPassword(password);
                    user.IsActive = true;
                }
                context.SaveChanges();
            }
            return returnVal;
        }

        private string HashPassword(string password)
        {
            return System.Text.ASCIIEncoding.ASCII.GetString(SHA1.Create().ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(password)));
        }
        private bool ValidateToken(string tokenId, out long retUserId)
        {
            retUserId = 0;
            bool validToken = false;
            try
            {
                var splitedToken = this.DecodeBase64String(tokenId).Split(':');
                if (splitedToken.Length < 3)
                {
                    return false;
                }
                else
                {
                    var token = splitedToken[0];
                    var email = splitedToken[1];
                    long userId = 0;
                    if (Int64.TryParse(splitedToken[2], out userId))
                    {
                        retUserId = userId;
                    }
                    using (SIDEContxts context = new SIDEContxts())
                    {
                        if ((from v in context.People
                             join u in context.Users on v.PersonId equals u.PersonId
                             where u.UserId == userId && v.Email == email
                             select u).Any())
                        {
                            validToken = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }

            return validToken;
        }

        private bool Authenticate(string userName, string password)
        {
            bool isAuthenticate = false;
            using (SIDEContxts context = new SIDEContxts())
            {
                if (context.Users.Any(p => p.UserName == userName && p.Password == password && p.IsActive == true))
                {
                    isAuthenticate = true;
                    var user = (from v in context.Users
                               join p in context.People
                               on v.PersonId equals p.PersonId

                               where v.UserName == userName && v.Password == password
                               select new UserViewModel()
                               {
                                   FirstName = p.FirstName,
                                   LastName = p.LastName,
                                   Email = p.Email

                               }).FirstOrDefault();
                    if (user != null)
                    {
                        HttpContext.Session[LoginConstants.LoginSession] = user;
                    }
                }
            }

            return isAuthenticate;
        }

        private long AddPerson(UserViewModel model)
        {
            long id = 0;
            using (SIDEContxts context = new SIDEContxts())
            {
                if ((from v in context.People
                     join u in context.Users on v.PersonId equals u.PersonId
                     where v.Email == model.Email
                     select v.PersonId).Any())
                {
                    return -1;
                }
                var person = new Person()
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    DOB = model.DateOfBirth,
                    Sex = model.Gender,
                    CreationDate = DateTime.Now,
                    Email = model.Email
                };
                context.People.Add(person);

                context.SaveChanges();
                var user = new User()
                {
                    PersonId = person.PersonId,
                    UserName = person.Email,
                    Password = this.HashPassword("side-interior"),
                    IsActive = false
                };
                context.Users.Add(user);
                context.SaveChanges();
                id = user.UserId;
            }
            return id;
        }

        private string GetActivationLink(string emailAddress, long personId)
        {
            return string.Format("http://QA.side-interior.com/login/validate/?t={0}", this.FormatLink(this.GetActivationToken(emailAddress, personId), emailAddress, personId));
        }
        private string GetActivationToken(string emailAddress, long personId)
        {
            string formatedPhrease = emailAddress + personId.ToString() + DateTime.Now.Ticks.ToString();
            return System.Text.ASCIIEncoding.ASCII.GetString(SHA1.Create().ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(formatedPhrease)));
        }

        private string DecodeBase64String(string base64bitEncodedString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64bitEncodedString));
        }

        private string EncodeBase64String(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        private string FormatLink(string token, string email, long personid)
        {
            return this.EncodeBase64String(string.Format("{0}:{1}:{2}", token, email, personid.ToString()));
        }

        private void SendMail(string fromAddr, string toAddr, string subject, string mailbody)
        {
            int port = 25;
            string server = "mail.side-interior.com";
            string user = "Admin@side-interior.com";
            string pass = "Surajit@20";
            ThreadPool.QueueUserWorkItem(t =>
            {
                try
                {
                    using (SmtpClient client = new SmtpClient(server, port))
                    {
                        MailAddress from = new MailAddress(fromAddr, String.Empty, System.Text.Encoding.UTF8);
                        MailAddress to = new MailAddress(toAddr);

                        using (MailMessage message = new MailMessage(from, to))
                        {
                            message.Body = mailbody;
                            message.Subject = subject;
                            message.IsBodyHtml = true;

                            message.BodyEncoding = System.Text.Encoding.UTF8;
                            message.SubjectEncoding = System.Text.Encoding.UTF8;

                            client.EnableSsl = false;
                            client.Credentials = new System.Net.NetworkCredential(user, pass);
                            client.Send(message);
                        }
                    }
                }
                catch (Exception exception)
                {
                    // logger.Error(exception);
                }
            });
        }

        #endregion
    }
}
