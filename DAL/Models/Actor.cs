using Core.Entity;

namespace DAL.Models;

public class Actor : BaseEntity
{
    // [Key]
    //public Guid ActorId { get; set; }
    public string Name { get; set; }

    public string Bio { get; set; }
    //public virtual ICollection<MovieActor> MovieActors { get; set; }
}