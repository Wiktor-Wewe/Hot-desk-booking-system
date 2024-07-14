using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using Hdbs.Transfer.Reservations.Data;
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
    public class ListReservationsByEmployeeHandler : IRequestHandler<ListReservationsByEmployeeQuery, PaginatedList<ReservationListDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ListReservationsByEmployeeHandler> _logger;

        public ListReservationsByEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<ListReservationsByEmployeeHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<ReservationListDto>> Handle(ListReservationsByEmployeeQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeRepository.ListReservationsAsync(request);
        }
    }
}
