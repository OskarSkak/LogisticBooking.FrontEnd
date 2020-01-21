using System;
 
 namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking
 {
     public class UpdateBookingCommand
     {
         public int ExternalId { get; set; }
         public int TotalPallets { get; set; }
         public DateTime BookingTime { get; set; }
         public string TransporterName { get; set; }
         public int Port { get; set; }
         public DateTime ActualArrival { get; set; }
         public DateTime StartLoading { get; set; }
         public DateTime EndLoading { get; set; }   
      
         public Guid InternalId { get; set; }
         public string Email { get; set; }
         public Guid TransporterId { get; set; }
     
     }
 }