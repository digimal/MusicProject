using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    public class ArtistAssignedTag
    {
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public int ArtistTagId { get; set; }
        public virtual ArtistTag ArtistTag { get; set; }

    }
}
