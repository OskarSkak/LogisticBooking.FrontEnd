namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class CreateUserCommand
    {
        public string Email { get; set; }
        public string Name { get; set; }
        
        public string Role { get; set; }
    }
}