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
    public class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand>
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<UpdateLocationHandler> _logger;

        public UpdateLocationHandler(ILocationService locationService, ILogger<UpdateLocationHandler> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        public async Task Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _locationService.UpdateAsync(request);
        }
    }
}
