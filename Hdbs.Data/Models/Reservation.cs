using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return StartDate < EndDate && (EndDate - StartDate).Days <= 7;
        }
    }
}
