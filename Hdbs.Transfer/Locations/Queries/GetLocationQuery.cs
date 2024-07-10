using Hdbs.Transfer.Locations.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Locations.Queries
{
    public class GetLocationQuery : IRequest<LocationDto>
    {
        public Guid Id { get; set; }
    }
}
