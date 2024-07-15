using Hdbs.Repositories.Implementations;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Desks.Queries;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers.Desk
{
    public class ListDeskHandler : IRequestHandler<ListDesksQuery, PaginatedList<DeskListDto>>
    {
        private readonly IDeskRepository _deskRepository;
        private readonly ILogger<ListDeskHandler> _logger;

        public ListDeskHandler(IDeskRepository deskRepository, ILogger<ListDeskHandler> logger)
        {
            _deskRepository = deskRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<DeskListDto>> Handle(ListDesksQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _deskRepository.ListAsync(request);
        }
    }
}
