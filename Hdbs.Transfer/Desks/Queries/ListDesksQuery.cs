using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Shared.Data;
using Hdbs.Transfer.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Queries
{
    public class ListDesksQuery : ListQuery, IRequest<PaginatedList<DeskListDto>>
    {
    }
}
