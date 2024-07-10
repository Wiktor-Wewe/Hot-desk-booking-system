using Hdbs.Transfer.Locations.Commands;
using Hdbs.Transfer.Locations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Interfaces
{
    public interface ILocationService
    {
        Task<LocationDto> CreateAsync(CreateLocationCommand command);
        Task UpdateAsync(UpdateLocationCommand command);
        Task DeleteAsync(DeleteLocationCommand command);
    }
}
