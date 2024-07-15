using MediatR;

namespace Hdbs.Transfer.Employees.Commands
{
    public class UpdateMyEmployeeCommand : IRequest
    {
        public string? EmployeeId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public string? NewPassword { get; set; }
        public string? RePassword { get; set; }
    }
}
