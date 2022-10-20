using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MvcProject.Domain
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<FavoriteArtist> FavoriteArtists { get; set; }
        public virtual ICollection<FavoriteRecording> FavoriteRecordings { get; set; }

        public virtual ICollection<Log> Logs { get; set; }
    }

}
