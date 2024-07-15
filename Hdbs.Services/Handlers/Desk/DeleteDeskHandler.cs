using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers.Desk
{
    public class DeleteDeskHandler : IRequestHandler<DeleteDeskCommand>
    {
        private readonly IDeskService _deskService;
        private readonly ILogger<DeleteDeskHandler> _logger;

        public DeleteDeskHandler(IDeskService deskService, ILogger<DeleteDeskHandler> logger)
        {
            _deskService = deskService;
            _logger = logger;
        }

        public async Task Handle(DeleteDeskCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _deskService.DeleteAsync(request);
        }
    }
}
