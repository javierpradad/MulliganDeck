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
    public async Task<ActionResult<List<Carta>>> GetCartas(){
        return Ok(await _repo.GetCartas());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Carta>> GetCartaPorId(int id){
        var carta = await _repo.GetCartaPorId(id);
        if (carta == null){
            return NotFound();
        }
        return Ok(carta);
    }

    //POST
    [HttpPost]
    public async Task<ActionResult<Carta>> PostCarta(Carta carta){
        carta = await _repo.AddCarta(carta);
        return CreatedAtAction(nameof(GetCartaPorId), new { id = carta.Id }, carta);
    }

    //PUT
    [HttpPut("{id}")]
    public async Task<ActionResult<Carta>> PutCarta(int id, Carta carta){
        if (!await _repo.UpdateCarta(id, carta)){
            return NotFound();
        }
        return Ok(carta);
    }

    //DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCarta(int id){
        if (!await _repo.DeleteCarta(id)){
            return NotFound();
        }
        return NoContent();
    }


}