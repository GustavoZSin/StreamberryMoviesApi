using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Data
{
    public class StreamBerryContext : IdentityDbContext<User>
    {
        public StreamBerryContext(DbContextOptions<StreamBerryContext> opts) : base(opts)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MovieStreamingAssociation>()
                .HasKey(association => new { association.MovieId, association.StreamingServiceId });

            builder.Entity<MovieStreamingAssociation>()
                .HasOne(availableOnStreaming => availableOnStreaming.StreamingPlatform)
                .WithMany(streamingService => streamingService.ServicesOfStreaming)
                .HasForeignKey(availableOnStreaming => availableOnStreaming.StreamingServiceId);

            builder.Entity<MovieStreamingAssociation>()
                .HasOne(availableOnStreaming => availableOnStreaming.Movie)
                .WithMany(movie => movie.ServicesOfStreaming)
                .HasForeignKey(availableOnStreaming => availableOnStreaming.MovieId);

            builder.Entity<Rating>()
                .HasKey(rating => rating.Id);

            builder.Entity<Rating>()
                .Property(rating => rating.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Rating>()
                .HasOne(rating => rating.User)
                .WithMany(user => user.Ratings)
                .HasForeignKey(rating => rating.UserId);

            builder.Entity<Rating>()
                .HasOne(rating => rating.Movie)
                .WithMany(movie => movie.Ratings)
                .HasForeignKey(rating => rating.MovieId);

            builder.Entity<User>().ToTable("aspnetusers");
            builder.Entity<IdentityRole>().ToTable("aspnetroles");
            builder.Entity<IdentityUserRole<string>>().ToTable("aspnetuserroles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("aspnetuserclaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("aspnetuserlogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("aspnetroleclaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("aspnetusertokens");
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<StreamingPlatform> StreamingServices { get; set; }
        public DbSet<MovieStreamingAssociation> Movies_StreamingServices { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
