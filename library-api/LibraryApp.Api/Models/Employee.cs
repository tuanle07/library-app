namespace LibraryApp.Api.Models;

public sealed class Employee
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string TeamName { get; set; }
    public ICollection<Book> OwnedBooks { get; set; } = [];
    public ICollection<Book> BorrowedBooks { get; set; } = [];
}
