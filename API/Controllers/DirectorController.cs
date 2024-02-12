using BLL.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DirectorController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Directors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
    {
        var directors = await _unitOfWork.Directors.GetAllAsync();
        return Ok(directors);
    }

    // GET: api/Director/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Director>> GetDirector(Guid id)
    {
        var director = await _unitOfWork.Directors.GetByIdAsync(id);

        if (director == null) return NotFound();

        return Ok(director);
    }

    // POST: api/Director
    [HttpPost]
    public async Task<ActionResult<Director>> PostDirector([FromBody] Director director)
    {
        await _unitOfWork.Directors.AddAsync(director);
        return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
    }

    // PUT: api/Director/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDirector(Guid id, [FromBody] Director director)
    {
        if (id != director.Id) return BadRequest();

        try
        {
            await _unitOfWork.Directors.UpdateAsync(director);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Director/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDirector(Guid id)
    {
        var director = await _unitOfWork.Directors.GetByIdAsync(id);
        if (director == null) return NotFound();

        await _unitOfWork.Directors.Remove(id);
        return NoContent();
    }
}