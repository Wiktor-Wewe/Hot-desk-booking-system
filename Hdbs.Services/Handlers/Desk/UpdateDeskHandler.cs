using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Desk
{
    public class UpdateDeskHandler : IRequestHandler<UpdateDeskCommand>
    {
        private readonly IDeskService _deskService;
        private readonly ILogger<UpdateDeskHandler> _logger;

        public UpdateDeskHandler(IDeskService deskService, ILogger<UpdateDeskHandler> logger)
        {
            _deskService = deskService;
            _logger = logger;
        }

        public async Task Handle(UpdateDeskCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _deskService.UpdateAsync(request);
        }
    }
}
