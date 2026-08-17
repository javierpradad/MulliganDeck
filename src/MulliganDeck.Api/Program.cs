using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;
using MulliganDeck.Infrastructure;
using MulliganDeck.Infrastructure.Scryfall;
using MulliganDeck.Api;
using MulliganDeck.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.RateLimiting;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddOpenApi();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
    });

builder.Services.AddHealthChecks()
    .AddDbContextCheck<MulliganDeckContext>();
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MulliganDeckContext>();

    db.Database.Migrate();

    var hayAdmin = db.Users.Any(u => u.Role == "Admin");
    if (!hayAdmin)
    {
        var email = builder.Configuration["AdminSeed:Email"]!;
        var password = builder.Configuration["AdminSeed:Password"]!;

        db.Users.Add(new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Admin"
        });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }