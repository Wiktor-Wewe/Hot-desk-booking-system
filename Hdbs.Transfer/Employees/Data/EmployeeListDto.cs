using Hdbs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Data
{
    public class EmployeeListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
