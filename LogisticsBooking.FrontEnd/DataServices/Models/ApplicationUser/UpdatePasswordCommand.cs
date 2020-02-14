namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class UpdatePasswordCommand
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string UserId { get; set; }
    }
}