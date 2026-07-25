// File: Models/ITFacilityRequirement.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefineryBooking.Models
{
    public class ITFacilityRequirement
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }

        public bool NeedsVideoConferencing { get; set; }
        public bool NeedsProjector { get; set; }
        
        [Range(0, 20)]
        public int MicCount { get; set; }

        public TechSetupStatus SetupStatus { get; set; } = TechSetupStatus.Pending;

        [StringLength(500)]
        public string? TechNotes { get; set; }
    }
}