
using EventEase.Models;
using System.ComponentModel.DataAnnotations;

namespace EventEas3.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public DateTime bDate { get; set; }

        [Required]
        public TimeSpan bTime { get; set; }

        [Required]
        [StringLength(100)]
        public string bName { get; set; }

        // Navigation Property
        public ICollection<EventEaser> EventEasers { get; set; }
    }
}


