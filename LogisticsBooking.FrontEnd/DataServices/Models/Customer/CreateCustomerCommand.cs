namespace LogisticsBooking.FrontEnd.DataServices.Models.Customer
{
    public class CreateCustomerCommand
    {
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string TelephoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
    }
}