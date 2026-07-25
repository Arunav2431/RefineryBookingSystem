// File: Models/Booking.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefineryBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ConferenceRoomId { get; set; }
        [ForeignKey(nameof(ConferenceRoomId))]
        public ConferenceRoom? ConferenceRoom { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        [Required, StringLength(150)]
        public string MeetingTitle { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Range(1, 500)]
        public int ExpectedAttendees { get; set; }

        public bool RequiresCatering { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [StringLength(250)]
        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (1-to-1)
        public ITFacilityRequirement? ITRequirement { get; set; }
    }
}