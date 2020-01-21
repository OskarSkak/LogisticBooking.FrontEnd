using LogisticBooking.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogisticsBooking.FrontEnd
{
    public class ApiDbContext : DbContext , ILogisticBookingApiDatabase
    {

        
        
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Interval> Intervals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<UtilBooking> UtilBookings { get; set; }
        public DbSet<DeletedBooking> DeletedBookings { get; set; }
        
        public DbSet<MasterIntervalStandard> MasterIntervalStandards { get; set; }
        
        public DbSet<MasterScheduleStandard> MasterScheduleStandards { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("UtilBookingNumberStart", schema: "shared")
                .StartsAt(50000)
                .IncrementsBy(1);  
            modelBuilder.Entity<UtilBooking>()
                .Property(o => o.Bookingid)
                .HasDefaultValueSql("NEXT VALUE FOR shared.sampleNumber");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
        } 
       
    }
}