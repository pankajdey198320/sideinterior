using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideInfrastructure.Model.ViewModel
{
    public class UserProfileViewModel
    {
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get { return dobx != null ? dobx.Value.ToShortDateString() : string.Empty; } set {
            DateTime d = new DateTime();
            if (DateTime.TryParse(value, out d)) {
                dobx = d;
            }
        } }
        public string Sex { get; set; }
        public string Email { get; set; }

        public Nullable<DateTime> dobx { get; set; }
    }
}
