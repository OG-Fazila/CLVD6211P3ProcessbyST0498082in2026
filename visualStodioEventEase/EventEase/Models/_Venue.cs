
using EventEase.Models;
using System.ComponentModel.DataAnnotations;

namespace EventEas3.Models
{
    public class _Venue
    {
        [Key]
        public int VenueID { get; set; }

        [Required]
        [StringLength(100)]
        public string vLocation { get; set; }

        [Required]
        [StringLength(100)]
        public string vName { get; set; }

        [Required]
        [StringLength(200)]
        public string vAddress { get; set; }

        [StringLength(255)]
        public string vImage { get; set; }

        // Navigation Property
        public ICollection<EventEaser> EventEasers { get; set; }
    }
}