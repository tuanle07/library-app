using LibraryApp.Api.Models;

namespace LibraryApp.Api.Repositories.Books;

public interface IBookRepository
{
    Task<PagedResult<Book>> GetBooksAsync(
        string? search,
        BookStatus? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<Book?> GetBookAsync(int id, CancellationToken cancellationToken);

    Task<Book?> GetTrackedBookAsync(int id, CancellationToken cancellationToken);

    Task AddBookAsync(Book book, CancellationToken cancellationToken);

    void DeleteBook(Book book);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount);
