using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcProject.Dal.Attributes;
using MvcProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Dal
{
    public class MusicContextSeed
    {
        public static async Task SeedAsync(MusicContext musicContext,
        ILogger logger,
        int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (musicContext.Database.IsSqlServer())
                {
                    musicContext.Database.Migrate();
                }

                //var methods = typeof(MusicContextSeed)
                //    .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                //    .Where(m => Attribute.IsDefined(m, typeof(OrderAttribute)))
                //    .OrderBy(m => ((OrderAttribute)m.GetCustomAttribute(typeof(OrderAttribute))).Order);

                //foreach (var method in methods)
                //{
                //    var args = method.ReturnType.GetGenericArguments();

                //    dynamic dbSet = Convert.ChangeType(musicContext.GetType().GetMethod("Set").MakeGenericMethod(args)
                //        .Invoke(musicContext, new object[0]), typeof(DbSet<>).MakeGenericType(args));

                //    if (!await dbSet.AnyAsync())
                //    {
                //        var data = method.Invoke(null, null);

                //        await (Task)dbSet.GetType().GetMethod("AddRangeAsync").Invoke(musicContext, data);

                //        await musicContext.SaveChangesAsync();
                //    }
                //}

                if (!await musicContext.Pictures.AnyAsync())
                {
                    await musicContext.Pictures.AddRangeAsync(
                        GetPictures());

                    await SaveChanges(musicContext, "Pictures");
                }

                if (!await musicContext.ArtistTags.AnyAsync())
                {
                    await musicContext.ArtistTags.AddRangeAsync(
                        GetArtistTags());

                    await SaveChanges(musicContext, "ArtistTags");
                }

                if (!await musicContext.ArtistRelationTypes.AnyAsync())
                {
                    await musicContext.ArtistRelationTypes.AddRangeAsync(
                        GetArtistRelationTypes());

                    await SaveChanges(musicContext, "ArtistRelationTypes");
                }

                if (!await musicContext.Artists.AnyAsync())
                {
                    await musicContext.Artists.AddRangeAsync(
                        GetArtists());

                    await SaveChanges(musicContext, "Artists");
                }

                if (!await musicContext.ArtistAssignedTags.AnyAsync())
                {
                    await musicContext.ArtistAssignedTags.AddRangeAsync(
                        GetArtistAssignedTags());

                    await SaveChanges(musicContext, "ArtistAssignedTags");
                }

                if (!await musicContext.ArtistRelations.AnyAsync())
                {
                    await musicContext.ArtistRelations.AddRangeAsync(
                        GetArtistRelations());

                    await SaveChanges(musicContext, "ArtistRelations");
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;

                logger.LogError(ex.Message);
                await SeedAsync(musicContext, logger, retryForAvailability);
                throw;
            }
        }

        static async Task SaveChanges(MusicContext musicContext, string name)
        {
            // await musicContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{name} ON");
            await musicContext.SaveChangesAsync();
            // await musicContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{name} OFF");
        }

        [Order]
        static IEnumerable<Picture> GetPictures() => new List<Picture>
        {
            new Picture() { Path = "/Pictures/Default/default-1.jpg" },
            new Picture() { Path = "/Pictures/220px-Indestructible.jpg" }
        };

        [Order]
        static IEnumerable<ArtistTag> GetArtistTags() => new List<ArtistTag>
        {
            new ArtistTag() { Name = "Fictional" },
            new ArtistTag() { Name = "Vocaloid" },
            new ArtistTag() { Name = "Rock" },
            new ArtistTag() { Name = "Rap" },
            new ArtistTag() { Name = "Pop" },
            new ArtistTag() { Name = "Jazz" },
            new ArtistTag() { Name = "Japan" }
        };

        [Order]
        static IEnumerable<ArtistRelationType> GetArtistRelationTypes() => new List<ArtistRelationType>
        {
            new ArtistRelationType() { Name = "Member", SourceMessage = "Groups", TargetMessage = "Members" },
            new ArtistRelationType() { Name = "Founder", SourceMessage = "Founded groups", TargetMessage = "Founders" }
        };

        [Order]
        static IEnumerable<Artist> GetArtists() => new List<Artist>
        {
            new Artist() { Name = "Nightwish", Aliases = "", Interval = new TimeInterval() { } },
            new Artist() { Name = "Five Finger Death Punch", Aliases = "FFDP, 5FPD", Interval = new TimeInterval() { } },
            new Artist() { Name = "System of a Down", Aliases = "SOAD", Interval = new TimeInterval() { } },
            new Artist() { Name = "Hatzune Miku", Aliases = "", Interval = new TimeInterval() { } },
            new Artist() { Name = "Disturbed", Aliases = "", PictureId = 2, Interval = new TimeInterval() { } },
            new Artist() { Name = "Device", Aliases = "", Interval = new TimeInterval() { } },
            new Artist() { Name = "David Dreyman", Aliases = "", Interval = new TimeInterval() { } }
        };

        [Order]
        static IEnumerable<ArtistAssignedTag> GetArtistAssignedTags() => new List<ArtistAssignedTag>
        {
            new ArtistAssignedTag() { ArtistId = 4, ArtistTagId = 1 },
            new ArtistAssignedTag() { ArtistId = 4, ArtistTagId = 2 }
        };

        [Order]
        static IEnumerable<ArtistRelation> GetArtistRelations() => new List<ArtistRelation>
        {
            new ArtistRelation() { SourceId = 7, TargetId = 5, TypeId = 1, Interval = new TimeInterval() { } },
            new ArtistRelation() { SourceId = 7, TargetId = 6, TypeId = 1, Interval = new TimeInterval() { } },
            new ArtistRelation() { SourceId = 7, TargetId = 6, TypeId = 2, Interval = new TimeInterval() { } }
        };
    }
}
