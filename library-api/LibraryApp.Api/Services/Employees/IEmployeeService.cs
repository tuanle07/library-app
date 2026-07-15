using LibraryApp.Api.Contracts;
using LibraryApp.Api.Models;

namespace LibraryApp.Api.Services.Employees;

public interface IEmployeeService
{
    Task<IReadOnlyList<EmployeeResponse>> GetEmployeesAsync(CancellationToken cancellationToken);

    Task<Employee> GetEmployeeAsync(int id, CancellationToken cancellationToken);

    Task<EmployeeResponse> CreateEmployeeAsync(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken);
}
