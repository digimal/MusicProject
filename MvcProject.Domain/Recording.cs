using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Domain
{
    public class Recording
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public virtual ICollection<FavoriteRecording> Fans { get; set; }
    }
}
