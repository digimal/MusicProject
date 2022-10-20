using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Domain
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }
    }
}
