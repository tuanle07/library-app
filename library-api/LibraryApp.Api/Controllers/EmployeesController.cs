using LibraryApp.Api.Contracts;
using LibraryApp.Api.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<EmployeeResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeResponse>>> GetEmployees(
        CancellationToken cancellationToken)
    {
        var employees = await employeeService.GetEmployeesAsync(cancellationToken);
        return Ok(employees);
    }

    [HttpGet("{id:int}", Name = nameof(GetEmployee))]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetEmployee(
        int id,
        CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetEmployeeAsync(id, cancellationToken);
        return Ok(new EmployeeResponse(
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.TeamName));
    }

    [HttpPost]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<EmployeeResponse>> CreateEmployee(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var employee = await employeeService.CreateEmployeeAsync(request, cancellationToken);
        return CreatedAtRoute(nameof(GetEmployee), new { id = employee.Id }, employee);
    }
}
