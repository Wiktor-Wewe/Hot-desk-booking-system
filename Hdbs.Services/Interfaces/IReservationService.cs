using Hdbs.Transfer.Reservations.Commands;
using Hdbs.Transfer.Reservations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateAsync(CreateReservationCommand command);
        Task UpdateAsync(UpdateReservationCommand command);
        Task UpdateMyAsync(UpdateMyReservationCommand command);
        Task DeleteAsync(DeleteReservationCommand command);
    }
}
