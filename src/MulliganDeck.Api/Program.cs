var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var carta1 = new Carta(1, "Ureni de lo no escrito", 7, "Rojo, Verde, Azul", 7, 7);
var carta2 = new Carta(2, "Engendro de escarcha engañoso", 2, "Azul", 1, 1);
var carta3 = new Carta(3, "Sirviente de la señora dragon", 2, "Rojo", 1, 3);

List<Carta> listaCartas = new List<Carta> { carta1, carta2, carta3 };

app.MapGet("/cartas", () =>
{
    return listaCartas;
})
.WithName("GetCartas");

app.Run();

record Carta(int Id, string Nombre, int CosteMana, string Color, int Ataque, int Vida);
