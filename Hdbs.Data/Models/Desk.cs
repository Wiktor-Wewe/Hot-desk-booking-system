using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Data.Models
{
    public class Desk
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [ForeignKey("Location")]
        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public bool IsAvailable { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
