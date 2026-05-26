
using EventEase.Models;

namespace EventEase.Models
{
    public class EventEas3Context : DbContext
    {
        public EventEas3Context(DbContextOptions<EventEas3Context> options)
          : base(options)
        {
        }

        public DbSet<_Venue> _Venues { get; set; }
        public DbSet<Eventie> Eventies { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventEaser> EventEasers { get; set; }
    }
}
