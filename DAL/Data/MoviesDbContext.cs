using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class MoviesDbContext: DbContext
{
public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
{
}

public DbSet<Movie> Movies { get; set; }
public DbSet<Director> Directors { get; set; }
public DbSet<Genre> Genres { get; set; }
public DbSet<Actor> Actors { get; set; }
public DbSet<MovieActor> MovieActors { get; set; }
public DbSet<UserReview> UserReviews { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configure the primary key for the MovieActor join table.
    modelBuilder.Entity<MovieActor>()
        .HasKey(ma => new { ma.MovieId, ma.ActorId });

    // Configure many-to-many relationship between Movies and Actors
    modelBuilder.Entity<MovieActor>()
        .HasOne(ma => ma.Movie)
        .WithMany(m => m.MovieActors)
        .HasForeignKey(ma => ma.MovieId);

    modelBuilder.Entity<MovieActor>()
        .HasOne(ma => ma.Actor)
        .WithMany(a => a.MovieActors)
        .HasForeignKey(ma => ma.ActorId);

    // Add any additional configuration here
}
}