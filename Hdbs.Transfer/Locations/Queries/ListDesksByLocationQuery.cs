using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Shared.Data;
using Hdbs.Transfer.Shared.Queries;
using MediatR;

namespace Hdbs.Transfer.Locations.Queries
{
    public class ListDesksByLocationQuery : ListQuery, IRequest<PaginatedList<DeskListDto>>
    {
        public Guid? LocationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}
    }
}
