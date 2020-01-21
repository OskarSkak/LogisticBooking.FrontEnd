using System;

namespace LogisticBooking.API.Domain.Entities
{
    public class Settings
    {
        public Guid SettingsId { get; set; }
        
        public int Port { get; set; } 
    }
}