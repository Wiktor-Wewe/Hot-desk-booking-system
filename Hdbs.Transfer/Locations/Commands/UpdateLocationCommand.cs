using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Locations.Commands
{
    public class UpdateLocationCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
