using LibraryApp.Api.Data;
using LibraryApp.Api.Middlewares;
using LibraryApp.Api.Repositories.Books;
using LibraryApp.Api.Repositories.Employees;
using LibraryApp.Api.Services.Books;
using LibraryApp.Api.Services.Employees;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseInMemoryDatabase("CrumbToCrumbLibraryDB"));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    await SeedData.InitialiseAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAppExceptionHandler();

app.UseCors("Frontend");

app.MapControllers();

app.Run();
