using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Locations.Commands;
using Hdbs.Transfer.Locations.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers
{
    public class CreateLocationHandler : IRequestHandler<CreateLocationCommand, LocationDto>
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<CreateLocationHandler> _logger;

        public CreateLocationHandler(ILocationService locationService, ILogger<CreateLocationHandler> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _locationService.CreateAsync(request);
        }
    }
}
