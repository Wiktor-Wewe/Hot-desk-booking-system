using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Queries;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Repositories.Handlers
{
    public class ListReservationsByDeskHandler : IRequestHandler<ListReservationsByDeskQuery, PaginatedList<ReservationListDto>>
    {
        private readonly IDeskRepository _deskRepository;
        private readonly ILogger<ListReservationsByDeskHandler> _logger;

        public ListReservationsByDeskHandler(IDeskRepository deskRepository, ILogger<ListReservationsByDeskHandler> logger)
        {
            _deskRepository = deskRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<ReservationListDto>> Handle(ListReservationsByDeskQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _deskRepository.ListReservationsAsync(request);
        }
    }
}
