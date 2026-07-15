using FluentAssertions;
using LibraryApp.Api.Contracts;
using LibraryApp.Api.Exceptions;
using LibraryApp.Api.Models;
using LibraryApp.Api.Repositories.Books;
using LibraryApp.Api.Services.Books;
using LibraryApp.Api.Services.Employees;
using Moq;

namespace LibraryApp.Test;

public sealed class BookServiceTests
{
    [Fact]
    public async Task GetBooksAsync_NormalizesPagingAndCalculatesTotalPages()
    {
        var books = new[]
        {
            CreateBook(1, "Clean Code"),
            CreateBook(2, "The Pragmatic Programmer")
        };
        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.GetBooksAsync(
                "code",
                BookStatus.Available,
                1,
                100,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Book>(books, TotalCount: 250));
        var service = new BookService(repository.Object, Mock.Of<IEmployeeService>());

        var response = await service.GetBooksAsync(
            search: "code",
            status: BookStatus.Available,
            page: 0,
            pageSize: 200,
            CancellationToken.None);

        response.Should().BeEquivalentTo(new
        {
            Page = 1,
            PageSize = 100,
            TotalCount = 250,
            TotalPages = 3
        });
        response.Items.Should().HaveCount(2);
        repository.VerifyAll();
    }

    [Fact]
    public async Task CreateBookAsync_TrimsFieldsAndNormalizesEmptyOptionalValues()
    {
        var owner = CreateEmployee(5, "Grace", "Hopper", "Platform");
        Book? savedBook = null;
        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.AddBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .Callback<Book, CancellationToken>((book, _) =>
            {
                book.Id = 12;
                savedBook = book;
            })
            .Returns(Task.CompletedTask);
        repository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var employeeService = new Mock<IEmployeeService>();
        employeeService
            .Setup(x => x.GetEmployeeAsync(owner.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner);
        var service = new BookService(repository.Object, employeeService.Object);

        var response = await service.CreateBookAsync(
            new CreateBookRequest(
                "  Refactoring  ",
                "  Martin Fowler  ",
                "   ",
                owner.Id,
                "  Keep near desk  "),
            CancellationToken.None);

        response.Should().BeEquivalentTo(new
        {
            Id = 12,
            Title = "Refactoring",
            Author = "Martin Fowler",
            Isbn = (string?)null,
            Notes = "Keep near desk",
            OwnerId = owner.Id
        });
        savedBook.Should().BeEquivalentTo(new
        {
            Title = "Refactoring",
            Author = "Martin Fowler",
            Isbn = (string?)null,
            OwnerEmployeeId = owner.Id,
            Notes = "Keep near desk"
        });
        repository.Verify(x => x.AddBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task BorrowBookAsync_WhenBookIsAvailable_AssignsBorrowerAndBorrowedDate()
    {
        var borrower = CreateEmployee(8, "Katherine", "Johnson", "Research");
        var book = CreateBook(3, "Domain-Driven Design");
        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.GetTrackedBookAsync(book.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        repository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var employeeService = new Mock<IEmployeeService>();
        employeeService
            .Setup(x => x.GetEmployeeAsync(borrower.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(borrower);
        var service = new BookService(repository.Object, employeeService.Object);
        var earliestBorrowDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var response = await service.BorrowBookAsync(
            book.Id,
            new BorrowBookRequest(borrower.Id),
            CancellationToken.None);

        var latestBorrowDate = DateOnly.FromDateTime(DateTime.UtcNow);
        response.Status.Should().Be(BookStatus.Borrowed);
        response.BorrowerId.Should().Be(borrower.Id);
        response.BorrowedOn.Should().NotBeNull();
        response.BorrowedOn!.Value.CompareTo(earliestBorrowDate).Should().BeGreaterThanOrEqualTo(0);
        response.BorrowedOn.Value.CompareTo(latestBorrowDate).Should().BeLessThanOrEqualTo(0);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task BorrowBookAsync_WhenBookIsAlreadyBorrowed_ThrowsInvalidRequest()
    {
        var book = CreateBook(3, "Domain-Driven Design");
        book.Status = BookStatus.Borrowed;
        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.GetTrackedBookAsync(book.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        var service = new BookService(repository.Object, Mock.Of<IEmployeeService>());

        Func<Task> act = () => service.BorrowBookAsync(
            book.Id,
            new BorrowBookRequest(8),
            CancellationToken.None);

        (await act.Should().ThrowAsync<InvalidRequestException>())
            .WithMessage("Only available books can be borrowed.");
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateBookAsync_WhenBorrowedWithoutBorrower_ThrowsInvalidRequest()
    {
        var owner = CreateEmployee(5, "Grace", "Hopper", "Platform");
        var book = CreateBook(3, "Domain-Driven Design", owner);
        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.GetTrackedBookAsync(book.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        var employeeService = new Mock<IEmployeeService>();
        employeeService
            .Setup(x => x.GetEmployeeAsync(owner.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner);
        var service = new BookService(repository.Object, employeeService.Object);

        Func<Task> act = () => service.UpdateBookAsync(
            book.Id,
            new UpdateBookRequest(
                "Domain-Driven Design",
                "Eric Evans",
                null,
                owner.Id,
                BookStatus.Borrowed,
                BorrowerId: null,
                BorrowedOn: DateOnly.FromDateTime(DateTime.UtcNow),
                Notes: null),
            CancellationToken.None);

        (await act.Should().ThrowAsync<InvalidRequestException>())
            .WithMessage("Borrower is required when a book is borrowed.");
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ReturnBookAsync_ClearsBorrowerState()
    {
        var owner = CreateEmployee(5, "Grace", "Hopper", "Platform");
        var borrower = CreateEmployee(8, "Katherine", "Johnson", "Research");
        var book = CreateBook(3, "Domain-Driven Design", owner);
        book.Status = BookStatus.Borrowed;
        book.BorrowerEmployeeId = borrower.Id;
        book.BorrowerEmployee = borrower;
        book.BorrowedOn = new DateOnly(2026, 7, 15);

        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.GetTrackedBookAsync(book.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);
        repository
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var service = new BookService(repository.Object, Mock.Of<IEmployeeService>());

        var response = await service.ReturnBookAsync(book.Id, CancellationToken.None);

        response.Status.Should().Be(BookStatus.Available);
        response.BorrowerId.Should().BeNull();
        response.BorrowedOn.Should().BeNull();
        book.BorrowerEmployee.Should().BeNull();
        book.BorrowerEmployeeId.Should().BeNull();
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteBookAsync_WhenBookDoesNotExist_ThrowsNotFound()
    {
        var repository = new Mock<IBookRepository>();
        repository
            .Setup(x => x.GetTrackedBookAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);
        var service = new BookService(repository.Object, Mock.Of<IEmployeeService>());

        Func<Task> act = () => service.DeleteBookAsync(99, CancellationToken.None);

        (await act.Should().ThrowAsync<NotFoundException>())
            .WithMessage("Book was not found.");
        repository.Verify(x => x.DeleteBook(It.IsAny<Book>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static Employee CreateEmployee(
        int id,
        string firstName,
        string lastName,
        string teamName) =>
        new()
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            TeamName = teamName
        };

    private static Book CreateBook(int id, string title, Employee? owner = null)
    {
        owner ??= CreateEmployee(1, "Ada", "Lovelace", "Engineering");

        return new Book
        {
            Id = id,
            Title = title,
            Author = "Test Author",
            OwnerEmployeeId = owner.Id,
            OwnerEmployee = owner
        };
    }
}
