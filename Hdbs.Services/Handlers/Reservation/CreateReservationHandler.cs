using Azure.Core;
using Hdbs.Services.Implementations;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Reservations.Commands;
using Hdbs.Transfer.Reservations.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, ReservationDto>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<CreateReservationHandler> _logger;

        public CreateReservationHandler(IReservationService reservationService, ILogger<CreateReservationHandler> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _reservationService.CreateAsync(request);
        }
    }
}
