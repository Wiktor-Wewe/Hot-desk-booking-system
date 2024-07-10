using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers
{
    public class GetLocationHandler : IRequestHandler<GetLocationQuery, LocationDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<GetLocationHandler> _logger;

        public GetLocationHandler(ILocationRepository reportRepository, ILogger<GetLocationHandler> logger)
        {
            _locationRepository = reportRepository;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _locationRepository.GetAsync(request);
        }
    }
}
