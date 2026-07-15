using LibraryApp.Api.Contracts;
using LibraryApp.Api.Models;

namespace LibraryApp.Api.Services.Books;

public interface IBookService
{
    Task<PagedResponse<BookResponse>> GetBooksAsync(
        string? search,
        BookStatus? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<BookResponse> GetBookAsync(int id, CancellationToken cancellationToken);

    Task<BookResponse> CreateBookAsync(CreateBookRequest request, CancellationToken cancellationToken);

    Task<BookResponse> UpdateBookAsync(
        int id,
        UpdateBookRequest request,
        CancellationToken cancellationToken);

    Task<BookResponse> BorrowBookAsync(
        int id,
        BorrowBookRequest request,
        CancellationToken cancellationToken);

    Task<BookResponse> ReturnBookAsync(int id, CancellationToken cancellationToken);

    Task DeleteBookAsync(int id, CancellationToken cancellationToken);
}
