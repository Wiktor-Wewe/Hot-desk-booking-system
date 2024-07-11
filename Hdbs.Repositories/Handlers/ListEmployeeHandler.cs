﻿using Hdbs.Repositories.Implementations;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Desks.Queries;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
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
    public class ListEmployeeHandler : IRequestHandler<ListEmployeesQuery, PaginatedList<EmployeeListDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ListEmployeeHandler> _logger;

        public ListEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<ListEmployeeHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<EmployeeListDto>> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeRepository.ListAsync(request);
        }
    }
}
