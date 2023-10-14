using LibraryManagement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("product"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/books", async (ApplicationDbContext db) =>
    await db.Books.Take(20).ToListAsync());

app.MapGet("/api/books/{id}", async (ApplicationDbContext db, int id) =>
    await db.Books.FindAsync(id)
        is Book todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/api/books", async (ApplicationDbContext db, Book book) =>
{
    db.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{book.Isbn}", book);

});

app.MapDelete("/api/books/{id}", async (ApplicationDbContext db, int id) =>
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();

class ApiReponse
{
    public bool success { get; set; }
    public string message { get; set; } = string.Empty;
    public object? result { get; set; }
}