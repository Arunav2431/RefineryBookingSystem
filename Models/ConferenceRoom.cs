// File: Models/ConferenceRoom.cs
using System.ComponentModel.DataAnnotations;

namespace RefineryBooking.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string BuildingLocation { get; set; } = string.Empty;

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