using LibraryApp.Api.Models;

namespace LibraryApp.Api.Repositories.Employees;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);

    Task<Employee?> GetEmployeeAsync(int id, CancellationToken cancellationToken);

    Task<Employee?> GetEmployeeAsync(
        string firstName,
        string lastName,
        string teamName,
        CancellationToken cancellationToken);

    Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
