using System.ComponentModel.DataAnnotations;

public record CrearCartaDto(
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