using Hdbs.Transfer.Desks.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Queries
{
    public class GetDeskQuery : IRequest<DeskDto>
    {
        public Guid Id { get; set; }
    }
}
