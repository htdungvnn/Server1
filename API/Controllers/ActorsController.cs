using BLL.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ActorsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Actors
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
    {
        var actors = await _unitOfWork.Actors.GetAllAsync();
        return Ok(actors);
    }

    // GET: api/Actors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Actor>> GetActor(Guid id)
    {
        var actor = await _unitOfWork.Actors.GetByIdAsync(id);

        if (actor == null) return NotFound();

        return Ok(actor);
    }

    // POST: api/Actors
    [HttpPost]
    public async Task<ActionResult<Actor>> PostActor([FromBody] Actor actor)
    {
        await _unitOfWork.Actors.AddAsync(actor);
        return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
    }

    // PUT: api/Actors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutActor(Guid id, [FromBody] Actor actor)
    {
        if (id != actor.Id) return BadRequest();

        try
        {
            await _unitOfWork.Actors.UpdateAsync(actor);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Actors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActor(Guid id)
    {
        var actor = await _unitOfWork.Actors.GetByIdAsync(id);
        if (actor == null) return NotFound();

        await _unitOfWork.Actors.Remove(id);
        return NoContent();
    }
}