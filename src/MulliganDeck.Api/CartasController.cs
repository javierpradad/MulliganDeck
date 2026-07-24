using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cartas")]
public class CartasController : ControllerBase
{   
    private readonly CartaRepository _repo;

    public CartasController(CartaRepository repo)
    {
        _repo = repo;
    }

    //GET
    [HttpGet]
    public ActionResult<List<Carta>> GetCartas(){
        return Ok(_repo.GetCartas());
    }

    [HttpGet("{id}")]
    public ActionResult<Carta> GetCartaPorId(int id){
        var carta = _repo.GetCartaPorId(id);
        if (carta == null){
            return NotFound();
        }
        return Ok(carta);
    }

    //POST
    [HttpPost]
    public ActionResult<Carta> PostCarta(Carta carta){
        carta = _repo.AddCarta(carta);
        return CreatedAtAction(nameof(GetCartaPorId), new { id = carta.Id }, carta);
    }

    //PUT
    [HttpPut("{id}")]
    public ActionResult<Carta> PutCarta(int id, Carta carta){
        if (!_repo.UpdateCarta(id, carta)){
            return NotFound();
        }
        return Ok(carta);
    }

    //DELETE
    [HttpDelete("{id}")]
    public IActionResult DeleteCarta(int id){
        if (!_repo.DeleteCarta(id)){
            return NotFound();
        }
        return NoContent();
    }


}