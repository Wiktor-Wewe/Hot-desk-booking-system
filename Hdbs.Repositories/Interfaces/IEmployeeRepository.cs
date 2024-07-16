using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;

namespace Hdbs.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<PaginatedList<EmployeeListDto>> ListAsync(ListEmployeesQuery listAsyncQuery);
        Task<PaginatedList<ReservationListDto>> ListReservationsAsync(ListReservationsByEmployeeQuery listAsyncQuery);
        Task<EmployeeDto> GetMeEmployeeAsync(GetMeEmployeeQuery query);
        Task<EmployeeDto> GetAsync(GetEmployeeQuery query);
        Task<MemoryStream> GetImportExcelAsync(GetImportExcelEmployeeQuery query);
    }
}
