using LibraryApp.Api.Contracts;
using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PagedResponse<BookResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<BookResponse>>> GetBooks(
        [FromQuery] string? search,
        [FromQuery] BookStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var books = await bookService.GetBooksAsync(search, status, page, pageSize, cancellationToken);
        return Ok(books);
    }

    [HttpGet("{id:int}", Name = nameof(GetBook))]
    [ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> GetBook(int id, CancellationToken cancellationToken)
    {
        var book = await bookService.GetBookAsync(id, cancellationToken);
        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType<BookResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<BookResponse>> CreateBook(
        CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await bookService.CreateBookAsync(request, cancellationToken);
        return CreatedAtRoute(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> UpdateBook(
        int id,
        UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await bookService.UpdateBookAsync(id, request, cancellationToken);
        return Ok(book);
    }

    [HttpPost("{id:int}/borrow")]
    [ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> BorrowBook(
        int id,
        BorrowBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await bookService.BorrowBookAsync(id, request, cancellationToken);
        return Ok(book);
    }

    [HttpPost("{id:int}/return")]
    [ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> ReturnBook(int id, CancellationToken cancellationToken)
    {
        var book = await bookService.ReturnBookAsync(id, cancellationToken);
        return Ok(book);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken)
    {
        await bookService.DeleteBookAsync(id, cancellationToken);
        return NoContent();
    }
}
