using AutoMapper;
using BLL.IServices;
using Core.Repository;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ActorService : GenericRepository<Actor>, IActorService
{
    public ActorService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}