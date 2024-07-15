using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Desks.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Desk
{
    public class CreateDeskHandler : IRequestHandler<CreateDeskCommand, DeskDto>
    {
        private readonly IDeskService _deskService;
        private readonly ILogger<CreateDeskHandler> _logger;

        public CreateDeskHandler(IDeskService deskService, ILogger<CreateDeskHandler> logger)
        {
            _deskService = deskService;
            _logger = logger;
        }

        public async Task<DeskDto> Handle(CreateDeskCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _deskService.CreateAsync(request);
        }
    }
}
