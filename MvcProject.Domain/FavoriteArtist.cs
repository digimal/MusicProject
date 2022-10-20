using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    public class FavoriteArtist
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
