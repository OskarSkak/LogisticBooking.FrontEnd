namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class UpdatePasswordCommand
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public ApplicationUserViewModel User { get; set; }
    }
}