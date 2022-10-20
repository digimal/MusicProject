using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcProject.Domain;

namespace MvcProject.Dal
{
    public class MusicContext : IdentityDbContext<User, Role, int>
    {
        public MusicContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtistRelation>()
                .HasOne(e => e.Source)
                .WithMany(s => s.RelationsWhereSource)
                .HasForeignKey(k => k.SourceId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ArtistRelation>()
                .HasOne(e => e.Target)
                .WithMany(s => s.RelationsWhereTarget)
                .HasForeignKey(k => k.TargetId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Artist>()
                .HasOne(e => e.Picture)
                .WithMany(s => s.Artists)
                .HasForeignKey(k => k.PictureId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ArtistAssignedTag>().HasKey(e => new { e.ArtistId, e.ArtistTagId });
            modelBuilder.Entity<ArtistRelation>().HasKey(e => new { e.SourceId, e.TargetId, e.TypeId });
            modelBuilder.Entity<AuthorArtist>().HasKey(e => new { e.AuthorId, e.ArtistId });
            modelBuilder.Entity<FavoriteArtist>().HasKey(e => new { e.UserId, e.ArtistId });
            modelBuilder.Entity<FavoriteRecording>().HasKey(e => new { e.UserId, e.RecordingId });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.OwnsOne(e => e.Interval);
            });

            modelBuilder.Entity<ArtistRelation>(entity =>
            {
                entity.OwnsOne(e => e.Interval);
            });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistTag> ArtistTags { get; set; }
        public DbSet<ArtistAssignedTag> ArtistAssignedTags { get; set; }
        public DbSet<ArtistRelationType> ArtistRelationTypes { get; set; }
        public DbSet<ArtistRelation> ArtistRelations { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorArtist> AuthorArtists { get; set; }
        public DbSet<Recording> Recordings { get; set; }
        public DbSet<FavoriteArtist> FavouriteArtists { get; set; }
        public DbSet<FavoriteRecording> FavouriteRecordings { get; set; }
        public DbSet<Picture> Pictures { get;set; }
        public DbSet<Log> Logs { get; set; }


    }
}
