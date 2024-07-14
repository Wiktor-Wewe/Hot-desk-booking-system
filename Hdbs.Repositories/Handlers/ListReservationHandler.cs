using Hdbs.Repositories.Implementations;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Reservations.Queries;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers
{
    public class ListReservationHandler : IRequestHandler<ListReservationsQuery, PaginatedList<ReservationListDto>>
    {
        public readonly IReservationRepository _reservationRepository;
        public readonly ILogger<ListReservationHandler> _logger;

        public ListReservationHandler(IReservationRepository reservationRepository, ILogger<ListReservationHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<ReservationListDto>> Handle(ListReservationsQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _reservationRepository.ListAsync(request);
        }
    }
}
