using BLL.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public GenresController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Genres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        var genres = await _unitOfWork.Genres.GetAllAsync();
        return Ok(genres);
    }

    // GET: api/Genres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(Guid id)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(id);

        if (genre == null) return NotFound();

        return Ok(genre);
    }

    // POST: api/Genres
    [HttpPost]
    public async Task<ActionResult<Genre>> PostGenre([FromBody] Genre genre)
    {
        await _unitOfWork.Genres.AddAsync(genre);
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }

    // PUT: api/Genres/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenre(Guid id, [FromBody] Genre genre)
    {
        if (id != genre.Id) return BadRequest();

        try
        {
            await _unitOfWork.Genres.UpdateAsync(genre);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Genres/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(id);
        if (genre == null) return NotFound();

        await _unitOfWork.Genres.Remove(id);
        return NoContent();
    }
}