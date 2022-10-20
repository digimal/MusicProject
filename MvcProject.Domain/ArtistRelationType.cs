using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Domain
{
    public class ArtistRelationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string SourceMessage { get; set; }

        public string TargetMessage { get; set; }

        public virtual ICollection<ArtistRelation> Relations { get; set; }
    }
}
