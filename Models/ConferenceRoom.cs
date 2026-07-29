// File: Models/ConferenceRoom.cs
using System.ComponentModel.DataAnnotations;

namespace RefineryBooking.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string HallCode { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string OwnerDepartment { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string CostCentreCode { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string BuildingLocation { get; set; } = string.Empty;

        [StringLength(20)]
        public string? FloorNumber { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedByUserId { get; set; }

        [Range(1, 500)]
        public int Capacity { get; set; }

        public bool HasVideoConferencing { get; set; }
        public bool HasProjector { get; set; }
        public bool HasWhiteboard { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}