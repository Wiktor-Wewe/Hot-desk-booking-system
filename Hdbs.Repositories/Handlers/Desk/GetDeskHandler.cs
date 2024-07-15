using Hdbs.Repositories.Implementations;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Desks.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Handlers.Desk
{
    public class GetDeskHandler : IRequestHandler<GetDeskQuery, DeskDto>
    {
        private readonly IDeskRepository _deskRepository;
        private readonly ILogger<GetDeskHandler> _logger;

        public GetDeskHandler(IDeskRepository deskRepository, ILogger<GetDeskHandler> logger)
        {
            _deskRepository = deskRepository;
            _logger = logger;
        }

        public async Task<DeskDto> Handle(GetDeskQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _deskRepository.GetAsync(request);
        }
    }
}
