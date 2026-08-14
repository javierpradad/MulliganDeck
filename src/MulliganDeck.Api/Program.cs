using Microsoft.EntityFrameworkCore;
using MulliganDeck.Infrastructure;
using MulliganDeck.Infrastructure.Scryfall;
using MulliganDeck.Api;
using MulliganDeck.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddControllers();
builder.Services.AddDbContext<MulliganDeckContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient("Scryfall", client =>
{
    client.BaseAddress = new Uri("https://api.scryfall.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "MulliganDeck/1.0");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddStandardResilienceHandler();
builder.Services.AddScoped<ScryfallClient>();
builder.Services.AddScoped<ScryfallMapper>();
builder.Services.AddScoped<ScryfallImporter>();
builder.Services.AddHostedService<ScryfallSyncWorker>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MulliganDeckContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }