using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hdbs.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IConfiguration _configuration;

        public EmployeeService(UserManager<Employee> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeCommand command)
        {
            if(command.Password != command.RePassword)
            {
                throw new CustomException(CustomErrorCode.PasswordsNotMatch, "Unable to create employee - passwords not match");
            }

            var employee = new Employee
            {
                IsDisabled = false,
                UserName = command.Name,
                Surname = command.Surname,
                Email = command.Email,
                Permissions = UserPermissions.SimpleView
            };

            var result = await _userManager.CreateAsync(employee, command.Password);

            if(result.Succeeded == false)
            {
                throw new CustomException(CustomErrorCode.UnableToCreateEmployee, $"Unable to create employee: {result.Errors}");
            }

            var employeeFromDb = await _userManager.FindByIdAsync(employee.Id);

            if (employeeFromDb == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {employee.Id}");
            }

            return new EmployeeDto
            {
                Id = employeeFromDb.Id,
                IsDisabled = employeeFromDb.IsDisabled,
                Name = employeeFromDb.UserName == null ? "" : employeeFromDb.UserName,
                Surname = employeeFromDb.Surname,
                Email = employeeFromDb.Email == null ? "" : employeeFromDb.Email,
                Permissions = employeeFromDb.Permissions.ToString()
            };
        }

        public async Task DeleteAsync(DeleteEmployeeCommand command)
        {
            var employee = await _userManager.FindByIdAsync(command.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.Id}");
            }

            var result = await _userManager.DeleteAsync(employee);

            if(result.Succeeded == false)
            {
                throw new CustomException(CustomErrorCode.UnableToDeleteEmployee, $"Unable to create employee: {result.Errors}");
            }
        }

        public async Task<string> LoginEmployeeAsync(LoginEmployeeCommand command)
        {
            var employee = await _userManager.FindByEmailAsync(command.Email);
            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with email: {command.Email}");
            }

            if (employee.IsDisabled)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with email: {command.Email}");
            }

            if (await _userManager.CheckPasswordAsync(employee, command.Password) == false)
            {
                throw new CustomException(CustomErrorCode.WrongPassword, $"Unable to login employee with email: {command.Email} - wrong password");
            }

            var secretKey = _configuration["Jwt:SecretKey"];
            if(secretKey == null)
            {
                throw new CustomException(CustomErrorCode.NoJwtSecretKey, "Unable to get SecretKey");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id),
                new Claim(ClaimTypes.Email, employee.Email == null ? "" : employee.Email),
                new Claim("surname", employee.Surname),
                new Claim("permissions", ((int)employee.Permissions).ToString())
            };

            var issuer = _configuration["Jwt:ValidIssuer"];
            var audience = _configuration["Jwt:ValidAudience"];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SetPermissionsAsync(SetPermissionsForEmployeeCommand command)
        {
            if(command.Id == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, "Unable to find employee with id: null");
            }

            var employee = await _userManager.FindByIdAsync(command.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.Id}");
            }

            UserPermissions userPermissions = new UserPermissions();
            
            if (command.None) userPermissions |= UserPermissions.None;
            if (command.SimpleView) userPermissions |= UserPermissions.SimpleView;
            if (command.AdminView) userPermissions |= UserPermissions.AdminView;

            if (command.CreateEmployee) userPermissions |= UserPermissions.CreateEmployee;
            if (command.UpdateEmployee) userPermissions |= UserPermissions.UpdateEmployee;
            if (command.DeleteEmployee) userPermissions |= UserPermissions.DeleteEmployee;
            
            if (command.CreateLocation) userPermissions |= UserPermissions.CreateLocation;
            if (command.UpdateLocation) userPermissions |= UserPermissions.UpdateLocation;
            if (command.DeleteLocation) userPermissions |= UserPermissions.DeleteLocation;

            if (command.CreateDesk) userPermissions |= UserPermissions.CreateDesk;
            if (command.UpdateDesk) userPermissions |= UserPermissions.UpdateDesk;
            if (command.DeleteDesk) userPermissions |= UserPermissions.DeleteDesk;

            if (command.SetPermissions) userPermissions |= UserPermissions.SetPermissions;
            if (command.SetEmployeeStatus) userPermissions |= UserPermissions.SetEmployeeStatus;

            if (command.CreateReservation) userPermissions |= UserPermissions.CreateReservation;
            if (command.UpdateReservation) userPermissions |= UserPermissions.UpdateReservation;
            if (command.DeleteReservation) userPermissions |= UserPermissions.DeleteReservation;

            if (command.ImportEmployee) userPermissions |= UserPermissions.ImportEmployee;

            employee.Permissions = userPermissions;
            
            var result = await _userManager.UpdateAsync(employee);
            if (result.Succeeded == false)
            {
                throw new CustomException(CustomErrorCode.PermissionError, $"Unable to set permission: {userPermissions} for employee with id: {command.Id}");
            }
        }

        public async Task UpdateAsync(UpdateEmployeeCommand command)
        {
            var employee = await _userManager.FindByIdAsync(command.Id);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.Id}");
            }

            employee.UserName = command.Name == null ? employee.UserName : command.Name;
            employee.Surname = command.Surname == null ? employee.Surname : command.Surname;
            employee.Email = command.Email == null ? employee.Email : command.Email;

            if((command.NewPassword != null && command.RePassword == null) || (command.RePassword != null && command.NewPassword == null))
            {
                throw new CustomException(CustomErrorCode.UnableToUpdatePassword, $"Password or RePassword was missing or not match");
            }

            if(command.NewPassword != null)
            {
                if(command.NewPassword != command.RePassword)
                {
                    throw new CustomException(CustomErrorCode.UnableToUpdatePassword, $"Password or RePassword was missing or not match");
                }

                employee.PasswordHash = _userManager.PasswordHasher.HashPassword(employee, command.NewPassword);
            }

            var result = await _userManager.UpdateAsync(employee);
            if(result.Succeeded == false)
            {
                throw new CustomException(CustomErrorCode.UnableToUpdateEmployee, $"Unable to update employee: {result.Errors}");
            }
        }
        
        public async Task SetStatusAsync(SetStatusForEmployeeCommand command)
        {
            if(command.EmployeeId == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, "Unable to find employee with id: null");
            }

            var employee = await _userManager.FindByIdAsync(command.EmployeeId);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.EmployeeId}");
            }

            employee.IsDisabled = command.IsDisabled;

            var result = await _userManager.UpdateAsync(employee);
            if (result.Succeeded == false)
            {
                throw new CustomException(CustomErrorCode.UnableToUpdateEmployee, $"Unable to update employee: {result.Errors}");
            }
        }

        public async Task UpdateMyAsync(UpdateMyEmployeeCommand command)
        {
            if (command.EmployeeId == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeIdIsNull, "Unable to find employee with id: null");
            }

            var employee = await _userManager.FindByIdAsync(command.EmployeeId);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.EmployeeId}");
            }

            if (await _userManager.CheckPasswordAsync(employee, command.Password) == false)
            {
                throw new CustomException(CustomErrorCode.WrongPassword, $"Unable to update employee with id: {command.EmployeeId} - wrong password");
            }

            employee.UserName = command.Name == null ? employee.UserName : command.Name;
            employee.Surname = command.Surname == null ? employee.Surname : command.Surname;
            employee.Email = command.Email == null ? employee.Email : command.Email;

            if ((command.NewPassword != null && command.RePassword == null) || (command.RePassword != null && command.NewPassword == null))
            {
                throw new CustomException(CustomErrorCode.UnableToUpdatePassword, $"Password or RePassword was missing or not match");
            }

            if (command.NewPassword != null)
            {
                if (command.NewPassword != command.RePassword)
                {
                    throw new CustomException(CustomErrorCode.UnableToUpdatePassword, $"Password or RePassword was missing or not match");
                }

                employee.PasswordHash = _userManager.PasswordHasher.HashPassword(employee, command.NewPassword);
            }

            var result = await _userManager.UpdateAsync(employee);
            if (result.Succeeded == false)
            {
                throw new CustomException(CustomErrorCode.UnableToUpdateEmployee, $"Unable to update employee: {result.Errors}");
            }
        }

        public async Task<ImportExcelEmployeeResponse> ImportExcelEmployeeAsync(ImportExcelEmployeeCommand command)
        {
            if (command.File == null || command.File.Length == 0)
            {
                throw new CustomException(CustomErrorCode.NoFileUploaded, "No file uploaded");
            }

            var fileExtension = Path.GetExtension(command.File.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                throw new CustomException(CustomErrorCode.InvalidFileFormat, "The uploaded file is not a valid Excel file");
            }

            var response = new ImportExcelEmployeeResponse();

            using (var stream = new MemoryStream())
            {
                await command.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    var expectedHeaders = new[] { "IsDisabled", "Name", "Surname", "Email", "Password", "Permissions" };
                    for (int i = 0; i < expectedHeaders.Length; i++)
                    {
                        if (worksheet.Cells[1, i + 1].Text != expectedHeaders[i])
                        {
                            throw new CustomException(CustomErrorCode.InvalidTemplateFormat, "The uploaded Excel file does not have the correct template format");
                        }
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            response.TotalNumber++;

                            var isDisabled = bool.Parse(worksheet.Cells[row, 1].Text);
                            var name = worksheet.Cells[row, 2].Text;
                            var surname = worksheet.Cells[row, 3].Text;
                            var email = worksheet.Cells[row, 4].Text;
                            var password = worksheet.Cells[row, 5].Text;
                            var permissions = Enum.Parse<UserPermissions>(worksheet.Cells[row, 6].Text);

                            if(await _userManager.FindByEmailAsync(email) != null)
                            {
                                response.SkippedNumber++;
                                continue;
                            }

                            var employee = new Employee
                            {
                                IsDisabled = isDisabled,
                                UserName = name,
                                Surname = surname,
                                Email = email,
                                Permissions = permissions
                            };
                        
                            var userResponse = await _userManager.CreateAsync(employee, password);
                            if(userResponse.Succeeded == false)
                            {
                                throw new CustomException(CustomErrorCode.UnableToCreateEmployee, $"Unable to create employee: {userResponse.Errors}");
                            }

                            response.SuccesedNumber++;
                        }
                        catch (Exception ex)
                        {
                            response.ErrorsNumber++;
                            response.Errors.Add($"Row {row}: {ex.Message}");
                        }

                    }
                }
            }

            return response;
        }
    }
}
