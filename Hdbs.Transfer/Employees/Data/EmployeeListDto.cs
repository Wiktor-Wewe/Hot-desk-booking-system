﻿using Hdbs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Data
{
    public class EmployeeListDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserPermissions Permissions { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}