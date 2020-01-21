using System.Threading;
using System.Threading.Tasks;
using LogisticBooking.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogisticsBooking.FrontEnd
{
    public interface ILogisticBookingApiDatabase
    {
        DbSet<Booking> Bookings { get; set; }
        
        DbSet<Interval> Intervals { get; set; }
        
        DbSet<Order> Orders { get; set; }
        
        DbSet<Schedule> Schedules { get; set; }
        
        DbSet<Supplier> Suppliers { get; set; }
        
        DbSet<Transporter> Transporters { get; set; }
        
        DbSet<UtilBooking> UtilBookings { get; set; }
        
        DbSet<MasterIntervalStandard> MasterIntervalStandards { get; set; }
        
        DbSet<MasterScheduleStandard> MasterScheduleStandards { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        DbSet<DeletedBooking> DeletedBookings { get; set; }
    }
}