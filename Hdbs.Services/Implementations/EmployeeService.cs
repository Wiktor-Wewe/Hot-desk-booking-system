using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Locations.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HdbsContext _dbContext;

        public EmployeeService(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeCommand command)
        {
            var employee = new Employee
            {
                Name = command.Name
            };

            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveOrHandleExceptionAsync();

            var employeeFromDb = await _dbContext.Employees
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(p => p.Id == employee.Id);

            if (employeeFromDb == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {employee.Id}");
            }

            return new EmployeeDto
            {
                Id = employeeFromDb.Id,
                Name = employeeFromDb.Name,
                Reservations = employeeFromDb.Reservations
            };
        }

        public async Task DeleteAsync(DeleteEmployeeCommand command)
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(p => p.Id == command.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.Id}");
            }


            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveOrHandleExceptionAsync();
        }

        public async Task UpdateAsync(UpdateEmployeeCommand command)
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(p => p.Id == command.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.Id}");
            }

            employee.Name = command.Name;

            await _dbContext.SaveOrHandleExceptionAsync();
        }
    }
}
