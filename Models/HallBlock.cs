using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefineryBooking.Models
{
    public class HallBlock
    {
        public int Id { get; set; }

        [Required]
        public int ConferenceRoomId { get; set; }
        public ConferenceRoom? ConferenceRoom { get; set; }

        [Required]
        public DateTime BlockedDate { get; set; }

        [Required]
        public HallBlockReason Reason { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum HallBlockReason
    {
        Safety,
        Maintenance,
        Other
    }
}
