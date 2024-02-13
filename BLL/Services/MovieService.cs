using AutoMapper;
using Core.Repository;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class MovieService : GenericRepository<Movie>
{
    public MovieService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}