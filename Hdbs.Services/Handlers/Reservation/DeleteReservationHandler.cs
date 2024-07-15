using Hdbs.Services.Implementations;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Reservations.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers.Reservation
{
    public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<DeleteReservationHandler> _logger;

        public DeleteReservationHandler(IReservationService reservationService, ILogger<DeleteReservationHandler> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        public async Task Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _reservationService.DeleteAsync(request);
        }
    }
}
