using LibraryApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Data;

public sealed class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(employee => employee.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(employee => employee.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(employee => employee.TeamName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(employee => new
                {
                    employee.FirstName,
                    employee.LastName,
                    employee.TeamName
                })
                .IsUnique();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(book => book.Author)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasOne(book => book.OwnerEmployee)
                .WithMany(employee => employee.OwnedBooks)
                .HasForeignKey(book => book.OwnerEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(book => book.BorrowerEmployee)
                .WithMany(employee => employee.BorrowedBooks)
                .HasForeignKey(book => book.BorrowerEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
