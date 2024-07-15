using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Locations.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers.Location
{
    public class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand>
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<DeleteLocationHandler> _logger;

        public DeleteLocationHandler(ILocationService projectService, ILogger<DeleteLocationHandler> logger)
        {
            _locationService = projectService;
            _logger = logger;
        }

        public async Task Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _locationService.DeleteAsync(request);
        }
    }
}
