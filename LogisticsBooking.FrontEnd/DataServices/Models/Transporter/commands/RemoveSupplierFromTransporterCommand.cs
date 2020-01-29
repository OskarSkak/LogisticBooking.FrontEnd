using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Transporter.commands
{
    public class RemoveSupplierFromTransporterCommand
    {
        public Guid TransporterId { get; set; }    
        public Guid SupplierId { get; set; }       
    }
}