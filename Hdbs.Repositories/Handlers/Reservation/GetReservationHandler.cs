using Hdbs.Repositories.Implementations;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Reservations.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers.Reservation
{
    public class GetReservationHandler : IRequestHandler<GetReservationQuery, ReservationDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger<GetReservationHandler> _logger;

        public GetReservationHandler(IReservationRepository reservationRepository, ILogger<GetReservationHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public async Task<ReservationDto> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _reservationRepository.GetAsync(request);
        }
    }
}
