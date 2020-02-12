using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class UpdateUserWithRolesCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string ApplicationUserId { get; set; }
        
        public UpdateUserWithRolesCommand(){Roles = new List<string>();}
    }
}