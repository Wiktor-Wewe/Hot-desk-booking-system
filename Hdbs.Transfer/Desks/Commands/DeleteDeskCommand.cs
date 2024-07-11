using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Commands
{
    public class DeleteDeskCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
