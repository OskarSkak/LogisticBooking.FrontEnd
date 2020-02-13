using System;
using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class CreateUserCommand
    {
        public string Email { get; set; }
        public string Name { get; set; }
        
        public List<string> Role { get; set; }
        
        public Guid Id { get; set; }
    }
}