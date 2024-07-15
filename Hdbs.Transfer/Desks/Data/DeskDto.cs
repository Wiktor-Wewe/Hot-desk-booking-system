using Hdbs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Data
{
    public class DeskDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public string LocationCity { get; set; } = null!;
        public string LocationCountry { get; set; } = null!;
        public bool? IsAvailable { get; set; }

    }
}
