using FluentAssertions;
using LibraryApp.Api.Contracts;
using LibraryApp.Api.Exceptions;
using LibraryApp.Api.Models;
using LibraryApp.Api.Repositories.Employees;
using LibraryApp.Api.Services.Employees;
using Moq;

namespace LibraryApp.Test;

public sealed class EmployeeServiceTests
{
    [Fact]
    public async Task CreateEmployeeAsync_TrimsFieldsAndSavesNewEmployee()
    {
        Employee? savedEmployee = null;
        var repository = new Mock<IEmployeeRepository>();
        repository
            .Setup(x => x.GetEmployeeAsync(
                "Ada",
                "Lovelace",
                "Engineering",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);
        repository
            .Setup(x => x.AddEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .Callback<Employee, CancellationToken>((employee, _) =>
            {
                employee.Id = 11;
                savedEmployee = employee;
            })
            .Returns(Task.CompletedTask);
        repository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var service = new EmployeeService(repository.Object);

        var response = await service.CreateEmployeeAsync(
            new CreateEmployeeRequest(" Ada ", " Lovelace ", " Engineering "),
            CancellationToken.None);

        response.Should().BeEquivalentTo(new
        {
            Id = 11,
            FirstName = "Ada",
            LastName = "Lovelace",
            TeamName = "Engineering"
        });
        savedEmployee.Should().BeEquivalentTo(new
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            TeamName = "Engineering"
        });
        repository.Verify(x => x.AddEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ReturnsExistingEmployeeWithoutSavingDuplicate()
    {
        var existing = new Employee
        {
            Id = 7,
            FirstName = "Ada",
            LastName = "Lovelace",
            TeamName = "Engineering"
        };
        var repository = new Mock<IEmployeeRepository>();
        repository
            .Setup(x => x.GetEmployeeAsync(
                "Ada",
                "Lovelace",
                "Engineering",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        var service = new EmployeeService(repository.Object);

        var response = await service.CreateEmployeeAsync(
            new CreateEmployeeRequest(" Ada ", " Lovelace ", " Engineering "),
            CancellationToken.None);

        response.Should().BeEquivalentTo(new
        {
            existing.Id,
            existing.FirstName,
            existing.LastName,
            existing.TeamName
        });
        repository.Verify(x => x.AddEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetEmployeeAsync_ThrowsWhenEmployeeDoesNotExist()
    {
        var repository = new Mock<IEmployeeRepository>();
        repository
            .Setup(x => x.GetEmployeeAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);
        var service = new EmployeeService(repository.Object);

        Func<Task> act = () => service.GetEmployeeAsync(42, CancellationToken.None);

        (await act.Should().ThrowAsync<NotFoundException>())
            .WithMessage("Employee was not found.");
    }
}
