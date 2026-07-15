using LibraryApp.Api.Contracts;
using LibraryApp.Api.Exceptions;
using LibraryApp.Api.Models;
using LibraryApp.Api.Repositories.Employees;

namespace LibraryApp.Api.Services.Employees;

public sealed class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task<IReadOnlyList<EmployeeResponse>> GetEmployeesAsync(CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetEmployeesAsync(cancellationToken);
        return employees.Select(ToResponse).ToList();
    }

    public async Task<Employee> GetEmployeeAsync(int id, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEmployeeAsync(id, cancellationToken);
        return employee is null
            ? throw new NotFoundException("Employee was not found.")
            : employee;
    }

    public async Task<EmployeeResponse> CreateEmployeeAsync(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedFirstName = request.FirstName.Trim();
        var normalizedLastName = request.LastName.Trim();
        var normalizedTeamName = request.TeamName.Trim();

        var employee = await employeeRepository.GetEmployeeAsync(
            normalizedFirstName,
            normalizedLastName,
            normalizedTeamName,
            cancellationToken);

        if (employee is not null)
        {
            return ToResponse(employee);
        }

        employee = new Employee
        {
            FirstName = normalizedFirstName,
            LastName = normalizedLastName,
            TeamName = normalizedTeamName
        };

        await employeeRepository.AddEmployeeAsync(employee, cancellationToken);
        await employeeRepository.SaveChangesAsync(cancellationToken);
        return ToResponse(employee);
    }

    private static EmployeeResponse ToResponse(Employee employee) =>
        new(employee.Id, employee.FirstName, employee.LastName, employee.TeamName);
}
