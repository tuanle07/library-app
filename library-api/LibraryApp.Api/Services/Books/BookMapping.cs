using LibraryApp.Api.Contracts;
using LibraryApp.Api.Models;

namespace LibraryApp.Api.Services.Books;

internal static class BookMapping
{
    public static BookResponse ToResponse(this Book book) =>
        new(
            book.Id,
            book.Title,
            book.Author,
            book.Isbn,
            book.OwnerEmployeeId,
            book.OwnerEmployee.FirstName,
            book.OwnerEmployee.LastName,
            book.OwnerEmployee.TeamName,
            book.Status,
            book.BorrowerEmployeeId,
            book.BorrowerEmployee?.FirstName,
            book.BorrowerEmployee?.LastName,
            book.BorrowerEmployee?.TeamName,
            book.BorrowedOn,
            book.Notes);
}
