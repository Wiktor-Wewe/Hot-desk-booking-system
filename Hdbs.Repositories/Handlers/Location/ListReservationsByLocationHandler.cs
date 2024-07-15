using Hdbs.Repositories.Implementations;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Repositories.Handlers.Location
{
    public class ListReservationsByLocationHandler : IRequestHandler<ListReservationsByLocationQuery, PaginatedList<ReservationListDto>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<ListReservationsByLocationHandler> _logger;

        public ListReservationsByLocationHandler(ILocationRepository locationRepository, ILogger<ListReservationsByLocationHandler> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<ReservationListDto>> Handle(ListReservationsByLocationQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _locationRepository.ListReservationsAsync(request);
        }
    }
}
