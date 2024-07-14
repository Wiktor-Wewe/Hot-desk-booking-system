using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Hdbs.Data.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Desk")]
        public Guid DeskId { get; set; }
        public Desk Desk { get; set; } = null!;

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; } = null!;
        public Employee Employee { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsValid()
        {
            return StartDate.Date <= EndDate.Date && (EndDate.Date - StartDate.Date).Days <= 7;
        }

        public bool IsExpiredRightNow()
        {
            return DateTime.Now.Date > EndDate.Date;
        }

        public bool IsFreeRightNow()
        {
            var date = DateTime.Now.Date;
            return date < StartDate.Date || date > EndDate.Date;
        }

        public bool IsFree(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                throw new ArgumentException("endDate must be greater than or equal to startDate");
            }

            return endDate.Date < StartDate.Date || startDate.Date > EndDate.Date;
        }
    }
}
