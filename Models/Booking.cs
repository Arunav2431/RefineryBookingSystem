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

        [Required, StringLength(100)]
        public string OrganizerName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string CostCentre { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Range(1, 500)]
        public int ExpectedAttendees { get; set; }

        public bool RequiresCatering { get; set; }

        [StringLength(1000)]
        public string? Remarks { get; set; }

        // ── Help Requests (optional, user-declared) ──────────────────────────
        /// <summary>User flagged they need AV/IT/equipment help from ITFM.</summary>
        public bool RequiresITFMHelp { get; set; }

        [StringLength(500)]
        public string? ITFMHelpDetails { get; set; }

        /// <summary>User flagged they need scheduling/room help from Allocator.</summary>
        public bool RequiresAllocatorHelp { get; set; }

        [StringLength(500)]
        public string? AllocatorHelpDetails { get; set; }

        [StringLength(300)]
        public string? AttachmentPath { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [StringLength(250)]
        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (1-to-1)
        public ITFacilityRequirement? ITRequirement { get; set; }
    }
}