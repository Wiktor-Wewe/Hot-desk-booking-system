﻿using Hdbs.Transfer.Employees.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<EmployeeDto>
    {
        public string Name { get; set; } = null!;
    }
}
