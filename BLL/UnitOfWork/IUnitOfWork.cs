using Core.Repository;
using DAL.Models;

namespace BLL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Movie> Movies { get; }
    IGenericRepository<Director> Directors { get; }
    IGenericRepository<Genre> Genres { get; }
    IGenericRepository<Actor> Actors { get; }
    IGenericRepository<UserReview> UserReviews { get; }

    Task<int> CompleteAsync();
}