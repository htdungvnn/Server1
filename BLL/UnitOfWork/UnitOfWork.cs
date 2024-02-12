using AutoMapper;
using Core.Repository;
using DAL.Data;
using DAL.Models;

namespace BLL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly MoviesDbContext _context;
    private readonly IMapper _mapper;

    public UnitOfWork(MoviesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        ;
        Movies = new GenericRepository<Movie>(_context, _mapper);
        Directors = new GenericRepository<Director>(_context, _mapper);
        Genres = new GenericRepository<Genre>(_context, _mapper);
        Actors = new GenericRepository<Actor>(_context, _mapper);
        UserReviews = new GenericRepository<UserReview>(_context, _mapper);
    }

    public IGenericRepository<Movie> Movies { get; }
    public IGenericRepository<Director> Directors { get; }
    public IGenericRepository<Genre> Genres { get; }
    public IGenericRepository<Actor> Actors { get; }
    public IGenericRepository<UserReview> UserReviews { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}