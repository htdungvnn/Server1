using AutoMapper;
using Core.EFRepository;
using Core.Repository;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BLL.Services;

public class GenreService : GenericRepository<Genre>
{
    public GenreService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}