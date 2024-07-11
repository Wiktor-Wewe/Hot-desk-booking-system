using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {query.Id}");
            }

            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Reservations = employee.Reservations
            };
        }

        public async Task<PaginatedList<EmployeeListDto>> ListAsync(ListEmployeesQuery listAsyncQuery)
        {
            var query = _dbContext.Employees
                .Include(e => e.Reservations)
                .AsNoTracking()
                .OrderBy(e => e.Id);

            return await PaginatedList<EmployeeListDto>.CreateAsync(query.Select(e => new EmployeeListDto
            {
                Id = e.Id,
                Name = e.Name,
                Reservations = e.Reservations
            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );
        }
    }
}
