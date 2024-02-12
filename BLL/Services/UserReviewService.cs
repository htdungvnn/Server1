using AutoMapper;
using Core.Repository;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class UserReviewService : GenericRepository<UserReview>
{
    public UserReviewService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}