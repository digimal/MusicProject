using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Domain
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Aliases { get; set; }

        public string Description { get; set; }

        public int? PictureId { get; set; }
        public virtual Picture Picture { get; set; }

        public TimeInterval Interval { get; set; }

        public virtual ICollection<FavoriteArtist> Fans { get; set; }

        public virtual ICollection<AuthorArtist> Authorships { get; set; }

        public virtual ICollection<ArtistAssignedTag> AssignedTags { get; set; }

        public virtual ICollection<ArtistRelation> RelationsWhereSource { get; set; }

        public virtual ICollection<ArtistRelation> RelationsWhereTarget { get; set; }
    }
}
