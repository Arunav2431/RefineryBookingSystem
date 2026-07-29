using System.ComponentModel.DataAnnotations;

namespace RefineryBooking.Models
{
    public class CostCentre
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Code { get; set; } = string.Empty;

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
