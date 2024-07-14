﻿using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Reservations.Queries;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers
{
    public class ListMyReservationsHandler : IRequestHandler<ListMyReservationsQuery, PaginatedList<ReservationListDto>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogger<ListMyReservationsHandler> _logger;

        public ListMyReservationsHandler(IReservationRepository reservationRepository, ILogger<ListMyReservationsHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<ReservationListDto>> Handle(ListMyReservationsQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _reservationRepository.ListMyReservationsAsync(request);
        }
    }
}
