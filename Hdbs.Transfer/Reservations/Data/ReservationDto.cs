using Hdbs.Data.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
        public string DeskName { get; set; } = null!;
        public string LocationName { get; set; } = null!;
        public string LocationCity { get; set; } = null!;
        public string LocationCountry { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string EmployeeSurname { get; set; } = null!;
        public string EmployeeEmail { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsExpired { get; set; }
        public bool IsActive {  get; set; }
    }
}
