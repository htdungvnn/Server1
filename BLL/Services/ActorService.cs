using AutoMapper;
using BLL.IServices;
using Core.Repository;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BLL.Services;

public class ActorService : GenericRepository<Actor>,IActorService
{
    public ActorService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}