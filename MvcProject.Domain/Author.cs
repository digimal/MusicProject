using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Domain
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public virtual ICollection<Recording> Recordings { get; set; }

        public virtual ICollection<AuthorArtist> Members { get; set; }
    }
}