using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
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

            if (!string.IsNullOrEmpty(listAsyncQuery.SearchFor) && !string.IsNullOrEmpty(listAsyncQuery.SearchBy))
            {
                if (Utils.IsValidProperty<Employee>(listAsyncQuery.SearchBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidSearchBy, $"Unable to search by: {listAsyncQuery.SearchBy}");
                }

                listAsyncQuery.SearchFor = listAsyncQuery.SearchFor.Replace("'", "''");
                query = (IOrderedQueryable<Employee>)query.Where($"{listAsyncQuery.SearchBy}.Contains(@0)", listAsyncQuery.SearchFor);
            }

            if (!string.IsNullOrEmpty(listAsyncQuery.OrderBy))
            {
                if (Utils.IsValidProperty<Employee>(listAsyncQuery.OrderBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidOrderBy, $"Unable to order by: {listAsyncQuery.OrderBy}");
                }

                query = listAsyncQuery.Ascending
                    ? query.OrderBy(listAsyncQuery.OrderBy)
                    : query.OrderBy($"{listAsyncQuery.OrderBy} descending");
            }

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
