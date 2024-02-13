using AutoMapper;
using Core.Repository;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class DirectorService : GenericRepository<Director>
{
    public DirectorService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}