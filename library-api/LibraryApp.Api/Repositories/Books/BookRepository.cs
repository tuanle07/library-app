using LibraryApp.Api.Data;
using LibraryApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Repositories.Books;

public sealed class BookRepository(LibraryDbContext db) : IBookRepository
{
    public async Task<PagedResult<Book>> GetBooksAsync(
        string? search,
        BookStatus? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = db.Books
            .Include(book => book.OwnerEmployee)
            .Include(book => book.BorrowerEmployee)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(book =>
                book.Title.ToLower().Contains(term) ||
                book.Author.ToLower().Contains(term) ||
                book.OwnerEmployee.FirstName.ToLower().Contains(term) ||
                book.OwnerEmployee.LastName.ToLower().Contains(term) ||
                book.OwnerEmployee.TeamName.ToLower().Contains(term));
        }

        if (status is not null)
        {
            query = query.Where(book => book.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(book => book.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Book>(items, totalCount);
    }

    public Task<Book?> GetBookAsync(int id, CancellationToken cancellationToken) =>
        db.Books
            .Include(book => book.OwnerEmployee)
            .Include(book => book.BorrowerEmployee)
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);

    public async Task<Book?> GetTrackedBookAsync(int id, CancellationToken cancellationToken) =>
        await db.Books
            .Include(book => book.OwnerEmployee)
            .Include(book => book.BorrowerEmployee)
            .FirstOrDefaultAsync(book => book.Id == id, cancellationToken);

    public async Task AddBookAsync(Book book, CancellationToken cancellationToken) =>
        await db.Books.AddAsync(book, cancellationToken);

    public void DeleteBook(Book book) =>
        db.Books.Remove(book);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        db.SaveChangesAsync(cancellationToken);
}
