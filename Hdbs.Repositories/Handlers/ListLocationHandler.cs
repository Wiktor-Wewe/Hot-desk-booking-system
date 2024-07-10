﻿using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers
{
    public class ListLocationHandler : IRequestHandler<ListLocationsQuery, PaginatedList<LocationListDto>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<ListLocationHandler> _logger;

        public ListLocationHandler(ILocationRepository locationRepository, ILogger<ListLocationHandler> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<LocationListDto>> Handle(ListLocationsQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _locationRepository.ListAsync(request);
        }
    }
}