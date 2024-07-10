using Hdbs.Data.Models;
using Hdbs.Transfer.Locations.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Locations.Commands
{
    public class CreateLocationCommand : IRequest<LocationDto>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Desk> Desks { get; set; } = [];
    }
}
