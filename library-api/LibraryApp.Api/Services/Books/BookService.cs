using LibraryApp.Api.Contracts;
using LibraryApp.Api.Exceptions;
using LibraryApp.Api.Models;
using LibraryApp.Api.Repositories.Books;
using LibraryApp.Api.Services.Employees;

namespace LibraryApp.Api.Services.Books;

public sealed class BookService(
    IBookRepository bookRepository,
    IEmployeeService employeeService) : IBookService
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    public async Task<PagedResponse<BookResponse>> GetBooksAsync(
        string? search,
        BookStatus? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1
            ? DefaultPageSize
            : Math.Min(pageSize, MaxPageSize);

        var books = await bookRepository.GetBooksAsync(
            search,
            status,
            normalizedPage,
            normalizedPageSize,
            cancellationToken);

        var items = books.Items.Select(book => book.ToResponse()).ToList();
        var totalPages = books.TotalCount == 0
            ? 0
            : (int)Math.Ceiling((double)books.TotalCount / normalizedPageSize);

        return new PagedResponse<BookResponse>(
            items,
            normalizedPage,
            normalizedPageSize,
            books.TotalCount,
            totalPages);
    }

    public async Task<BookResponse> GetBookAsync(int id, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetBookAsync(id, cancellationToken);
        return book is null
            ? throw new NotFoundException("Book was not found.")
            : book.ToResponse();
    }

    public async Task<BookResponse> CreateBookAsync(
        CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var owner = await employeeService.GetEmployeeAsync(request.OwnerId, cancellationToken);

        var book = new Book
        {
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            Isbn = NormalizeOptional(request.Isbn),
            OwnerEmployeeId = owner.Id,
            OwnerEmployee = owner,
            Notes = NormalizeOptional(request.Notes)
        };

        await bookRepository.AddBookAsync(book, cancellationToken);
        await bookRepository.SaveChangesAsync(cancellationToken);

        return book.ToResponse();
    }

    public async Task<BookResponse> UpdateBookAsync(
        int id,
        UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetTrackedBookAsync(id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException("Book was not found.");
        }

        var owner = await employeeService.GetEmployeeAsync(request.OwnerId, cancellationToken);

        Employee? borrower = null;
        if (request.Status == BookStatus.Borrowed)
        {
            if (request.BorrowerId is null)
            {
                throw new InvalidRequestException("Borrower is required when a book is borrowed.");
            }

            borrower = await employeeService.GetEmployeeAsync(request.BorrowerId.Value, cancellationToken);
        }

        book.Title = request.Title.Trim();
        book.Author = request.Author.Trim();
        book.Isbn = NormalizeOptional(request.Isbn);
        book.OwnerEmployeeId = owner.Id;
        book.OwnerEmployee = owner;
        book.Status = request.Status;
        book.BorrowerEmployeeId = borrower?.Id;
        book.BorrowerEmployee = borrower;
        book.BorrowedOn = request.Status == BookStatus.Borrowed
            ? request.BorrowedOn
            : null;
        book.Notes = NormalizeOptional(request.Notes);

        await bookRepository.SaveChangesAsync(cancellationToken);
        return book.ToResponse();
    }

    public async Task<BookResponse> BorrowBookAsync(
        int id,
        BorrowBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetTrackedBookAsync(id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException("Book was not found.");
        }

        if (book.Status != BookStatus.Available)
        {
            throw new InvalidRequestException("Only available books can be borrowed.");
        }

        var borrower = await employeeService.GetEmployeeAsync(request.BorrowerId, cancellationToken);

        book.Status = BookStatus.Borrowed;
        book.BorrowerEmployeeId = borrower.Id;
        book.BorrowerEmployee = borrower;
        book.BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow);

        await bookRepository.SaveChangesAsync(cancellationToken);
        return book.ToResponse();
    }

    public async Task<BookResponse> ReturnBookAsync(int id, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetTrackedBookAsync(id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException("Book was not found.");
        }

        book.Status = BookStatus.Available;
        book.BorrowerEmployee = null;
        book.BorrowerEmployeeId = null;
        book.BorrowedOn = null;

        await bookRepository.SaveChangesAsync(cancellationToken);
        return book.ToResponse();
    }

    public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetTrackedBookAsync(id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException("Book was not found.");
        }

        bookRepository.DeleteBook(book);
        await bookRepository.SaveChangesAsync(cancellationToken);
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
