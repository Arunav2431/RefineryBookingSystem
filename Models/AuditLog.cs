// File: Models/AuditLog.cs
using System.ComponentModel.DataAnnotations;

namespace RefineryBooking.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        
        public string? UserId { get; set; }
        public string? UserName { get; set; }

        [Required, StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string EntityName { get; set; } = string.Empty;

        public string? EntityId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(1000)]
        public string? Details { get; set; }
    }
}