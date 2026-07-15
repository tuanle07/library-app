using LibraryApp.Api.Models;

namespace LibraryApp.Api.Contracts;

public sealed record BookResponse(
    int Id,
    string Title,
    string Author,
    string? Isbn,
    int OwnerId,
    string OwnerFirstName,
    string OwnerLastName,
    string OwnerTeam,
    BookStatus Status,
    int? BorrowerId,
    string? BorrowerFirstName,
    string? BorrowerLastName,
    string? BorrowerTeam,
    DateOnly? BorrowedOn,
    string? Notes);

public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record CreateBookRequest(
    string Title,
    string Author,
    string? Isbn,
    int OwnerId,
    string? Notes);

public sealed record UpdateBookRequest(
    string Title,
    string Author,
    string? Isbn,
    int OwnerId,
    BookStatus Status,
    int? BorrowerId,
    DateOnly? BorrowedOn,
    string? Notes);

public sealed record BorrowBookRequest(int BorrowerId);
