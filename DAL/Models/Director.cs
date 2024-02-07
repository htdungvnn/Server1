using System.ComponentModel.DataAnnotations;
using Core.Entity;

namespace DAL.Models;

public class Director:BaseEntity
{
    [Key]
    public Guid DirectorId { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public virtual ICollection<Movie> Movies { get; set; }
}