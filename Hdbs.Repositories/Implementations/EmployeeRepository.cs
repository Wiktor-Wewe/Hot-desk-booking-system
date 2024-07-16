using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Linq.Dynamic.Core;

namespace Hdbs.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HdbsContext _dbContext;

        public EmployeeRepository(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDto> GetAsync(GetEmployeeQuery query)
        {
            var employee = await _dbContext.Employees
                .Include(d => d.Reservations)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {query.Id}");
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                IsDisabled = employee.IsDisabled,
                Name = employee.UserName == null ? "" : employee.UserName,
                Surname = employee.Surname,
                Email = employee.Email == null ? "" : employee.Email,
                Permissions = employee.Permissions.ToString()
            };
        }

        public async Task<EmployeeDto> GetMeEmployeeAsync(GetMeEmployeeQuery query)
        {
            if(query.Id == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeIdIsNull, "Unable to find employee with id: null");
            }

            var employee = await _dbContext.Employees
                .Include(d => d.Reservations)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {query.Id}");
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                IsDisabled = employee.IsDisabled,
                Name = employee.UserName == null ? "" : employee.UserName,
                Surname = employee.Surname,
                Email = employee.Email == null ? "" : employee.Email,
                Permissions = employee.Permissions.ToString()
            };
        }

        public async Task<PaginatedList<EmployeeListDto>> ListAsync(ListEmployeesQuery listAsyncQuery)
        {
            var query = _dbContext.Employees
                .Include(e => e.Reservations)
                .AsNoTracking()
                .OrderBy(e => e.Id);

            query = (IOrderedQueryable<Employee>)PaginatedList<Employee>.ApplySearchAndSorting(query, listAsyncQuery.SearchBy, listAsyncQuery.SearchFor, listAsyncQuery.OrderBy, listAsyncQuery.Ascending);

            return await PaginatedList<EmployeeListDto>.CreateAsync(query.Select(e => new EmployeeListDto
            {
                Id = e.Id,
                IsDisabled = e.IsDisabled,
                Name = e.UserName == null ? "" : e.UserName,
                Surname = e.Surname,
                Email = e.Email == null ? "" : e.Email
            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );
        }

        public async Task<PaginatedList<ReservationListDto>> ListReservationsAsync(ListReservationsByEmployeeQuery listAsyncQuery)
        {
            if(listAsyncQuery.EmployeeId == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, "Unable to find employee with id: null");
            }

            var query = _dbContext.Reservations
                .Include(r => r.Desk)
                .Include(r => r.Employee)
                .Where(r => r.EmployeeId == listAsyncQuery.EmployeeId)
                .AsNoTracking()
                .OrderBy(d => d.Id);

            if (listAsyncQuery.OnlyActive)
            {
                query = (IOrderedQueryable<Reservation>)query.Where(r => (DateTime.Now.Date < r.StartDate.Date || DateTime.Now.Date > r.EndDate.Date) == false);
            }

            query = (IOrderedQueryable<Reservation>)PaginatedList<Reservation>.ApplySearchAndSorting(query, listAsyncQuery.SearchBy, listAsyncQuery.SearchFor, listAsyncQuery.OrderBy, listAsyncQuery.Ascending);

            return await PaginatedList<ReservationListDto>.CreateAsync(query.Select(d => new ReservationListDto
            {
                Id = d.Id,
                DeskId = d.DeskId,
                LocationName = d.Desk.Location.Name,
                LocationCity = d.Desk.Location.City,
                LocationCountry = d.Desk.Location.Country,
                EmployeeId = d.EmployeeId,
                EmployeeName = d.Employee.UserName == null ? "" : d.Employee.UserName,
                EmployeeSurname = d.Employee.Surname,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                IsExpired = d.IsExpiredRightNow(),
                IsActive = !d.IsFreeRightNow()

            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );
        }

        public async Task<MemoryStream> GetImportExcelAsync(GetImportExcelEmployeeQuery query)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                worksheet.Cells[1, 1].Value = "IsDisabled";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Surname";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Password";
                worksheet.Cells[1, 6].Value = "Permissions";

                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return stream;
            }
        }
    }
}
