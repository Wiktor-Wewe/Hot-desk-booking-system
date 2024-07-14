using Hdbs.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Reservations.Data
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid DeskId { get; set; }
        public Desk? Desk { get; set; }
        public string EmployeeId { get; set; } = null!;
        public Employee? Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsExpired { get; set; }
        public bool IsActive {  get; set; }
    }
}
