
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEas3.Models
{
    public class EventEaser
    {
        [Key]
        public int EventEaserID { get; set; }

        // Foreign Keys
        public int EventID { get; set; }
        public int VenueID { get; set; }
        public int BookingID { get; set; }

        // Navigation Properties
        [ForeignKey("EventID")]
        public Eventie Eventie { get; set; }

        [ForeignKey("VenueID")]
        public _Venue _Venue { get; set; }

        [ForeignKey("BookingID")]
        public Booking Booking { get; set; }
    }
}