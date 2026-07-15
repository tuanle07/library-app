namespace LibraryApp.Api.Models;

public sealed class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? Isbn { get; set; }
    public int OwnerEmployeeId { get; set; }
    public required Employee OwnerEmployee { get; set; }
    public BookStatus Status { get; set; } = BookStatus.Available;
    public int? BorrowerEmployeeId { get; set; }
    public Employee? BorrowerEmployee { get; set; }
    public DateOnly? BorrowedOn { get; set; }
    public string? Notes { get; set; }
}
