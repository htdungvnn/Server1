using Core.Entity;

namespace DAL.Models;

public class UserReview : BaseEntity
{
    // [Key]
    // public Guid ReviewId { get; set; }
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; } // Assuming you have a User entity defined elsewhere
    public double Rating { get; set; }

    public string Comment { get; set; }
    // [ForeignKey("MovieId")]
    // public virtual Movie Movie { get; set; }
}