using Core.Entity;

namespace DAL.Models;

public class Movie : BaseEntity
{
    // [Key]
    // public Guid MovieId { get; set; }
    public string Title { get; set; }
    public Guid DirectorId { get; set; }
    public DateTime ReleaseDate { get; set; }

    public Guid GenreId { get; set; }
    // [ForeignKey("DirectorId")]
    //public virtual Director Director { get; set; }
    // [ForeignKey("GenreId")]
    // public virtual Genre Genre { get; set; }
    // public virtual ICollection<MovieActor> MovieActors { get; set; }
    // public virtual ICollection<UserReview> UserReviews { get; set; }
}