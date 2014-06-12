using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideInfrastructure.Model.Edmx;
using SideInfrastructure.Model.ViewModel;

namespace SideInfrastructure.Service
{
    
    public class UserService
    {

        public UserProfileViewModel GetUserProfile(long userId)
        {
            using (SIDEContxts context = new SIDEContxts()) {
                return (from v in context.People
                        join k in context.Users
                            on v.PersonId equals k.PersonId
                        where k.UserId == userId
                        select new UserProfileViewModel() {
                             dobx = v.DOB,
                             Email=v.Email,
                             FirstName = v.FirstName,
                             LastName = v.LastName,
                             MiddleName = v.MiddleName,
                             Sex = v.Sex                        
                        }).FirstOrDefault();
            
            }
        }
        public void SaveUserProfile(UserProfileViewModel model)
        {
            using (SIDEContxts context = new SIDEContxts())
            {
                var pro = (from v in context.Users
                          join k in context.People
                          on v.PersonId equals k.PersonId
                          where v.UserId == model.UserID
                          select k).FirstOrDefault();

                if (pro != null) {
                    pro.FirstName = model.FirstName;
                    pro.LastName = model.LastName;
                    pro.MiddleName = model.MiddleName;
                    pro.DOB = model.dobx;
                    pro.Email = model.Email;
                }
                context.SaveChanges();

            }
        }
    }
}
