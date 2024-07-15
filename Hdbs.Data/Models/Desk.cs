using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hdbs.Data.Models
{
    public class Desk
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public bool ForcedUnavailable { get; set; } = false;

        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [ForeignKey("Location")]
        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;


        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
