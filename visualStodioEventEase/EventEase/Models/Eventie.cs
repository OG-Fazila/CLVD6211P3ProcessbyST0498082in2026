
using EventEase.Models;
using System.ComponentModel.DataAnnotations;

namespace EventEas3.Models
{
    public class Eventie
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        public DateTime eDateTime { get; set; }

        [Required]
        [StringLength(50)]
        public string eType { get; set; }

        [Required]
        public int eDuration { get; set; }

        [Required]
        public int eLimit { get; set; }

        // Navigation Property
        public ICollection<EventEaser> EventEasers { get; set; }
    }
}


