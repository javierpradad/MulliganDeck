using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;
using MulliganDeck.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddScoped<CartaRepository>();
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
