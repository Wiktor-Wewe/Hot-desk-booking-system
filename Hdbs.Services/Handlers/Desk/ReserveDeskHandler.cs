using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Reservations.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Desk
{
    public class ReserveDeskHandler : IRequestHandler<ReserveDeskCommand, ReservationDto>
    {
        private readonly IDeskService _deskService;
        private readonly ILogger<ReserveDeskHandler> _logger;

        public ReserveDeskHandler(IDeskService deskService, ILogger<ReserveDeskHandler> logger)
        {
            _deskService = deskService;
            _logger = logger;
        }

        public async Task<ReservationDto> Handle(ReserveDeskCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _deskService.ReserveDeskAsync(request);
        }
    }
}
