using BLL.UnitOfWork;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public MoviesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
    {
        var movies = await _unitOfWork.Movies.GetAllAsync();
        return Ok(movies);
    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(Guid id)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);

        if (movie == null) return NotFound();

        return Ok(movie);
    }

    // POST: api/Movies
    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie([FromBody] Movie movie)
    {
        await _unitOfWork.Movies.AddAsync(movie);
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT: api/Movies/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(Guid id, [FromBody] Movie movie)
    {
        if (id != movie.Id) return BadRequest();

        try
        {
            await _unitOfWork.Movies.UpdateAsync(movie);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);
        if (movie == null) return NotFound();

        await _unitOfWork.Movies.Remove(id);
        return NoContent();
    }
}