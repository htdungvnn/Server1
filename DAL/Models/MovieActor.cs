using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity;

namespace DAL.Models;

public class MovieActor:BaseEntity
{
    // MovieActors (Many-to-Many relationship between Movies and Actors)
    [Key, Column(Order = 0)]
    public Guid MovieId { get; set; }
    [Key, Column(Order = 1)]
    public Guid ActorId { get; set; }
    public string Role { get; set; }
    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; }
    [ForeignKey("ActorId")]
    public virtual Actor Actor { get; set; }
}