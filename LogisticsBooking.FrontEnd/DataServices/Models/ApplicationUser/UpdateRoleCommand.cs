using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class UpdateRoleCommand
    {
        public Guid ApplicationUserId { get; set; }
        
        public string RoleToAdd { get; set; }
        
        
    }
}