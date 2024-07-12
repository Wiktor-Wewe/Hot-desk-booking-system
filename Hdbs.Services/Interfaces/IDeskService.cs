﻿using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Desks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Interfaces
{
    public interface IDeskService
    {
        Task<DeskDto> CreateAsync(CreateDeskCommand command);
        Task UpdateAsync(UpdateDeskCommand command);
        Task DeleteAsync(DeleteDeskCommand command);
    }
}