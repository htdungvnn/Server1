using Core.Entity;

namespace DAL.Models;

public class Genre : BaseEntity
{
    // [Key]
    // public Guid GenreId { get; set; }
    public string Name { get; set; }
    //public virtual ICollection<Movie> Movies { get; set; }
}