using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Repositories.Handlers
{
    public class ListDesksByLocationHandler : IRequestHandler<ListDesksByLocationQuery, PaginatedList<DeskListDto>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<ListDesksByLocationHandler> _logger;

        public ListDesksByLocationHandler(ILocationRepository locationRepository, ILogger<ListDesksByLocationHandler> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<DeskListDto>> Handle(ListDesksByLocationQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _locationRepository.ListDesksAsync(request);
        }
    }
}
