using AutoMapper;
using Core.Repository;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class GenreService : GenericRepository<Genre>
{
    public GenreService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}