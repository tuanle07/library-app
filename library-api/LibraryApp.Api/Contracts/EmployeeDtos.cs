namespace LibraryApp.Api.Contracts;

public sealed record EmployeeResponse(
    int Id,
    string FirstName,
    string LastName,
    string TeamName);

public sealed record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    string TeamName);
