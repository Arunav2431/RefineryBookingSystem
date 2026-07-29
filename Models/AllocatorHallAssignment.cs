// File: Models/AllocatorHallAssignment.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefineryBooking.Models
{
    /// <summary>
    /// Represents the assignment of an Allocator to a specific Conference Room.
    /// An allocator can only see and act on bookings for their assigned halls.
    /// One hall can have multiple allocators assigned. (Many-to-Many)
    /// </summary>
    public class AllocatorHallAssignment
    {
        public int Id { get; set; }

        /// <summary>The Allocator user assigned to this hall.</summary>
        [Required]
        public string AllocatorUserId { get; set; } = string.Empty;

        [ForeignKey(nameof(AllocatorUserId))]
        public ApplicationUser? Allocator { get; set; }

        /// <summary>The conference room this allocator is assigned to.</summary>
        public int ConferenceRoomId { get; set; }

        [ForeignKey(nameof(ConferenceRoomId))]
        public ConferenceRoom? ConferenceRoom { get; set; }

        /// <summary>When this assignment was made.</summary>
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Who made the assignment (Admin user ID).</summary>
        public string? AssignedByUserId { get; set; }
    }
}
