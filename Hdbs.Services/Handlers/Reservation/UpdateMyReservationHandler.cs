using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Reservations.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Reservation
{
    public class UpdateMyReservationHandler : IRequestHandler<UpdateMyReservationCommand>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<UpdateMyReservationHandler> _logger;

        public UpdateMyReservationHandler(IReservationService reservationService, ILogger<UpdateMyReservationHandler> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        public async Task Handle(UpdateMyReservationCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _reservationService.UpdateMyAsync(request);
        }
    }
}
