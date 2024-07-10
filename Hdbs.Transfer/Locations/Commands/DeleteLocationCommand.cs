using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Locations.Commands
{
    public class DeleteLocationCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
