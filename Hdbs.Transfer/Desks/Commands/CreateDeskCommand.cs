using Hdbs.Data.Models;
using Hdbs.Transfer.Desks.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Commands
{
    public class CreateDeskCommand : IRequest<DeskDto>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid LocationId { get; set; }
    }
}
