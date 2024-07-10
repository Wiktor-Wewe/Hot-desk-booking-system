using Hdbs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Locations.Data
{
    public class LocationDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Desk>? Desks { get; set; }
    }
}
