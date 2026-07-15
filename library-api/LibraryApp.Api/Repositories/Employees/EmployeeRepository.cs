using LibraryApp.Api.Data;
using LibraryApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Repositories.Employees;

public sealed class EmployeeRepository(LibraryDbContext db) : IEmployeeRepository
{
    public async Task<IReadOnlyList<Employee>> GetEmployeesAsync(CancellationToken cancellationToken) =>
        await db.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.FirstName)
            .ThenBy(employee => employee.LastName)
            .ThenBy(employee => employee.TeamName)
            .ToListAsync(cancellationToken);

    public Task<Employee?> GetEmployeeAsync(int id, CancellationToken cancellationToken) =>
        db.Employees.FirstOrDefaultAsync(employee => employee.Id == id, cancellationToken);

    public Task<Employee?> GetEmployeeAsync(
        string firstName,
        string lastName,
        string teamName,
        CancellationToken cancellationToken)
    {
        var normalizedFirstName = firstName.Trim();
        var normalizedLastName = lastName.Trim();
        var normalizedTeamName = teamName.Trim();

        return db.Employees.FirstOrDefaultAsync(employee =>
            employee.FirstName.Equals(normalizedFirstName, StringComparison.CurrentCultureIgnoreCase) &&
            employee.LastName.Equals(normalizedLastName, StringComparison.CurrentCultureIgnoreCase) &&
            employee.TeamName.Equals(normalizedTeamName, StringComparison.CurrentCultureIgnoreCase),
            cancellationToken);
    }

    public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken) =>
        await db.Employees.AddAsync(employee, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        db.SaveChangesAsync(cancellationToken);
}
