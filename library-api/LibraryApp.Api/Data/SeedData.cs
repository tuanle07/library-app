using LibraryApp.Api.Models;

namespace LibraryApp.Api.Data;

public static class SeedData
{
    public static async Task InitialiseAsync(LibraryDbContext db)
    {
        if (db.Books.Any())
        {
            return;
        }

        var ava = new Employee { FirstName = "Ava", LastName = "Patel", TeamName = "Platform" };
        var noah = new Employee { FirstName = "Noah", LastName = "Kim", TeamName = "Product Engineering" };
        var sofia = new Employee { FirstName = "Sofia", LastName = "Garcia", TeamName = "Enablement" };
        var mia = new Employee { FirstName = "Mia", LastName = "Chen", TeamName = "Platform" };
        var liam = new Employee { FirstName = "Liam", LastName = "Nguyen", TeamName = "Data" };
        var zoe = new Employee { FirstName = "Zoe", LastName = "Wilson", TeamName = "Design Systems" };

        db.Employees.AddRange(ava, noah, sofia, mia, liam, zoe);

        db.Books.AddRange(
            new Book
            {
                Title = "Domain-Driven Design",
                Author = "Eric Evans",
                Isbn = "9780321125217",
                OwnerEmployee = ava,
                Status = BookStatus.Available,
                Notes = "Happy to lend for a couple of weeks."
            },
            new Book
            {
                Title = "Designing Data-Intensive Applications",
                Author = "Martin Kleppmann",
                Isbn = "9781449373320",
                OwnerEmployee = noah,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = mia,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                Notes = "Popular pick for architecture chats."
            },
            new Book
            {
                Title = "Refactoring",
                Author = "Martin Fowler",
                Isbn = "9780134757599",
                OwnerEmployee = sofia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "The Pragmatic Programmer",
                Author = "David Thomas and Andrew Hunt",
                Isbn = "9780135957059",
                OwnerEmployee = mia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Isbn = "9780132350884",
                OwnerEmployee = liam,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = noah,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12)),
                Notes = "Use as a discussion starter, not a rulebook."
            },
            new Book
            {
                Title = "Working Effectively with Legacy Code",
                Author = "Michael C. Feathers",
                Isbn = "9780131177055",
                OwnerEmployee = zoe,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Patterns of Enterprise Application Architecture",
                Author = "Martin Fowler",
                Isbn = "9780321127426",
                OwnerEmployee = ava,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Building Microservices",
                Author = "Sam Newman",
                Isbn = "9781492034025",
                OwnerEmployee = noah,
                Status = BookStatus.Available,
                Notes = "Temporarily reserved for onboarding."
            },
            new Book
            {
                Title = "Release It!",
                Author = "Michael T. Nygard",
                Isbn = "9781680502398",
                OwnerEmployee = sofia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Accelerate",
                Author = "Nicole Forsgren, Jez Humble, and Gene Kim",
                Isbn = "9781942788331",
                OwnerEmployee = mia,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = ava,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            },
            new Book
            {
                Title = "Staff Engineer",
                Author = "Will Larson",
                Isbn = "9781736417911",
                OwnerEmployee = liam,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "An Elegant Puzzle",
                Author = "Will Larson",
                Isbn = "9781732265189",
                OwnerEmployee = zoe,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Team Topologies",
                Author = "Matthew Skelton and Manuel Pais",
                Isbn = "9781942788812",
                OwnerEmployee = ava,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = sofia,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            },
            new Book
            {
                Title = "The Manager's Path",
                Author = "Camille Fournier",
                Isbn = "9781491973899",
                OwnerEmployee = noah,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Observability Engineering",
                Author = "Charity Majors, Liz Fong-Jones, and George Miranda",
                Isbn = "9781492076445",
                OwnerEmployee = sofia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Fundamentals of Software Architecture",
                Author = "Mark Richards and Neal Ford",
                Isbn = "9781492043454",
                OwnerEmployee = mia,
                Status = BookStatus.Available,
                Notes = "Missing dust jacket."
            },
            new Book
            {
                Title = "Software Architecture: The Hard Parts",
                Author = "Neal Ford, Mark Richards, Pramod Sadalage, and Zhamak Dehghani",
                Isbn = "9781492086895",
                OwnerEmployee = liam,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Continuous Delivery",
                Author = "Jez Humble and David Farley",
                Isbn = "9780321601919",
                OwnerEmployee = zoe,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "The Phoenix Project",
                Author = "Gene Kim, Kevin Behr, and George Spafford",
                Isbn = "9780988262591",
                OwnerEmployee = ava,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = liam,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            },
            new Book
            {
                Title = "The DevOps Handbook",
                Author = "Gene Kim, Jez Humble, Patrick Debois, and John Willis",
                Isbn = "9781942788003",
                OwnerEmployee = noah,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Site Reliability Engineering",
                Author = "Betsy Beyer, Chris Jones, Jennifer Petoff, and Niall Richard Murphy",
                Isbn = "9781491929124",
                OwnerEmployee = sofia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Database Internals",
                Author = "Alex Petrov",
                Isbn = "9781492040347",
                OwnerEmployee = mia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Designing Machine Learning Systems",
                Author = "Chip Huyen",
                Isbn = "9781098107963",
                OwnerEmployee = liam,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = zoe,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            },
            new Book
            {
                Title = "A Philosophy of Software Design",
                Author = "John Ousterhout",
                Isbn = "9781732102200",
                OwnerEmployee = zoe,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Code Complete",
                Author = "Steve McConnell",
                Isbn = "9780735619678",
                OwnerEmployee = ava,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Peopleware",
                Author = "Tom DeMarco and Timothy Lister",
                Isbn = "9780321934116",
                OwnerEmployee = noah,
                Status = BookStatus.Available,
                Notes = "Being rebound."
            },
            new Book
            {
                Title = "The Mythical Man-Month",
                Author = "Frederick P. Brooks Jr.",
                Isbn = "9780201835953",
                OwnerEmployee = sofia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Don't Make Me Think",
                Author = "Steve Krug",
                Isbn = "9780321965516",
                OwnerEmployee = mia,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Inspired",
                Author = "Marty Cagan",
                Isbn = "9781119387503",
                OwnerEmployee = liam,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "Shape Up",
                Author = "Ryan Singer",
                Isbn = "9781734339102",
                OwnerEmployee = zoe,
                Status = BookStatus.Available
            },
            new Book
            {
                Title = "User Story Mapping",
                Author = "Jeff Patton",
                Isbn = "9781491904909",
                OwnerEmployee = ava,
                Status = BookStatus.Borrowed,
                BorrowerEmployee = mia,
                BorrowedOn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            },
            new Book
            {
                Title = "Lean UX",
                Author = "Jeff Gothelf and Josh Seiden",
                Isbn = "9781491953600",
                OwnerEmployee = noah,
                Status = BookStatus.Available
            });

        await db.SaveChangesAsync();
    }
}
