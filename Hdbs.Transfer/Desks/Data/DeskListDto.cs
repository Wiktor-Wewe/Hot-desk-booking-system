using Hdbs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Data
{
    public class DeskListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid LocationId { get; set; }
        public Location? Location { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
