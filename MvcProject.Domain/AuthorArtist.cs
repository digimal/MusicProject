using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    public class AuthorArtist
    {
        public int AuthorId { get; set; }
        public virtual Author Author {get;set;}

        public int Position { get; set; }

        [Required]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public string Name { get; set; }

        public string JoinPhrase { get; set; }


    }
}
