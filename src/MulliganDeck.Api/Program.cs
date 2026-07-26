using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<CartaRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<MulliganDeckContext>(options =>
    options.UseSqlite("Data Source=mulligandeck.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public record Carta(
    int Id,

    [Required(ErrorMessage = "El nombre de la carta no puede estar vacío.")]
    string Nombre,

    [Range(0, 20, ErrorMessage = "El coste de maná debe estar entre 0 y 20.")]
    int CosteMana,

    string Color,

    [Range(0, 20, ErrorMessage = "El ataque debe estar entre 0 y 20.")]
    int Ataque,

    [Range(0, 20, ErrorMessage = "La vida debe estar entre 0 y 20.")]
    int Vida
);