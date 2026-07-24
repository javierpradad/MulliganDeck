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

Dictionary<string, string[]> ValidarCarta(Carta carta)
{
    var errores = new Dictionary<string, string[]>();
    if(string.IsNullOrWhiteSpace(carta.Nombre)){
        errores.Add("Nombre", new[] { "El nombre de la carta no puede estar vacío." });
    }
    if(carta.CosteMana < 0 || carta.CosteMana > 20){
        errores.Add("CosteMana", new[] { "El coste de maná debe estar entre 0 y 20." });
    }
    if(carta.Ataque < 0 || carta.Ataque > 20){
        errores.Add("Ataque", new[] { "El ataque debe estar entre 0 y 20." });
    }

    if(carta.Vida < 0 || carta.Vida > 20){
        errores.Add("Vida", new[] { "La vida debe estar entre 0 y 20." });
    }

    return errores;
}

//GET
app.MapGet("/cartas", () =>
{
    return listaCartas;
})
.WithName("GetCartas");

app.MapGet("/cartas/{id}", (int id) =>
{
    var carta = listaCartas.FirstOrDefault(c => c.Id == id);

    if (carta == null){
        return Results.NotFound();
    }
        
    return Results.Ok(carta);
})
.WithName("GetCartaPorId");

//POST
app.MapPost("/cartas", (Carta carta) =>
{   
    var errores = ValidarCarta(carta);
    if (errores.Any()){
        return Results.ValidationProblem(errores);
    }

    var maxID = 0;
    if (listaCartas.Any()){
        maxID = listaCartas.Max(c => c.Id);
    }
    carta = carta with { Id = maxID + 1 };
    listaCartas.Add(carta);
    return Results.Created($"/cartas/{carta.Id}", carta);
})
.WithName("PostCarta");

//PUT
app.MapPut("/cartas/{id}", (int id, Carta carta) =>
{
    var index = listaCartas.FindIndex(c => c.Id == id);
    if (index == -1){
        return Results.NotFound();
    }

    var errores = ValidarCarta(carta);
    if (errores.Any()){
        return Results.ValidationProblem(errores);
    }

    carta = carta with { Id = id };
    listaCartas[index] = carta;
    return Results.Ok(carta);
})
.WithName("PutCarta");

//DELETE
app.MapDelete("/cartas/{id}", (int id) =>
{
    var carta = listaCartas.FirstOrDefault(c => c.Id == id);
    if (carta == null){
        return Results.NotFound();
    }

    listaCartas.Remove(carta);
    return Results.NoContent();
})
.WithName("DeleteCarta");

app.Run();

record Carta(int Id, string Nombre, int CosteMana, string Color, int Ataque, int Vida);
